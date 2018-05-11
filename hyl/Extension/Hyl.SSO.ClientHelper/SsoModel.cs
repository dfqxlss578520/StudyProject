using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hyl.SSO.ClientHelper
{
    public class SsoModel
    {
        public SsoModel()
        {
            Appkey = ConfigurationSettings.AppSettings["SsoAppId"];
            Appsecret = ConfigurationSettings.AppSettings["SsoAppSecret"];
            SsoBaseUrl = ConfigurationSettings.AppSettings["SsoHost"];
            CompanyId = ConfigurationSettings.AppSettings["SsoCompanyId"];
        }

        #region Property
        private string _appkey;
        /// <summary>
        /// 应用Appkey
        /// </summary>
        public string Appkey
        {
            get { return _appkey; }
            set { _appkey = value; }
        }


        private string _appsecret;
        /// <summary>
        /// 应用Appsecret
        /// </summary>
        public string Appsecret
        {
            get { return _appsecret; }
            set { _appsecret = value; }
        }


        private string _apptoken;
        /// <summary>
        /// 应用Token
        /// </summary>
        private string AppToken
        {
            get { return _apptoken; }
            set { _apptoken = value; }
        }


        private string _hylid;
        /// <summary>
        /// SSO-server返回的身份信息
        /// </summary>
        public string Hylid
        {
            get { return _hylid; }
            set { _hylid = value; }
        }

        private string _companyid;
        /// <summary>
        /// SSO-server中公司ID，是为了解决多系统账号名相同的问题，如admin账号
        /// </summary>
        public string CompanyId
        {
            get { return _companyid; }
            set { _companyid = value; }
        }

        private string _ssobaseurl;
        /// <summary>
        /// 单点登录服务器host
        /// 例:http://localhost:8021/
        /// </summary>
        public string SsoBaseUrl
        {
            get { return _ssobaseurl; }
            set { _ssobaseurl = value; }
        }

        private string _jsonstr;
        /// <summary>
        /// 验证成功查询用户身份json
        /// </summary>
        public string JsonStr
        {
            get { return _jsonstr; }
            set { _jsonstr = value; }
        }
        #endregion



        #region Utility
        private string EncryptMD5(string source)
        {
            source = source + "f1114e97-f765-40a3-9acf-4c3951fc32f1";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(source);
            byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
            md5.Clear();
            (md5 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }

        //默认的密钥和向量
        private const string RijndaelKey = "UFCy76*h%(HilJjkP87jH7Guz(%&hj7x89H$yuBI0456FtmaT5&fvH$lhj!y6&(*";
        private const string RijndaelIV = "E4ghj*GhGUY86GfghUb#er57HBh(u%gg7!rNIfhg4ui%$hjkb&956HJ($jhWk7&!";

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey(string strKey)
        {
            SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
            string sTemp = strKey;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV(string strVI)
        {
            SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
            string sTemp = strVI;
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 解密方法（使用系统默认密钥和向量）
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="Source">向量</param>
        /// <returns>经过解密的串</returns>
        private string DecrypRijndael(string Source, string strKey, string strIV)
        {
            if (String.IsNullOrEmpty(Source)) return "";
            try
            {
                using (SymmetricAlgorithm mobjCryptoService = new RijndaelManaged())
                {
                    byte[] bytIn = Convert.FromBase64String(Source);
                    using (MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length))
                    {
                        mobjCryptoService.Key = GetLegalKey(strKey);
                        mobjCryptoService.IV = GetLegalIV(strIV);
                        using (ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read))
                            {
                                using (StreamReader sr = new StreamReader(cs))
                                { return sr.ReadToEnd(); }
                            }
                        }
                    }
                }
            }
            catch
            { return ""; }
        }
        /// <summary>
        /// 解密方法（使用系统默认密钥和向量）
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string DecrypRijndael(string Source)
        {
            return DecrypRijndael(Source, RijndaelKey, RijndaelIV);
        }
        /// <summary>
        /// URL字符解码
        /// </summary>
        public string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }
        #endregion



        #region Method

        /// <summary>
        /// 校验SSO-server返回的身份信息
        /// </summary>
        /// <returns></returns>
        public string ValidateHylId()
        {
            if (string.IsNullOrEmpty(Hylid))
            {
                return "未设置Hylid信息";
            }
            if (string.IsNullOrEmpty(Appkey))
            {
                return "未设置Appkey信息";
            }
            if (string.IsNullOrEmpty(Appsecret))
            {
                return "未设置Appsecret信息";
            }
            if (string.IsNullOrEmpty(SsoBaseUrl))
            {
                return "未设置SsoBaseUrl信息";
            }
            AppToken = EncryptMD5(Appkey + Appsecret);
            if (!SsoBaseUrl.EndsWith("/"))
            {
                SsoBaseUrl += "/";
            }
            JsonStr = HttpUtils.PostUrl(string.Format("{0}Authorize/validate", SsoBaseUrl), string.Format("hylid={0}&Appkey={1}&token={2}", Hylid, Appkey, AppToken));
            return JsonStr;
        }

        /// <summary>
        /// 获取SSO-server的当前登录信息
        /// </summary>
        /// <returns></returns>
        public void TryGetUserInfo(string returnUrl)
        {
            TryGetUserInfo(returnUrl, string.Empty, string.Empty);
        }

        /// <summary>
        /// 获取SSO-server的当前登录信息
        /// </summary>
        /// <returns></returns>
        public void TryGetUserInfo(string returnUrl, string openid, string unionid)
        {
            AppToken = EncryptMD5(Appkey + Appsecret);
            if (!SsoBaseUrl.EndsWith("/"))
            {
                SsoBaseUrl += "/";
            }
            HttpContext.Current.Response.Redirect(string.Format("{0}Authorize/TryGetUserInfo?Appkey={1}&token={2}&returnurl={3}&openid={4}&unionid={5}", SsoBaseUrl, Appkey, AppToken, returnUrl, openid, unionid));
        }

        /// <summary>
        /// 是否需要验证SSO-server返回的身份信息
        /// 如果需要，则自动为Hylid赋值
        /// </summary>
        /// <returns></returns>
        public bool IsNeedValidateHylId()
        {
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["hylid"]))
                {
                    Hylid = HttpContext.Current.Request.QueryString["hylid"];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 跳转到SSO登录页
        /// </summary>
        /// <param name="returnUrl"></param>
        public void ToSsoPage(string returnUrl)
        {
            if (string.IsNullOrEmpty(CompanyId))
            {
                HttpContext.Current.Response.Redirect(string.Format("{0}Authorize?returnurl={1}", SsoBaseUrl, returnUrl));
            }
            else
            {
                HttpContext.Current.Response.Redirect(string.Format("{0}Authorize?returnurl={1}&CompanyId={2}", SsoBaseUrl, returnUrl, CompanyId));
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void LoginOut(string returnUrl)
        {
            //AppToken = EncryptMD5(Appkey + Appsecret);
            //if (!SsoBaseUrl.EndsWith("/"))
            //{
            //    SsoBaseUrl += "/";
            //}
            //JsonStr = HttpUtils.PostUrl(string.Format("{0}Authorize/logout", SsoBaseUrl), string.Format("Appkey={0}&token={1}", Appkey, AppToken ));
            //return JsonStr;
            HttpContext.Current.Response.Redirect(string.Format("{0}Authorize/logout?returnurl={1}", SsoBaseUrl, returnUrl));
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        /// <returns></returns>
        public void AutoLogin(string registerName, string registerpassword, string returnurl)
        {
            AppToken = EncryptMD5(Appkey + Appsecret);
            if (!SsoBaseUrl.EndsWith("/"))
            {
                SsoBaseUrl += "/";
            }
            string url = string.Empty;
            if (!string.IsNullOrEmpty(CompanyId))
            {
                url = string.Format("{0}Authorize/AutoLogin?Appkey={1}&token={2}&username={3}&Password={4}&returnUrl={5}&CompanyId={6}",
                    SsoBaseUrl, Appkey, AppToken, registerName, registerpassword, returnurl, CompanyId);
            }
            else
            {
                url = string.Format("{0}Authorize/AutoLogin?Appkey={1}&token={2}&username={3}&Password={4}&returnUrl={5}",
                    SsoBaseUrl, Appkey, AppToken, registerName, registerpassword, returnurl);
            }
            HttpContext.Current.Response.Redirect(url);
        }

        /// <summary>
        /// 尝试注册新账户，如果没有则新注册
        /// </summary>
        /// <returns></returns>
        public string TryRegiste(string registerName, string registerpassword)
        {
            if (string.IsNullOrEmpty(Appkey))
            {
                return "未设置Appkey信息";
            }
            if (string.IsNullOrEmpty(Appsecret))
            {
                return "未设置Appsecret信息";
            }
            if (string.IsNullOrEmpty(SsoBaseUrl))
            {
                return "未设置SsoBaseUrl信息";
            }
            if (string.IsNullOrEmpty(registerName))
            {
                return "未设置注册用户名信息";
            }
            AppToken = EncryptMD5(Appkey + Appsecret);
            if (!SsoBaseUrl.EndsWith("/"))
            {
                SsoBaseUrl += "/";
            }
            string parameter = string.Empty;
            if (!string.IsNullOrEmpty(CompanyId))
            {
                parameter = string.Format("Appkey={0}&token={1}&username={2}&Password={3}&CompanyId={4}", Appkey, AppToken, registerName, registerpassword, CompanyId);
            }
            else
            {
                parameter = string.Format("Appkey={0}&token={1}&username={2}&Password={3}", Appkey, AppToken, registerName, registerpassword);
            }
            JsonStr = HttpUtils.PostUrl(string.Format("{0}Authorize/TryRegister", SsoBaseUrl), parameter);
            return JsonStr;
        }

        /// <summary>
        /// 尝试绑定SNS账号信息
        /// </summary>
        /// <returns></returns>
        public string TryBindSnsRelation(string openid, long uid)
        {
            if (string.IsNullOrEmpty(Appkey))
            {
                return "未设置Appkey信息";
            }
            if (string.IsNullOrEmpty(Appsecret))
            {
                return "未设置Appsecret信息";
            }
            if (string.IsNullOrEmpty(SsoBaseUrl))
            {
                return "未设置SsoBaseUrl信息";
            }
            if (string.IsNullOrEmpty(openid))
            {
                return "未设置SNS账号Openid";
            }
            AppToken = EncryptMD5(Appkey + Appsecret);
            if (!SsoBaseUrl.EndsWith("/"))
            {
                SsoBaseUrl += "/";
            }
            JsonStr = HttpUtils.PostUrl(string.Format("{0}Authorize/TryBindSnsRelation", SsoBaseUrl), string.Format("Appkey={0}&token={1}&openid={2}&uid={3}", Appkey, AppToken, openid, uid));
            return JsonStr;
        }
        #endregion




    }
}
