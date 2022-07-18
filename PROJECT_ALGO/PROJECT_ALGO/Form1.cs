using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace PROJECT_ALGO
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd;
        public Form1()
        {
            InitializeComponent();
        }
        public static byte ctd(string bin)
        {
            byte val = 1;
            byte sum = 0;
            for (int i = bin.Length - 1; i >= 0; i--)
            {
                if (bin[i] == '1')
                    sum += val;
                val *= 2;
            }

            return sum;
        }
        public static string rem(string st1)
        {
            string ns = "";
            for (int i = 1; i < st1.Length; i++)
                ns += st1[i];
            return ns;
        }
        public static string text;
        public static int tappos;
        public static string encrypt(string cl)
        {
            char res = '0';
            int tap = cl.Length - tappos - 1;
            // instead of 5 we must write (8-tappos-1).
            for (int i = 0; i < 8; i++)
            {
                if (cl[tap] != cl[0])
                    res = '1';

                cl = rem(cl);
                cl += res;
                res = '0';
            }
            string nc = cl;
            return nc;
        }
        public static byte encryptcolor(string text, string cl)
        {
            char res = '0';
            byte final;
            string newst = "";
            int j = text.Length - 8;
            for (int i = 0; i < 8; i++)
            {
                if (cl[i] != text[j])
                    res = '1';
                newst += res;
                res = '0';
                j++;
            }
            final = ctd(newst);
            return final;
        }     
        private void button1_Click(object sender, EventArgs e)
        {
            tappos = Convert.ToInt16(comboBox2.Text);
            if (comboBox1.Text.Length < 8)
                text = comboBox1.Text.PadLeft(8, '0');
            else
                text = comboBox1.Text;
            if (tappos > comboBox1.Text.Length)
            {
                MessageBox.Show("Error tap pos!");
            }
            else
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                int width = bmp.Width;
                int height = bmp.Height;
                Color p;
                string n1;
                string n2;
                string n3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        p = bmp.GetPixel(x, y);
                        byte a = p.A;
                        byte b = p.B; string bst = Convert.ToString(b, 2).PadLeft(8, '0');
                        byte g = p.G; string gst = Convert.ToString(g, 2).PadLeft(8, '0');
                        byte r = p.R; string rst = Convert.ToString(r, 2).PadLeft(8, '0');
                        n1 = encrypt(text);
                        byte nr = encryptcolor(n1, rst);
                        n2 = encrypt(n1);
                        byte ng = encryptcolor(n2, gst);
                        n3 = encrypt(n2);
                        byte nb = encryptcolor(n3, bst);
                        text = n3;
                        bmp.SetPixel(x, y, Color.FromArgb(a, nr, ng, nb));
                    }
                }
                pictureBox1.Image = bmp;
                if (button1.Text == "Encrypt")
                    button1.Text = "Decrypt";
                else
                    button1.Text = "Encrypt";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (ofd = new OpenFileDialog() { Filter = "All|*.*", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        pictureBox1.Image = Image.FromFile(ofd.FileName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
