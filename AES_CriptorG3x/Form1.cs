using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AES_CriptorG3x
{
    public partial class Form1 : Form
    {
        public static long is_n = 0, is_d = 0;
        public static string RSAnot = "p или q - не простые числа!", RSAin = "Введите p и q!", RSASecr = "Введите секретный ключ!",
             DSAnot = "p или q - не простые числа!", DSAY = "Файл подлинный. Подпись верна.", DSAN = "Внимание! Файл НЕ подлинный!!!";
        string MD5Y = "Хеши равны", MD5N = "Хеши разные", RSAD = "параметры q и p не должны совпадать", RSAP = "поля p и q не должны быть пустыми";
        public Form1()
        {
            InitializeComponent();
        }
        //GLOBAL CRIPT
        private void GLcript_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrigT.Text != "")
                {
                    if (AESB.Checked)
                    {
                        if (textBoxPass.Text == null || textBoxPass.Text == "")
                        {
                            textBoxPass.Text = "ewfg243rt3t23t56";
                        }
                        OrigAES.Text = OrigT.Text.Trim();
                        CriptAES.Text = null;
                        CriptAES.Text = AES.AESCript(OrigT.Text.Trim(), textBoxPass.Text);
                    }
                    if (RSAB.Checked)
                    {
                        List<String> res;
                        res = RSA.Cript(OrigT.Text.Trim(), textBox_d.Text, textBox_n.Text, textBox_p.Text, textBox_q.Text);
                        textBox_n.Text = is_n.ToString();
                        textBox_d.Text = is_d.ToString();
                        OrigRSA.Text = OrigT.Text.Trim();
                        CriptRSA.Text = null;
                        foreach (string str in res)
                        {
                            CriptRSA.Text += str + Environment.NewLine;
                        }
                    }
                    if (MD5B.Checked)
                    {
                        Heshtext.Text = Md5.Cript(OrigT.Text);
                        MD5text.Text = OrigT.Text;
                    }
                    MessageBox.Show("Текст зашифрован, результат в выбранных методах", "Криптор");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        //AES
        private void buttonCript_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrigAES.Text != "")
                {
                    CriptAES.Text = null;
                    CriptAES.Text = AES.AESCript(OrigAES.Text, textBoxPass.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please check your data..");
            }

        }
        private void buttonDecript_Click(object sender, EventArgs e)
        {
            try
            {
                if (CriptAES.Text != "")
                {
                    OrigAES.Text = null;
                    OrigAES.Text = AES.AESDecript(CriptAES.Text, textBoxPass.Text);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Please check your data..");
            }
        }
        //RSA
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            {
                if (textBoxp.Text.Trim() != "" || textBoxq.Text.Trim() != "")
                {
                    if (Convert.ToInt32(textBox_p.Text) != Convert.ToInt32(textBox_q.Text))
                    {
                        if (OrigRSA.Text != "")
                        {
                            List<String> res = new List<string>(RSA.Cript(OrigRSA.Text, textBox_d.Text, textBox_n.Text, textBox_p.Text, textBox_q.Text));
                            textBox_n.Text = is_n.ToString();
                            textBox_d.Text = is_d.ToString();
                            CriptRSA.Text = null;
                            foreach (string str in res)
                            {
                                CriptRSA.Text += str + Environment.NewLine;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(RSAD);
                    }
                }
                else
                {
                    MessageBox.Show(RSAP);
                }
            }
        }
        private void buttonDecipher_Click(object sender, EventArgs e)
        {
            if (CriptRSA.Text != "")
            {
                List<String> list = new List<string> { };
                foreach (string str in CriptRSA.Lines)
                {
                    if (str != "" && str != " ")
                        list.Add(str.Trim());
                }
                OrigRSA.Text = RSA.Uncript(list, textBox_d.Text, textBox_n.Text);

            }
        }
        //MD5
        private void hesh_Click(object sender, EventArgs e)
        {
            if (MD5text.Text != "")
                Heshtext.Text = Md5.Cript(MD5text.Text);
        }
        private void SRmd5_Click(object sender, EventArgs e)
        {
            if (hesh1.Text.Trim() != "" || hesh2.Text.Trim() != "")
            {
                if (Md5.compare(hesh1.Text.Trim(), hesh2.Text.Trim()))
                {
                    MessageBox.Show(MD5Y);
                }
                else
                {
                    MessageBox.Show(MD5N);
                }
            }
        }
        //DSA
        private void signButton_Click(object sender, EventArgs e)
        {
            if (SecurityPatch.Text.Length > 0 && FilePatch.Text.Length > 0 && textBoxp.Text.Length > 0 && textBoxq.Text.Length > 0)
            {
                List<string> result = new List<string>(DSA.Encrypt(SecurityPatch.Text, FilePatch.Text, Convert.ToInt64(textBoxp.Text), Convert.ToInt64(textBoxq.Text)));
                foreach (string str in result)
                {
                    podpic.Text += str + "\r\n";
                }
                textBoxn.Text = is_n.ToString();
                textBoxd.Text = is_d.ToString();

            }
            else
            {
                MessageBox.Show("недостаток данных");
            }

        }
        //Save and Open files
        private void checkSignButton_Click(object sender, EventArgs e)
        {
            if (SecurityPatch.Text.Length > 0 && FilePatch.Text.Length > 0 && textBoxp.Text.Length > 0 && textBoxq.Text.Length > 0)
            {
                DSA.Decipher(SecurityPatch.Text, FilePatch.Text, Convert.ToInt64(textBoxp.Text), Convert.ToInt64(textBoxq.Text));
                textBoxn.Text = is_n.ToString();
                textBoxd.Text = is_d.ToString();
            }
            else
            {
                MessageBox.Show("недостаток данных");
            }
        }
        private void sourceFileButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FilePatch.Text = ofd.FileName;
            }
        }
        private void signFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SecurityPatch.Text = ofd.FileName;
            }
        }
        //butons
        private void AESB_CheckedChanged(object sender, EventArgs e)
        {
            but();
        }
        private void RSAB_CheckedChanged(object sender, EventArgs e)
        {
            but();
        }
        private void MD5B_CheckedChanged(object sender, EventArgs e)
        {
            but();
        }
        private void OrigT_TextChanged(object sender, EventArgs e)
        {
            if (OrigT.Text.Length > 0)
            {
                if (!MD5B.Checked && !AESB.Checked && !RSAB.Checked)
                {
                    MessageBox.Show("Выберите метод");
                }
                else
                {
                    if (MD5B.Checked || AESB.Checked || RSAB.Checked)
                    {
                        GLcript.Enabled = true;
                    }
                }
            }
            else
            {
                GLcript.Enabled = false;
            }
        }
        private void but()
        {
            if ((MD5B.Checked || AESB.Checked || RSAB.Checked) && OrigT.Text != "")
            {
                GLcript.Enabled = true;
            }
            else
            {
                GLcript.Enabled = false;
            }
        }
        //------------------------------------------------------------
        //-----------------------------OPTIONS------------------------
        //------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            string patch = Directory.GetCurrentDirectory() + "\\Language";
            Dir(patch);
        }
        void Dir(string pyt)
        {
            if (new DirectoryInfo(pyt).Exists)
            {
                comboBox1.Items.Clear();
                DirectoryInfo INFO;
                string[] list = Directory.GetFiles(pyt, "*.lng");
                foreach (string s in list)
                {
                    INFO = new DirectoryInfo(s);
                    comboBox1.Items.Add(INFO.Name);
                }
                comboBox1.SelectedItem = comboBox1.Items[0].ToString();
            }
            else
            {
                MessageBox.Show("Language files not fount");
                comboBox1.SelectedItem = comboBox1.Items[0].ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Language" + "\\" + comboBox1.SelectedItem.ToString()))
            {
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Language" + "\\" + comboBox1.SelectedItem.ToString().Trim(), System.Text.Encoding.Default))
                {
                    //label
                    label01.Text = sr.ReadLine();
                    label02.Text = sr.ReadLine();
                    label03.Text = sr.ReadLine();
                    label04.Text = sr.ReadLine();
                    label05.Text = sr.ReadLine();
                    label06.Text = sr.ReadLine();
                    label07.Text = sr.ReadLine();
                    label08.Text = sr.ReadLine();
                    label09.Text = sr.ReadLine();
                    label010.Text = sr.ReadLine();
                    label011.Text = sr.ReadLine();
                    label012.Text = sr.ReadLine();
                    label013.Text = sr.ReadLine();
                    label014.Text = sr.ReadLine();
                    label015.Text = sr.ReadLine();
                    label016.Text = sr.ReadLine();
                    label017.Text = sr.ReadLine();
                    //contorls
                    tabControl1.TabPages[0].Text = sr.ReadLine();
                    tabControl1.TabPages[1].Text = sr.ReadLine();
                    tabControl1.TabPages[2].Text = sr.ReadLine();
                    tabControl1.TabPages[3].Text = sr.ReadLine();
                    tabControl1.TabPages[4].Text = sr.ReadLine();
                    tabControl1.TabPages[5].Text = sr.ReadLine();
                    //buttons
                    GLcript.Text = sr.ReadLine();
                    buttonCript.Text = sr.ReadLine();
                    buttonDecript.Text = sr.ReadLine();
                    buttonEncrypt.Text = sr.ReadLine();
                    buttonDecipher.Text = sr.ReadLine();
                    sourceFileButton.Text = sr.ReadLine();
                    signButton.Text = sr.ReadLine();
                    signFileButton.Text = sr.ReadLine();
                    checkSignButton.Text = sr.ReadLine();
                    hesh.Text = sr.ReadLine();
                    SRmd5.Text = sr.ReadLine();
                    button1.Text = sr.ReadLine();
                    //message
                    RSAin = sr.ReadLine();
                    RSAnot = sr.ReadLine();
                    RSASecr = sr.ReadLine();
                    RSAD = sr.ReadLine();
                    RSAP = sr.ReadLine();
                    DSAnot = sr.ReadLine();
                    DSAY = sr.ReadLine();
                    DSAN = sr.ReadLine();
                    MD5Y = sr.ReadLine();
                    MD5N = sr.ReadLine();
                }

            }
            else
            {
                MessageBox.Show("Not found file");
            }
        }
    }
}