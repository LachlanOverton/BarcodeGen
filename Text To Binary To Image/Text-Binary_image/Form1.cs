

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
        //Declaring variables and setting colours manually
        //Colours for start/end of image, 1's and 0's
        System.Drawing.Bitmap fimg = null;
        bool g = false;
        Color o = Color.Black;
        Color z = Color.White;
        Color se = Color.Purple;

        public Form1()
        {
            InitializeComponent();
        }

        //Generator button method
        private void btnGen_Click(object sender, EventArgs e)
        {
            //Declarations| Length of text, charactar array of text, B string to be loaded into, int array for length and width of image
            int length;
            Char[] c;
            String b = null;
            int[] lw = null;

            //Calls method to put text into char array
            c = cGen(b);
            
            //Set length as array length
            length = c.Length;

            //Calls method to create the image paramaters (x,y size)
            lw = lwGet(length);

            //Calls bitmap builder method
            Bitmap img = bmBuilder(lw[0], lw[1], c);
            //loads new bitmap into piturebox control on form
            pictureBox1.Image = img;
            //set bool for generation checking to true
            g = true;

        }

        //Loads text into binary array then a string then into a char array
        private Char[] cGen(String b)
        {            
            //array of byes
            byte[] bits = null;
            //hard coded strings of beggining and end of rich text box's binary data
            String rem1 = " ";
            String rem2 = "00000000";
            Char[] c;

            //Loads textbox into byte array
            bits = GetBytes(rtxtText.Text.ToString());
            //Calls method to turn byte array into String
            b = ToBinary(bits);

            //Removing start and end of string 
            b = b.Replace(rem2, "");
            b = b.Replace(rem1, "");

            //turning String of 0's and 1's into character array
            c = b.ToCharArray();

            //returns char array
            return c;
        }

        //Method to get length and width of image by the length of the character array
        private int[] lwGet(int length)
        {
            //Declaration fo height and loads the length into a different int to preserve original number 
            //res is for loading the length and height into one array to return a single "variable"
            int h = 0;
            int length1 = length;
            int[] res = new int[2];

            //counter for length of x during for loop
            int xi=0;
            //does this to make sure the array is long enough to be divided and stuff
            length = length + 2;
            //Divides length by itself because this is gonna be squareish
            length = length / 2;
            //ints to remember height and length whie debugging
            int hstore;
            int lngstore;

            //for loop, runs through by calculating double length then doubling the height and halving the length until height is greater or equal to length
            for(h=2; length * h >= length1-1;)
            {
                if (h < length)
                {
                    //checks to see if length is equal or eneven then adds to it if it isn't
                    if (length % 2 == 0){ } else{length = length + 1;}

                    lngstore = length;
                    //halfs length
                    length = length / 2;
                    //updates counter for x axis
                    xi++;
                    hstore = h;
                    //doubles height
                    h = h * 2;

                }
                else
                {
                    //evens out the length and height
                    int n = h - length;
                    n = n - 1;
                    h = h - n / 2;
                    length = length + n / 2;
                    break;
                }
            }
            //Loads results into int array then returns it
            res[0] = h;
            res[1] = length;
            return res;

        }

        //Method to create bitmap
        private Bitmap bmBuilder(int h, int w, Char[] c)
        {
            //Declares bitmap with the height and width from method call
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(h, w);
            int binc = 0;
            int x = 0;
            int y = 0;
            int fx = 0;
            int fy = 0;
            bool p = true;
            
            
   
            //For loop of x axis
            //++ is on left hand side so it adds it before getting the value i guess
            for ( x = -1; x <= img.Height; ++x)
            {
                //checking to see if loop has run more times that height of bitmap
                if (x > img.Height)
                {
                    break;
                }
                else
                {
                    //sets for loop for y axis inside of x axis
                    for ( y = -1; y <= img.Width; ++y)
                    {
                        //seeing if counter had been incremented more than the length of the char array
                        if (binc < c.Length)
                        {
                            //gets character using binc counter and checks if it is a 0
                            if (c[binc].ToString() == "0")
                            {
                                try
                                {
                                    //if else statement for setting the pixel colour of the bitmap
                                    //P is colour switch for start/end pixels
                                    if (p == true) { img.SetPixel(y, x, se); p = false; }
                                    else
                                    {
                                        img.SetPixel(y, x, z);
                                        binc++;
                                    }
                                }//Catches errors, has if statement checking if x counter is greater than bitmap height
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
                            //Basically does the same stuff as before but for 1's not 0's
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
                        //Setting pixels after running through the array
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
                                            //Setting pixels of start and end of bitmap
                                            if (p == false) { img.SetPixel(y, x, se); p = true; }
                                            else { img.SetPixel(y, x, Color.White); }

                                        }//if it goes outside of the bounds of the array it still outputs the bitmap
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

        
        //turns string into byte array
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        //Method to turn byte array into String
        public static String ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        //Creates a string from char array
        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        //Button for saving image
        private void button1_Click(object sender, EventArgs e)
        {
            String name = "";
            String savpath;
            name = txtName.Text;
            name = name + ".png";
            //Checks to see if an image has been generated
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
