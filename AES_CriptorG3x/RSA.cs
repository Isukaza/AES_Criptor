using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
namespace AES_CriptorG3x
{
    class RSA
    {
        static char[] characters = new char[] { '#', '+', '=', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                        'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                        'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                        'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                        '8', '9', '0' ,',','?','!',':','.',';','<','>','@','"',
                                                        '$','%','^','*','(',')','-','_','А','Б','*','/','\\','{',
                                                        '}','[',']','B','A','D','C','G','f','F','I','H','P','Q','N',
                                                        'M','L','J','K','O','T','Z','U','V','X','Y','W','E','R','S','.'};
        //зашифровать
        public static List<string> Cript(string Otext,string dtext,string ntext,string ptext, string qtext)
        {
            List<string> result = new List<string>();
            try {
                if ((ptext.Length > 0) && (qtext.Length > 0))
                {
                    long p = Convert.ToInt64(ptext);
                    long q = Convert.ToInt64(qtext);

                    if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                    {
                        Otext = Otext.ToUpper();

                        long n = p * q;
                        long m = (p - 1) * (q - 1);
                        long d = Calculate_d(m);
                        long e_ = Calculate_e(d, m);

                        result = RSA_Endoce(Otext, e_, n);

                        StreamWriter sw = new StreamWriter("Шифротекст RSA.txt");
                        foreach (string item in result)
                            sw.WriteLine(item);
                        sw.Close();

                        Form1.is_d = d;
                        Form1.is_n = n;
                        return result;
                    }
                    else
                        MessageBox.Show(Form1.RSAnot);
                }
                else
                    MessageBox.Show(Form1.RSAin);
            }
            catch (Exception)
            {

            }
            return result;
        }

        //расшифровать
        public static string Uncript(List<string> list,string dtext,string ntext)
        {
            if ((dtext.Length > 0) && (ntext.Length > 0))
            {
                long d = Convert.ToInt64(dtext);
                long n = Convert.ToInt64(ntext);
                string result = RSA_Dedoce(list, d, n);
                return result;
            }
            else
                MessageBox.Show(Form1.RSASecr);
            return "0";
        }
        //зашифровать
        static List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();
            try
            {
                BigInteger bi;

                for (int i = 0; i < s.Length; i++)
                {
                    int index = Array.IndexOf(characters, s[i]);

                    bi = new BigInteger(index);
                    bi = BigInteger.Pow(bi, (int)e);

                    BigInteger n_ = new BigInteger((int)n);

                    bi = bi % n_;
                     if (index != -1)
                     {
                         result.Add(bi.ToString());
                     }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            return result;
        }
        //расшифровать
        static string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";
            try
            {
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
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            return result;
        }
        static bool IsTheNumberSimple(long n)
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
        //вычисление параметра d. d должно быть взаимно простым с m
        //проверка: простое ли число?
        static long Calculate_d(long m)
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
        static long Calculate_e(long d, long m)
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

    }
}
