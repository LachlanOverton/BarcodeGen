

/*

 Crappy pointless program I made when I was bored.
 * 
 * -Lachlan Overton
 
*/

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

namespace TextBinaryImage
{
    public partial class Form1 : Form
    {
        System.Drawing.Bitmap fimg = null;
        bool g = false;
        Color o = Color.Black;
        Color z = Color.White;
        Color se = Color.Purple;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            int length;

            Char[] c;
            String b = null;
            int[] lw = null;

            c = cGen(b);

            length = c.Length;

            lw = lwGet(length);

            Bitmap img = bmBuilder(lw[0], lw[1], c);
            pictureBox1.Image = img;
            g = true;

        }

        private Char[] cGen(String b)
        {            
            byte[] bits = null;
            String rem1 = " ";
            String rem2 = "00000000";
            Char[] c;

            bits = GetBytes(rtxtText.Text.ToString());
            b = ToBinary(bits);

            b = b.Replace(rem2, "");
            b = b.Replace(rem1, "");

            c = b.ToCharArray();

            return c;
        }

        private int[] lwGet(int length)
        {
            int h = 0;
            int length1 = length;
            int[] res = new int[2];

            int xi=0;
            length = length + 2;
            length = length / 2;
            int hstore;
            int lngstore;
            for(h=2; length * h >= length1-1;)
            {
                if (h < length)
                {
                    if (length % 2 == 0){ } else{length = length + 1;}

                    lngstore = length;
                    length = length / 2;
                    xi++;
                    hstore = h;
                    h = h * 2;

                }
                else
                {
                    int n = h - length;
                    n = n - 1;
                    h = h - n / 2;
                    length = length + n / 2;
                    break;
                }
            }

            res[0] = h;
            res[1] = length;
            return res;

        }

        private Bitmap bmBuilder(int h, int w, Char[] c)
        {

            System.Drawing.Bitmap img = new System.Drawing.Bitmap(h, w);
            int binc = 0;
            int x = 0;
            int y = 0;
            int fx = 0;
            int fy = 0;
            bool p = true;
            
            
   
            

            for ( x = -1; x <= img.Height; ++x)
            {
                if (x > img.Height)
                {
                    break;
                }
                else
                {
                    for ( y = -1; y <= img.Width; ++y)
                    {

                        if (binc < c.Length)
                        {
                            if (c[binc].ToString() == "0")
                            {
                                try
                                {
                                    if (p == true) { img.SetPixel(y, x, se); p = false; }
                                    else
                                    {
                                        img.SetPixel(y, x, z);
                                        binc++;
                                    }
                                }
                                catch (ArgumentOutOfRangeException q)
                                {
                                    Console.WriteLine(q.Message);
                                    y = -1;
                                    if (x > img.Height)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        x++;
                                    }
                                }

                            }
                            else if (c[binc].ToString() == "1")
                            {
                                try
                                {
                                    img.SetPixel(y, x, o);
                                    binc++;
                                }
                                catch (ArgumentOutOfRangeException q)
                                {
                                    Console.WriteLine(q.Message);
                                    y = -1;
                                    if (x > img.Height)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        x++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (x < img.Height)
                            {
                                for (x = x; x <= img.Height; ++x)
                                {
                                    for (y = y; y <= img.Width; ++y)
                                    {
                                        try
                                        {
                                            if (p == false) { img.SetPixel(y, x, se); p = true; }
                                            else { img.SetPixel(y, x, Color.White); }

                                        }
                                        catch (ArgumentOutOfRangeException q)
                                        {
                                            Console.WriteLine(q.Message);
                                            fx = x;
                                            fy = y;
                                            fimg = new System.Drawing.Bitmap(fy, fx);
                                            fimg = img;
                                            return fimg;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                return fimg;
                            }
                        }
                    }
                }
            }
            return fimg;
        }

        

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            String name = "";
            String savpath;
            name = txtName.Text;
            name = name + ".png";

            if (g == true)
            {
                folderBrowserDialog1.ShowDialog();

                if (folderBrowserDialog1.SelectedPath != "")
                {
                    savpath = folderBrowserDialog1.SelectedPath;
                    String esc = null;
                    esc = savpath.Substring(Math.Max(0, savpath.Length - 1));
                    if (esc == "\\")
                    {
                        savpath = savpath + name;
                        fimg.Save(savpath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                    {
                        savpath = savpath + "\\" + name;
                        fimg.Save(savpath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                else
                {
                    MessageBox.Show("No filepath selected", ":(", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("No image generated", ":(", MessageBoxButtons.OK);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            o = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            z = colorDialog1.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            se = colorDialog1.Color;
        }


    }
}
