using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace forum.Domain.Helper
{
    public class MD5Helper
    {
        public static string GetMD5(string encypStr)
        {
            if (string.IsNullOrEmpty(encypStr))
            {
                throw new ArgumentException("encypStr或者charset参数为空");
            }
            string result;
            byte[] inputStr;
            byte[] ouputStr;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            
            inputStr = Encoding.UTF8.GetBytes(encypStr);

            ouputStr =md5.ComputeHash(inputStr);
            //释放资源
            md5.Clear();

            result = System.BitConverter.ToString(ouputStr);
            //替换掉-并将字符串大写
            result=result.Replace("-", "").ToLower();

            return result;
        }
    }
}
