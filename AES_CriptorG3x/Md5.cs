using System;
using System.Text;
using System.Security.Cryptography;


namespace AES_CriptorG3x
{
    class Md5
    {
        public static string Cript(string OrigTxt)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(OrigTxt));


                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            return sBuilder.ToString();
        }
        public static bool compare(string hesh1,string hesh2)
        {
            if (hesh1.Equals(hesh2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
