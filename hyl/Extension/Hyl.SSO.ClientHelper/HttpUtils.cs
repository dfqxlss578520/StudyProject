using System;
using System.IO;
using System.Net;
using System.Text;

namespace Hyl.SSO.ClientHelper
{
    /// <summary>
    /// HTTP请求工具类
    /// </summary>
    public class HttpUtils
    {
        #region 请求Url

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url)
        {
            return RequestUrl(url, "POST");
        }
        #endregion

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url, string method)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = method;
                request.ContentType = "text/html";
                request.Headers.Add("charset", "utf-8");

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                return content;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region 请求Url，发送数据
        /// <summary>
        /// 请求Url，发送数据
        /// </summary>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                Stream outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                return content;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #endregion

        #region Http下载文件
        ///// <summary>
        ///// Http下载文件
        ///// </summary>
        //public static string HttpDownloadFile(string url, string path)
        //{
        //    // 设置参数
        //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

        //    //发送请求并获取相应回应数据
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    //直到request.GetResponse()程序才开始向目标网页发送Post请求
        //    Stream responseStream = response.GetResponseStream();

        //    //创建本地文件写入流
        //    Stream stream = new FileStream(path, FileMode.Create);

        //    byte[] bArr = new byte[1024];
        //    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
        //    while (size > 0)
        //    {
        //        stream.Write(bArr, 0, size);
        //        size = responseStream.Read(bArr, 0, (int)bArr.Length);
        //    }
        //    stream.Close();
        //    responseStream.Close();
        //    return path;
        //}
        #endregion

        #region Http上传文件
        ///// <summary>
        ///// Http上传文件
        ///// </summary>
        //public static string HttpUploadFile(string url, string path)
        //{
        //    // 设置参数
        //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        //    CookieContainer cookieContainer = new CookieContainer();
        //    request.CookieContainer = cookieContainer;
        //    request.AllowAutoRedirect = true;
        //    request.Method = "POST";
        //    string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
        //    request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
        //    byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("--" + boundary + "");

        //    byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("--" + boundary + "--");


        //    int pos = path.LastIndexOf("");
        //    string fileName = path.Substring(pos + 1);

        //    //请求头部信息 
        //    StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name='file';filename='{0}' Content - Type:application / octet - stream", fileName));
        //    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

        //    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        //    byte[] bArr = new byte[fs.Length];
        //    fs.Read(bArr, 0, bArr.Length);
        //    fs.Close();

        //    Stream postStream = request.GetRequestStream();
        //    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        //    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
        //    postStream.Write(bArr, 0, bArr.Length);
        //    postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
        //    postStream.Close();

        //    //发送请求并获取相应回应数据
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    //直到request.GetResponse()程序才开始向目标网页发送Post请求
        //    Stream instream = response.GetResponseStream();
        //    StreamReader sr = new StreamReader(instream, Encoding.UTF8);
        //    //返回结果网页（html）代码
        //    string content = sr.ReadToEnd();
        //    return content;
        //}
        #endregion

    }
}
