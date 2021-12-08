using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using System.Numerics;

namespace AES_CriptorG3x
{
    class DSA
    {
       static char[] characters = new char[] { '#', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-' };
        //зашифровать
        public static List<string> Encrypt(string SecurityFile, string FilePath, long p, long q)
        {
            List<string> result = new List<string>();
                if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                {
                    string hash = File.ReadAllText(FilePath).GetHashCode().ToString();

                    long n = p * q;
                    long m = (p - 1) * (q - 1);
                    long d = Calculate_d(m);
                    long e_ = Calculate_e(d, m);

                    result = RSA_Endoce(hash, e_, n);

                    StreamWriter sw = new StreamWriter(SecurityFile);
                    foreach (string item in result)
                        sw.WriteLine(item);
                    sw.Close();

                    Form1.is_d = d;
                    Form1.is_n = n;

                    Process.Start(SecurityFile);
                }
                else
                    MessageBox.Show(Form1.DSAnot);
            return result;
        }

        //расшифровать
        public static void Decipher(string SecurityFile, string FilePath, long d, long n)
        {

                List<string> input = new List<string>();

                StreamReader sr = new StreamReader(SecurityFile);

                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }

                sr.Close();

                string result = RSA_Dedoce(input, d, n);

                string hash = File.ReadAllText(FilePath).GetHashCode().ToString();

                if (result.Equals(hash))
                    MessageBox.Show(Form1.DSAY);
                else
                    MessageBox.Show(Form1.DSAN);
        }
        //зашифровать
        private static List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }
        //расшифровать
        private static string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }
        //вычисление параметра d. d должно быть взаимно простым с m
        private static long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }
        //вычисление параметра e
        private static long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
        //проверка: простое ли число?
        private static bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
    }
}
