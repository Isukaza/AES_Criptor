using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace AES_CriptorG3x
{
    class AES
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("1234swt3xgwastgh"); //min 8 karakter

        // Cript
        public static string AESCript(string hamMetin, string sifre)
        {
            try {
                Random rand = new Random();
                if (string.IsNullOrEmpty(hamMetin))
                    throw new ArgumentNullException("hamMetin");
                if (string.IsNullOrEmpty(sifre))
                    throw new ArgumentNullException("sifre");

                string sifreliMetin = null;
                RijndaelManaged aes = null;
                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sifre, _salt);
                    aes = new RijndaelManaged();
                    aes.Key = key.GetBytes(aes.KeySize / 8);

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(BitConverter.GetBytes(aes.IV.Length), 0, sizeof(int));
                        msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(hamMetin);
                            }
                        }
                        sifreliMetin = Convert.ToBase64String(msEncrypt.ToArray());
                        //запись в файл
                        FileStream file = new FileStream("Шифротекст AES.txt", FileMode.Create); //создаем файловый поток
                        StreamWriter writer = new StreamWriter(file);
                        writer.WriteLine(sifreliMetin);
                        writer.Close();
                       
                    }
                }
                finally
                {
                    if (aes != null)
                        aes.Clear();
                }
                return sifreliMetin;
            }catch(Exception Error)
            {
                MessageBox.Show(Error.Message);
                return "0";
            }
        }
        
        //Decript
        public static string AESDecript(string sifreliMetin, string sifre)
        {
            if (string.IsNullOrEmpty(sifreliMetin))
                throw new ArgumentNullException("sifreliMetin");
            if (string.IsNullOrEmpty(sifre))
                throw new ArgumentNullException("sifre");

            RijndaelManaged aes = null;
            string cozulmusMetin = null;

             try
            {
               
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sifre, _salt);
                               
                byte[] bytes = Convert.FromBase64String(sifreliMetin);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                  
                    aes = new RijndaelManaged();
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = ReadByteArray(msDecrypt);

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                                 
                            cozulmusMetin = srDecrypt.ReadToEnd();
                    }
                }
            }
             finally
             {
                 if (aes != null)
                     aes.Clear();
             }
             return cozulmusMetin;
        }

        private static byte[] ReadByteArray(MemoryStream ms)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (ms.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (ms.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }
}
