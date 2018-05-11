using HCTECHLib;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
	/// <summary>
	/// 加密解密
	/// </summary>
	public class EncryTool
	{
	    
		public EncryTool()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

	    protected static string EncryptStr = "ZYXWVUTSRQPONMLKJIHGFEDCBA_zyxwvutsrqponmlkjihgfedcba*9876543210!@#$%^&";
		static string CONST_ENCRYKEY = "LouDaoKejifayangguangdasukangsheng";
	    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
	    ///   <summary> 
	    ///   DES解密字符串 
	    ///   </summary> 
	    ///   <param   name= "decryptString "> 待解密的字符串 </param> 
	    ///   <param   name= "decryptKey "> 解密密钥,要求为8位,和加密密钥相同 </param> 
	    ///   <returns> 解密成功返回解密后的字符串，失败返源串 </returns> 
	    public static string DecryptDES(string decryptString, string decryptKey)
	    {
            //CryptClass cry = new CryptClass();
            //return cry.DecryptMessageB(decryptString, decryptKey);

            try
            {
                decryptString = decryptString.Replace(' ', '+');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception e)
            {
                return decryptString;
            }
        }
        public static string DeCryptData64(string str, string key)
        {
            long num = 0L;
            string text = string.Empty;
            string text2 = string.Empty;
            string text3 = string.Empty;
            for (int i = 1; i <= key.Length; i++)
            {
                num += (long)Convert.ToInt32(char.Parse(key.Substring(i - 1, 1)));
            }
            text2 = str;
            while (text2.Length > 4)
            {
                text3 += text2.Substring(0, 4);
                text2 = text2.Substring(4);
                if (text2.Length > 0)
                {
                    text2 = text2.Substring(1);
                }
            }
            text3 += text2;
            for (int j = 1; j <= text3.Length; j++)
            {
                int num2 = (int)Convert.ToInt16((long)(char.Parse(text3.Substring(j - 1, 1)) - '\u0005') + num % 7L - (long)(j % 3));
                if (num2 >= 97 && num2 <= 122)
                {
                    num2 -= 32;
                }
                else
                {
                    if (num2 >= 65 && num2 <= 90)
                    {
                        num2 += 32;
                    }
                }
                text += Convert.ToChar(num2);
            }
            return text;
        }

	    public static string StrongEncrypt(string salt, string str)
	    {
	        return EncryptMD5(salt + EncryptStr + str);
	    }
	    public static string EncryptMD5(string source)
	    {
	        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
	        byte[] str1 = Encoding.UTF8.GetBytes(source);
	        byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
	        md5.Clear();
	        (md5 as IDisposable).Dispose();
	        return Convert.ToBase64String(str2);
	    }

	}
}
