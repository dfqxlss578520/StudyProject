using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Hyl.Core.Helpers.Utility
{
    public class Encrypt
    {
        protected static string EncryptStr = "ZYXWVUTSRQPONMLKJIHGFEDCBA_zyxwvutsrqponmlkjihgfedcba*9876543210!@#$%^&";
        public static string StrongEncrypt(string salt,string str)
        {
            return EncryptMD5(salt + EncryptStr + str);
        }

        #region MD5
        public static string EncryptMD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(source);
            byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
            md5.Clear();
            (md5 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }

        /// <summary>
        /// 与JAVA统一的MD5方法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Java(string source)
        {
            return MD5Java(source, "UTF-8");
        }
        public static string MD5Java(string source, string charset)
        {
            return MD5Java(Encoding.GetEncoding(charset).GetBytes(source));
        }
        /// <summary>
        /// 与JAVA统一的MD5方法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Java(byte[] source)
        {
            StringBuilder result = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(source);
                for (int i = 0; i < bytes.Length; i++)
                {
                    string hex = bytes[i].ToString("X");
                    if (hex.Length == 1)
                    {
                        result.Append("0");
                    }
                    result.Append(hex);
                }
            }
            return result.ToString();
        }
        public static byte[] MD5Hash(string source)
        {
            return MD5Hash(source, "UTF-8");
        }
        public static byte[] MD5Hash(string source, string charset)
        {
            return MD5Hash(Encoding.GetEncoding(charset).GetBytes(source));
        }
        public static byte[] MD5Hash(byte[] source)
        {
            byte[] bytes = null;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(source);
            }
            return bytes;
        }

        /// <summary>
        /// 获取STRING字符串的MD5参数
        /// </summary>
        /// <param name="WebString"></param>
        /// <returns></returns>
        public static string GetStringMD5(string WebString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(WebString)))
                {
                    System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] hash = md5.ComputeHash(ms);
                    foreach (byte b in hash)
                    {
                        sb.Append(String.Format("{0:X1}", b));
                    }
                    ms.Close();
                }
                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5Simple(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }


        #endregion

        #region 缩减函数


        //缩减参数
        public static string TrimEncodeStr(long Number)
        {
            StringBuilder sBuilder = new StringBuilder();
            long N = Number;
            while (N > 0)
            {
                sBuilder.Insert(0, Chr((int)(N % 26 + 97)));
                N = N / 26;
            }
            return sBuilder.ToString();
        }



        ////缩减参数
        //public static string TrimEncodeStr(int Number)
        //{
        //    StringBuilder sBuilder = new StringBuilder();
        //    int N = Number;
        //    while (N > 0)
        //    {
        //        sBuilder.Insert(0, Chr(N % 26 + 97));
        //        N = N / 26;
        //    }
        //    return sBuilder.ToString();
        //}

        //还原参数
        public static long TrimDecodeStr(string Str)
        {
            char[] t = Str.ToLower().ToCharArray();
            long ReturnInt = 0;
            for (int i = 0; i < t.Length; i++)
            {
                ReturnInt += (Asc(t[i].ToString()) - 97) * (long)System.Math.Pow(26.0, (double)(t.Length - i - 1));
            }
            return ReturnInt;
        }

        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                return "";
            }
        }

        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else if (character.Length == 2)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int hAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                int lAsciiCode = (int)asciiEncoding.GetBytes(character)[1];
                return ((256 * hAsciiCode + lAsciiCode) - 256 * 256);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 随机字符串

        /// <summary>
        /// 获取随机密码
        /// </summary>
        /// <param name="codeLen">密码长度</param>
        /// <returns></returns>
        public static string GetRndPsw(int codeLen)
        {
            codeLen = codeLen > 9 ? 9 : codeLen;
            Random rnd = new Random();
            return String.Format("{0:D" + codeLen + "}", rnd.Next((int)System.Math.Pow((double)10, (double)codeLen)));
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public static string GetRndStr(int codeLen)
        {
            Random rnd = new Random();
            var maxPos = EncryptStr.Length;
            var ret = "";
            for (int i = 0; i < codeLen; i++)
            {
                ret += EncryptStr[rnd.Next(maxPos)];
            }
            return ret;
        }

        #endregion
        #region Rijndael加密算法

        //默认的密钥和向量
        private const string RijndaelKey = "UFCy76*h%(HilJjkP87jH7Guz(%&hj7x89H$yuBI0456FtmaT5&fvH$lhj!y6&(*";
        private const string RijndaelIV = "E4ghj*GhGUY86GfghUb#er57HBh(u%gg7!rNIfhg4ui%$hjkb&956HJ($jhWk7&!";

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private static byte[] GetLegalKey(string strKey)
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
        private static byte[] GetLegalIV(string strVI)
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
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="Source">向量</param>
        /// <returns>经过加密的串</returns>
        public static string EncrypRijndael(string Source, string strKey, string strIV)
        {
            try
            {
                using (SymmetricAlgorithm mobjCryptoService = new RijndaelManaged())
                {
                    byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        mobjCryptoService.Key = GetLegalKey(strKey);
                        mobjCryptoService.IV = GetLegalIV(strIV);
                        using (ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write))
                            {
                                cs.Write(bytIn, 0, bytIn.Length);
                                cs.FlushFinalBlock();
                                ms.Close();
                                byte[] bytOut = ms.ToArray();
                                return Convert.ToBase64String(bytOut);
                            }
                        }
                    }
                }
            }
            catch
            { return ""; }
        }
        /// <summary>
        /// 加密方法（使用系统默认密钥和向量）
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public static string EncrypRijndael(string Source)
        {
            return EncrypRijndael(Source, RijndaelKey, RijndaelIV);
        }

        /// <summary>
        /// 加密方法（使用系统默认向量）
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <param name="Key">钥匙</param>
        /// <returns>经过加密的串</returns>
        public static string EncrypRijndael(string Source, string Key)
        {
            return EncrypRijndael(Source, Key, RijndaelIV);
        }

        /// <summary>
        /// 解密方法（使用系统默认密钥和向量）
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="Source">向量</param>
        /// <returns>经过解密的串</returns>
        public static string DecrypRijndael(string Source, string strKey, string strIV)
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
        public static string DecrypRijndael(string Source)
        {
            return DecrypRijndael(Source, RijndaelKey, RijndaelIV);
        }

        /// <summary>
        /// 解密方法（使用系统默认向量）
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <param name="Key">钥</param>
        /// <returns>经过解密的串</returns>
        public static string DecrypRijndael(string Source, string Key)
        {
            return DecrypRijndael(Source, Key, RijndaelIV);
        }
        #endregion

        #region AES加密算法
        #region AES
        //默认密钥向量
        private static byte[] AESKeys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        public static string AESEncode(string encryptString, string encryptKey)
        {
            encryptKey = Utils.GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = AESKeys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        public static string AESDecode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = Utils.GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = AESKeys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }
        #endregion
        /// <summary>
        /// 使用AES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密字符串</param>
        /// <param name="encryptKey">加密密匙</param>
        /// <param name="salt">盐</param>
        /// <returns>加密结果，加密失败则返回源串</returns>
        public static string EncryptAES(string encryptString, string encryptKey, string salt)
        {
            AesManaged aes = null;
            MemoryStream ms = null;
            CryptoStream cs = null;

            try
            {
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(encryptKey, Encoding.UTF8.GetBytes(salt));

                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] data = Encoding.UTF8.GetBytes(encryptString);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return encryptString;
            }
            finally
            {
                if (cs != null)
                    cs.Close();

                if (ms != null)
                    ms.Close();

                if (aes != null)
                    aes.Clear();
            }
        }

        /// <summary>
        /// 使用AES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密字符串</param>
        /// <param name="decryptKey">解密密匙</param>
        /// <param name="salt">盐</param>
        /// <returns>解密结果，解谜失败则返回源串</returns>
        public static string DecryptAES(string decryptString, string decryptKey, string salt)
        {
            AesManaged aes = null;
            MemoryStream ms = null;
            CryptoStream cs = null;

            try
            {
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(decryptKey, Encoding.UTF8.GetBytes(salt));

                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

                byte[] data = Convert.FromBase64String(decryptString);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
            }
            catch
            {
                return decryptString;
            }
            finally
            {
                if (cs != null)
                    cs.Close();

                if (ms != null)
                    ms.Close();

                if (aes != null)
                    aes.Clear();
            }
        }


        #endregion

        #region DES
        //默认密钥向量
        private static byte[] DESKeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string DESEncode(string encryptString, string encryptKey)
        {
            encryptKey = Utils.GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = DESKeys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string DESDecode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = Utils.GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = DESKeys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region GZip
        /// <summary>   
        /// Takes a binary input buffer and GZip encodes the input   
        /// </summary>   
        /// <param name="Buffer"></param>   
        /// <returns></returns>   
        public static byte[] GZipMemory(byte[] Buffer)
        {

            MemoryStream ms = new MemoryStream();
            GZipStream GZip = new GZipStream(ms, CompressionMode.Compress);
            GZip.Write(Buffer, 0, Buffer.Length);
            GZip.Close();
            byte[] Result = ms.ToArray();
            ms.Close();
            return Result;

        }

        /// <summary>
        /// GZIP压缩
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static byte[] GZipMemory(string Input, Encoding encode)
        {
            return GZipMemory(encode.GetBytes(Input));
        }
        #endregion

        #region Encode/Decode -> Base64(UTF-8)
        /// <summary>
        /// 编码URL为BASE64
        /// </summary>
        /// <param name="code">编码的URL</param>
        /// <returns>BASE64编码后的页面</returns>
        public static string EncodeBase64(string code)
        {
            try
            {
                string encode = "";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(code);
                //由于在本地保存缓存文件的时候文件名不能超过255，而有些URL地址很大，所以需要进行反转操作
                //Array.Reverse(bytes);
                try
                {
                    encode = Convert.ToBase64String(bytes);
                }
                catch
                {
                    encode = code;
                }
                return encode;
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// 解码BASE64的URL
        /// </summary>
        /// <param name="code">BASE64字串</param>
        /// <returns>解码后的URL</returns>
        public static string DecodeBase64(string code)
        {
            if (code.IndexOf("://") != -1)
            {
                //包含base64不支持的字符串，特别是url，直接返回，避免触发try catch
                return code;
            }
            string decode = "";
            //由于在本地保存缓存文件的时候文件名不能超过255，而有些URL地址很大，所以需要进行反转操作
            //Array.Reverse(bytes);
            try
            {
                byte[] bytes = Convert.FromBase64String(code);
                decode = System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region Encode/Decode -> Base64(GB2312)
        /// <summary>
        /// 编码URL为BASE64
        /// </summary>
        /// <param name="code">编码的URL</param>
        /// <returns>BASE64编码后的页面</returns>
        public static string EncodeBase64GB2312(string code)
        {
            try
            {
                string encode = "";
                byte[] bytes = System.Text.Encoding.Default.GetBytes(code);
                //由于在本地保存缓存文件的时候文件名不能超过255，而有些URL地址很大，所以需要进行反转操作
                //Array.Reverse(bytes);
                try
                {
                    encode = Convert.ToBase64String(bytes);
                }
                catch
                {
                    encode = code;
                }
                return encode;
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// 解码BASE64的URL
        /// </summary>
        /// <param name="code">BASE64字串</param>
        /// <returns>解码后的URL</returns>
        public static string DecodeBase64GB2312(string code)
        {
            string decode = "";
            //由于在本地保存缓存文件的时候文件名不能超过255，而有些URL地址很大，所以需要进行反转操作
            //Array.Reverse(bytes);
            try
            {
                byte[] bytes = Convert.FromBase64String(code);
                decode = System.Text.Encoding.Default.GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region 更少字符缩减函数

        //更少字符的缩减参数
        public static string TrimEncodeStr2(long Number)
        {
            StringBuilder sBuilder = new StringBuilder();
            long N = Number;
            while (N > 0)
            {
                sBuilder.Insert(0, EncryptStr[(int)(N % 62)]);
                N = N / 62;
            }
            return sBuilder.ToString();
        }

        //还原参数
        public static long TrimDecodeStr2(string Str)
        {
            long ReturnInt = 0L;
            for (int i = 0; i < Str.Length; i++)
            {
                int j = 0;
                for (j = 0; j < 62; j++)
                {
                    if (EncryptStr[j] == Str[i])
                        break;
                }
                if (j < 62)
                {
                    ReturnInt += j * (long)System.Math.Pow(62.0, (double)(Str.Length - i - 1));
                }
            }
            return ReturnInt;
        }

        #endregion

        #region DES加密解密

        public static string DesEncrypt(string encryptString, string key, string iv)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = Encoding.UTF8.GetBytes(iv);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static string DesDecrypt(string decryptString, string key, string iv)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = Encoding.UTF8.GetBytes(iv);
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        #endregion

        #region HMAC-SHA512
        private const string HMACSHA512Key = "K3v%2b8)fZ3Vn_Op0mVKls^3.L1L`~kF_#xj<C;(lQhP*vWl%rQox)pEl-oCir@R";
        public static string HMACSHA512(string inputText)
        {
            return HMACSHA512(inputText, HMACSHA512Key);
        }
        public static string HMACSHA512(string inputText, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytKey = encoding.GetBytes(key);
            HMACSHA512 hmacsha512 = new HMACSHA512(bytKey);
            byte[] hashValue = hmacsha512.ComputeHash(encoding.GetBytes(inputText));
            hmacsha512.Clear();
            return Convert.ToBase64String(hashValue);
        }
        #endregion

        #region SHA1

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA1Hash(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        #endregion

        #region SHA256,SHA512
        private const string SHA256HashSalt = "3r&nH_x!L:T)e#VkQoZp@0oE%s$^Gu-(vT%e@mPq%x+f~)zUhY*J,iCWg(ZS#(gE";
        public static string SHA256_Hash(string inputText)
        {
            return SHA256_Hash(inputText, SHA256HashSalt);
        }
        public static string SHA256_Hash(string inputText, string salt)
        {
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] bytIn = UTF8Encoding.Default.GetBytes(string.Concat(inputText, salt));
                byte[] bytOut = sha256.ComputeHash(bytIn);
                return BitConverter.ToString(bytOut);
            }
            catch { return ""; }
        }
        private const string SHA512HashSalt = "r2J,>f3pC=B(*i:Eh|]n%+%akAL(o]J$#Iz81*e$H&]C6i[r8D@t#Vl_Qs,Uq@9I";
        public static string SHA512_Hash(string inputText)
        {
            return SHA512_Hash(inputText, SHA512HashSalt);
        }
        public static string SHA512_Hash(string inputText, string salt)
        {
            try
            {
                SHA512 sha512 = new SHA512CryptoServiceProvider();
                byte[] bytIn = UTF8Encoding.Default.GetBytes(string.Concat(inputText, salt));
                byte[] bytOut = sha512.ComputeHash(bytIn);
                return BitConverter.ToString(bytOut);
            }
            catch { return ""; }
        }
        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }
        #endregion

        #region RSA

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="inputText">待加密的数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>RSA公钥加密后的数据</returns>
        public static String EncryptRSA(String inputText, string publicKey)
        {
            try
            {
                var provider = GetRSAServiceProvider();
                provider.FromXmlString(publicKey);
                Byte[] plainTxtData = new UnicodeEncoding().GetBytes(inputText);
                int maxBlockSize = provider.KeySize / 8 - 11;

                if (plainTxtData.Length <= maxBlockSize)
                { return Convert.ToBase64String(provider.Encrypt(plainTxtData, false)); }

                using (MemoryStream plainStream = new MemoryStream(plainTxtData))
                {
                    using (MemoryStream cryptStream = new MemoryStream())
                    {
                        Byte[] buffer = new Byte[maxBlockSize];
                        int blockSize = plainStream.Read(buffer, 0, maxBlockSize);
                        while (blockSize > 0)
                        {
                            Byte[] toEncrypt = new Byte[blockSize];
                            Array.Copy(buffer, 0, toEncrypt, 0, blockSize);

                            Byte[] cryptograph = provider.Encrypt(toEncrypt, false);
                            cryptStream.Write(cryptograph, 0, cryptograph.Length);

                            blockSize = plainStream.Read(buffer, 0, maxBlockSize);
                        }
                        return Convert.ToBase64String(cryptStream.ToArray(), Base64FormattingOptions.None);
                    }
                }
            }
            catch { return ""; }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="inputText">待解密的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>解密后字符串</returns>
        public static String DecryptRSA(String inputText, string privateKey)
        {
            try
            {
                var provider = GetRSAServiceProvider();

                provider.FromXmlString(privateKey);
                Byte[] cipherTxtData = Convert.FromBase64String(inputText);
                int maxBlockSize = provider.KeySize / 8;

                if (cipherTxtData.Length <= maxBlockSize)
                { return new UnicodeEncoding().GetString(provider.Decrypt(cipherTxtData, false)); }

                using (MemoryStream cryptStream = new MemoryStream(cipherTxtData))
                {
                    using (MemoryStream plainStream = new MemoryStream())
                    {
                        Byte[] buffer = new Byte[maxBlockSize];
                        int blockSize = cryptStream.Read(buffer, 0, maxBlockSize);
                        while (blockSize > 0)
                        {
                            Byte[] toDecrypt = new Byte[blockSize];
                            Array.Copy(buffer, 0, toDecrypt, 0, blockSize);

                            Byte[] plaintext = provider.Decrypt(toDecrypt, false);
                            plainStream.Write(plaintext, 0, plaintext.Length);

                            blockSize = cryptStream.Read(buffer, 0, maxBlockSize);
                        }
                        return new UnicodeEncoding().GetString(plainStream.ToArray());
                    }
                }
            }
            catch { return ""; }
        }

        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="inputText">待签名的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名后字符串</returns>
        public static string SignRSA(string inputText, string privateKey)
        {
            return SignRSA(inputText, privateKey, "unicode");
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="inputText">待签名的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="inputCharset">编码格式</param>
        /// <returns>签名后字符串</returns>
        public static string SignRSA(string inputText, string privateKey, string inputCharset)
        {
            try
            {
                var rsaProvider = GetRSAServiceProvider();
                using (var sha1Provider = new SHA1CryptoServiceProvider())
                {
                    var data = Encoding.GetEncoding(inputCharset).GetBytes(inputText);
                    rsaProvider.FromXmlString(privateKey);
                    return BitConverter.ToString(rsaProvider.SignData(data, sha1Provider));
                }
            }
            catch { return ""; }
        }

        public static string SignRSA2Base64(string inputText, string privateKey, string inputCharset)
        {
            try
            {
                var rsaProvider = GetRSAServiceProvider();
                using (var sha1Provider = new SHA1CryptoServiceProvider())
                {
                    var data = Encoding.GetEncoding(inputCharset).GetBytes(inputText);
                    rsaProvider.FromXmlString(privateKey);
                    return Convert.ToBase64String(rsaProvider.SignData(data, sha1Provider));
                }
            }
            catch { return ""; }
        }
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="inputText">待验证的签名</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public static bool VerifyRSA(string inputText, string publicKey, string sign)
        {
            return VerifyRSA(inputText, publicKey, sign, "unicode");
        }
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="inputText">待验证的签名</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <param name="inputCharset">编码格式</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public static bool VerifyRSA(string inputText, string publicKey, string sign, string inputCharset)
        {
            try
            {
                var rsaProvider = GetRSAServiceProvider();
                using (var sha1Provider = new SHA1CryptoServiceProvider())
                {
                    var data = Encoding.GetEncoding(inputCharset).GetBytes(inputText);
                    rsaProvider.FromXmlString(publicKey);
                    var hash = Encoding.UTF8.GetBytes(sign); //Parse.strToHexByte(sign);
                    return rsaProvider.VerifyData(data, sha1Provider, hash);
                }
            }
            catch { return false; }
        }
        public static RSACryptoServiceProvider GetRSAServiceProvider()
        {
            CspParameters CSPParam = new CspParameters();
            CSPParam.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider rsaProvider;
            if (System.Web.HttpContext.Current == null)
            { rsaProvider = new RSACryptoServiceProvider(); }
            else
            { rsaProvider = new RSACryptoServiceProvider(CSPParam); }
            return rsaProvider;
        }
        #endregion
    }

    /// <summary>
    /// 类名：RSAFromPkcs8
    /// 功能：RSA解密、签名、验签
    /// 详细：该类对Java生成的密钥进行解密和签名以及验签专用类，不需要修改
    /// 版本：3.0
    /// 日期：2011-09-01
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public class RSAFromPkcs8
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content">待签名字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns>签名后字符串</returns>
        public static string sign(string content, string privateKey, string input_charset)
        {
            byte[] Data = Encoding.GetEncoding(input_charset).GetBytes(content);
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] signData = rsa.SignData(Data, sh);
            return Convert.ToBase64String(signData);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="content">待验签字符串</param>
        /// <param name="signedString">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public static bool verify(string content, string signedString, string publicKey, string input_charset)
        {
            bool result = false;
            byte[] Data = Encoding.GetEncoding(input_charset).GetBytes(content);
            byte[] data = Convert.FromBase64String(signedString);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(Data, sh, data);
            return result;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="resData">加密字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns>明文</returns>
        public static string decryptData(string resData, string privateKey, string input_charset)
        {
            byte[] DataToDecrypt = Convert.FromBase64String(resData);
            string result = "";
            for (int j = 0; j < DataToDecrypt.Length / 128; j++)
            {
                byte[] buf = new byte[128];
                for (int i = 0; i < 128; i++)
                {

                    buf[i] = DataToDecrypt[i + 128 * j];
                }
                result += decrypt(buf, privateKey, input_charset);
            }
            return result;
        }

        #region 内部方法

        private static string decrypt(byte[] data, string privateKey, string input_charset)
        {
            string result = "";
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] source = rsa.Decrypt(data, false);
            char[] asciiChars = new char[Encoding.GetEncoding(input_charset).GetCharCount(source, 0, source.Length)];
            Encoding.GetEncoding(input_charset).GetChars(source, 0, source.Length, asciiChars, 0);
            result = new string(asciiChars);
            //result = ASCIIEncoding.ASCII.GetString(source);
            return result;
        }

        private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        {
            byte[] pkcs8privatekey;
            pkcs8privatekey = Convert.FromBase64String(pemstr);
            if (pkcs8privatekey != null)
            {
                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                return rsa;
            }
            else
                return null;
        }

        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)	//expect an Octet string 
                    return null;

                bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else
                if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)	//version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----

                //以下修正了在IIS7环境下会产生的问题
                CspParameters CSPParam = new CspParameters();
                CSPParam.Flags = CspProviderFlags.UseMachineKeyStore;

                RSACryptoServiceProvider RSA;
                if (System.Web.HttpContext.Current == null) // WinForm
                    RSA = new RSACryptoServiceProvider();
                else // WebForm - Uses Machine store for keys
                    RSA = new RSACryptoServiceProvider(CSPParam);

                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }



            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        #endregion

        #region 解析.net 生成的Pem
        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
        {
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 609)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }

            int index = 11;
            byte[] pemModulus = new byte[128];
            Array.Copy(keyData, index, pemModulus, 0, 128);

            index += 128;
            index += 2;//141
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);

            index += 3;
            index += 4;//148
            byte[] pemPrivateExponent = new byte[128];
            Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

            index += 128;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//279
            byte[] pemPrime1 = new byte[64];
            Array.Copy(keyData, index, pemPrime1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346
            byte[] pemPrime2 = new byte[64];
            Array.Copy(keyData, index, pemPrime2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//412/413
            byte[] pemExponent1 = new byte[64];
            Array.Copy(keyData, index, pemExponent1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//479/480
            byte[] pemExponent2 = new byte[64];
            Array.Copy(keyData, index, pemExponent2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//545/546
            byte[] pemCoefficient = new byte[64];
            Array.Copy(keyData, index, pemCoefficient, 0, 64);

            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            para.D = pemPrivateExponent;
            para.P = pemPrime1;
            para.Q = pemPrime2;
            para.DP = pemExponent1;
            para.DQ = pemExponent2;
            para.InverseQ = pemCoefficient;
            return para;
        }
        #endregion

    }

    public class EncryptDes3
    {
        #region CBC模式**
        /// <summary>  
        /// DES3 CBC模式加密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV</param>  
        /// <param name="data">明文的byte数组</param>  
        /// <returns>密文的byte数组</returns>  
        public static byte[] Des3EncodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            //复制于MSDN  
            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值  
                tdsp.Padding = PaddingMode.PKCS7;       //默认值  
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.  
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the   
                // MemoryStream that holds the   
                // encrypted data.  
                byte[] ret = mStream.ToArray();
                // Close the streams.  
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.  
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        /// <summary>  
        /// DES3 CBC模式解密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV</param>  
        /// <param name="data">密文的byte数组</param>  
        /// <returns>明文的byte数组</returns>  
        public static byte[] Des3DecodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed   
                // array of encrypted data.  
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.  
                byte[] fromEncrypt = new byte[data.Length];
                // Read the decrypted data out of the crypto stream  
                // and place it into the temporary buffer.  
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.  
                return fromEncrypt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion
        #region ECB模式
        /// <summary>  
        /// DES3 ECB模式加密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>  
        /// <param name="str">明文的byte数组</param>  
        /// <returns>密文的byte数组</returns>  
        public static byte[] Des3EncodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.  
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the   
                // MemoryStream that holds the   
                // encrypted data.  
                byte[] ret = mStream.ToArray();
                // Close the streams.  
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.  
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        /// <summary>  
        /// DES3 ECB模式解密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>  
        /// <param name="str">密文的byte数组</param>  
        /// <returns>明文的byte数组</returns>  
        public static byte[] Des3DecodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed   
                // array of encrypted data.  
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.  
                byte[] fromEncrypt = new byte[data.Length];
                // Read the decrypted data out of the crypto stream  
                // and place it into the temporary buffer.  
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.  
                return fromEncrypt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Convert36 的摘要说明
    /// </summary>
    public class GuidConvert36
    {
        private const string BASE_CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //转换为段字符
        private static string GetLongNo(UInt64 num, int length)
        {
            string str = "";
            while (num > 0)
            {
                int cur = (int)(num % 36);
                str = BASE_CHAR[cur] + str;
                num = num / 36;
            }
            if (str.Length > length)
            {
                str = str.Substring(str.Length - length);
            }
            else
            {
                str = str.PadLeft(length, '0');
            }

            return str;
        }

        //解析段字符
        private static UInt64 GetLongNum(string strNo)
        {
            UInt64 num = 0;
            for (int i = 0; i < strNo.Length; i++)
            {
                num += (UInt64)BASE_CHAR.IndexOf(strNo[i]) * (UInt64)Math.Pow(BASE_CHAR.Length, strNo.Length - i - 1);
            }

            return num;
        }

        /// <summary>
        /// 压缩GUID
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string GetGuidNo(Guid g)
        {
            string s = g.ToString().Replace("-", "").ToUpper();
            string s1 = s.Substring(0, 16);
            string s2 = s.Substring(16);
            UInt64 l1 = UInt64.Parse(s1, System.Globalization.NumberStyles.HexNumber);
            UInt64 l2 = UInt64.Parse(s2, System.Globalization.NumberStyles.HexNumber);
            string str1 = GetLongNo(l1, 13);
            string str2 = GetLongNo(l2, 13);

            return str1 + str2;
        }

        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool GetGuid(string str, out Guid guid)
        {
            guid = new Guid();
            if (str.Length != 26)
            {
                return false;
                //throw new Exception("字符串错误！长度必须是26位！");
            }
            string s1 = str.Substring(0, 13);
            string s2 = str.Substring(13);
            UInt64 l1 = GetLongNum(s1);
            UInt64 l2 = GetLongNum(s2);
            string str1 = l1.ToString("X");
            string str2 = l2.ToString("X");
            string strGuid = str1.PadLeft(16, '0');
            strGuid += str2.PadLeft(16, '0');
            try
            {
                guid = new Guid(strGuid);
                return true;
            }
            catch
            {
                guid = new Guid();
                return false;
            }
        }
    }
}
