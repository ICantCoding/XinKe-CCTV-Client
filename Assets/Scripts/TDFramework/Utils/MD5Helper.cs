

namespace TDFramework
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;

    public class Md5Helper
    {

        //对字符串进行MD5计算
        public static string Md5String(string str)
        {
            if (null == str) return null;
            byte[] result = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder output = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString();
        }
        //对文件进行MD5值计算
        public static string Md5File(string filePath)
        {
            if (null == filePath) return null;
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }
    }
}
