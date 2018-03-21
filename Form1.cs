using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace obr_iso
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double w1, w2, w3;

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap("Z:\\обр изо\\1.bmp");
            pictureBox1.Image = bmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp2 = (Bitmap)pictureBox1.Image.Clone();
            int x, y, r, g, f, b; Color c;
            for (x = 0; x < bmp2.Height; ++x)
            {
                for (y = 0; y < bmp2.Width; ++y)
                {
                    c = bmp2.GetPixel(y, x);
                    r = c.R;
                    g = c.G;
                    b = c.B;

                    f = (r + g + b) / 3;

                    //c = Color.FromArgb(255-r,255-g,255-b); //инверсия
                    //c = Color.FromArgb(f, f, f); // чб
                    c = Color.FromArgb(r, g, b);
                    bmp2.SetPixel(y, x, c);
                }
            }
            pictureBox2.Image = bmp2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Bitmap bmp1 = (Bitmap)pictureBox1.Image.Clone();
            //Bitmap bmp2 = new Bitmap(bmp1.Width,bmp1.Height);
            Bitmap bmp2 = new Bitmap(100, 100);
            int x, y, f; Color c;
            double u = 0.1, v = 0.2, F = 100;
            for (y = 0; y < bmp2.Height; ++y)
            {
                for (x = 0; x < bmp2.Width; ++x)
                {
                    f = 128 + Convert.ToInt32(F * Math.Cos(u * x + v * y));
                    c = Color.FromArgb(f, f, f);
                    bmp2.SetPixel(x, y, c);
                }
            }
            pictureBox2.Image = bmp2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Bitmap bmp2 = (Bitmap)pictureBox1.Image.Clone();
            int x, y, r, g, f, b; Color c;
            double D = 50, d;
            for (x = 0; x < bmp2.Height; ++x)
            {
                for (y = 0; y < bmp2.Width; ++y)
                {
                    c = bmp2.GetPixel(y, x);
                    r = c.R;
                    g = c.G;
                    b = c.B;

                    f = (r + g + b) / 3;

                    //c = Color.FromArgb(255-r,255-g,255-b); //инверсия
                    //c = Color.FromArgb(f, f, f); // чб
                    d = GaussRandom(rnd);
                    //r += Convert.ToInt32(D*(rnd.NextDouble()-0.5));
                    //g += Convert.ToInt32(D * (rnd.NextDouble() - 0.5));
                    //b += Convert.ToInt32(D * (rnd.NextDouble() - 0.5));
                    r += Convert.ToInt32(D * (d));
                    g += Convert.ToInt32(D * (d));
                    b += Convert.ToInt32(D * (d));
                    if (r < 0)
                        r = 0;
                    else if (r > 255)
                        r = 255;
                    if (g < 0)
                        g = 0;
                    else if (g > 255)
                        g = 255;
                    if (b < 0)
                        b = 0;
                    else if (b > 255)
                        b = 255;
                    c = Color.FromArgb(r, g, b);
                    bmp2.SetPixel(y, x, c);
                }
            }
            pictureBox1.Image = bmp2;
        }
        double GaussRandom(Random rnd)
        {
            double sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += rnd.NextDouble();
            }
            return sum - 6;
        }
        int GaussRandomInt(Random rnd, int s, int f)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += rnd.Next(s, f);
            }
            return sum - f / 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Bitmap bmp2 = (Bitmap)pictureBox1.Image.Clone();
            int x, y, r, g, f, b; Color c;
            double D = 50, d;
            int МосьСила = 8000;
            int rndX, rndY, rndColor;
            for (int n = 0; n < МосьСила; n++)
            {
                rndX = rnd.Next(0, bmp2.Width);
                rndY = rnd.Next(0, bmp2.Height);
                rndColor = rnd.Next(0, 2);
                if (rndColor == 1)
                    bmp2.SetPixel(rndX, rndY, Color.White);
                else
                    bmp2.SetPixel(rndX, rndY, Color.Black);
            }
            pictureBox1.Image = bmp2;
        }

        double[,] random() //свертка
        {
            //int sum = 1;
            double[,] res = new double[5, 5];
            int lastN = 0;
            int startN = 101;
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    startN -= lastN;
                    lastN = rnd.Next(0, startN);
                    res[i, j] = ((double)lastN) / 100;
                }
            return res;
        }
        //сделать функцию для w и рекрсивный филтр!!
        private void button6_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            double[,] matrix = random();
            //{
            //                  { 0.01, 0.02, 0.03, 0.04, 0.05 },
            //                  {-0.75,0,0.2,1,-1 },
            //                  { 0,0,-0.2,1,-1},
            //                  { 0,-0.5,0.2,1,-1},
            //                  { 0,0,-0.2,1,-1}};
            Bitmap bmp2 = (Bitmap)pictureBox1.Image.Clone();
            int x, y;
            double r = 0, g = 0, b = 0;
            Color c;
            
            for (y = 2; y < bmp2.Height - 2; y++)
            {
                for (x = 2; x < bmp2.Width - 2; x++)
                {
                    r = 0; g = 0; b = 0;
                    for (int i = -2; i < 3; i++)
                        for (int j = -2; j < 3; j++)
                        {
                            c = bmp2.GetPixel(x + i, y + j);
                            //r += c.R / 25; g += c.G / 25; b += c.B / 25;
                            r += c.R * matrix[i + 2, j + 2]; g += c.G * matrix[i + 2, j + 2]; b += c.B * matrix[i + 2, j + 2];
                        }
                    if (r < 0)
                        r = 0;
                    else if (r > 255)
                        r = 255;
                    if (g < 0)
                        g = 0;
                    else if (g > 255)
                        g = 255;
                    if (b < 0)
                        b = 0;
                    else if (b > 255)
                        b = 255;
                    c = Color.FromArgb(Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
                    bmp2.SetPixel(x, y, c);
                }

            }
            pictureBox2.Image = bmp2;
        }

        struct Pixel : IEquatable<Pixel>
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;

            public bool Equals(Pixel other)
            {
                return Red == other.Red && Green == other.Green && Blue == other.Blue && Alpha == other.Alpha;
            }
        }
        private Bitmap UnsafeCode(Bitmap bmp)
        {
            Bitmap one = bmp;

            unsafe
            {
                var oneBits = one.LockBits(new Rectangle(0, 0, one.Width, one.Height), ImageLockMode.ReadOnly, one.PixelFormat);

                int padding = oneBits.Stride - (one.Width * sizeof(Pixel));

                int width = one.Width;
                int height = one.Height;

                byte* ptr = (byte*)oneBits.Scan0;


                var pStart = (byte*)oneBits.Scan0;
                var pEnd = (byte*)(pStart + oneBits.Stride * oneBits.Height);

                while (pStart != pEnd)
                {
                    *pStart = 0; // R
                    *(pStart + 1) = 0; // G
                    *(pStart + 2) = 255; // B

                    pStart += 3;
                }
                one.UnlockBits(oneBits);
            }
            return bmp;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "1 фильтр": { break; }
                case "2 фильтр": { break; }
                case "3 фильтр": { break; }
                case "4 фильтр": { break; }
                case "5 фильтр": { break; }
                case "6 фильтр": { break; }
                case "рандом": { break; }
            }
        }
    }

    public unsafe class UnsafeBitmap
    {
        Bitmap bitmap;

        // three elements used for MakeGreyUnsafe
        int width;
        BitmapData bitmapData = null;
        Byte* pBase = null;

        public UnsafeBitmap(Bitmap bitmap)
        {
            this.bitmap = new Bitmap(bitmap);
        }

        public UnsafeBitmap(int width, int height)
        {
            this.bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }

        public Bitmap Bitmap
        {
            get
            {
                return (bitmap);
            }
        }

        private Point PixelSize
        {
            get
            {
                GraphicsUnit unit = GraphicsUnit.Pixel;
                RectangleF bounds = bitmap.GetBounds(ref unit);

                return new Point((int)bounds.Width, (int)bounds.Height);
            }
        }

        public void LockBitmap()
        {
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF boundsF = bitmap.GetBounds(ref unit);
            Rectangle bounds = new Rectangle((int)boundsF.X,
           (int)boundsF.Y,
           (int)boundsF.Width,
           (int)boundsF.Height);

            // Figure out the number of bytes in a row
            // This is rounded up to be a multiple of 4
            // bytes, since a scan line in an image must always be a multiple of 4 bytes
            // in length. 
            width = (int)boundsF.Width * sizeof(PixelData);
            if (width % 4 != 0)
            {
                width = 4 * (width / 4 + 1);
            }
            bitmapData =
           bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            pBase = (Byte*)bitmapData.Scan0.ToPointer();
        }

        public PixelData GetPixel(int x, int y)
        {
            PixelData returnValue = *PixelAt(x, y);
            return returnValue;
        }

        public void SetPixel(int x, int y, PixelData colour)
        {
            PixelData* pixel = PixelAt(x, y);
            *pixel = colour;
        }

        public void UnlockBitmap()
        {
            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
        }
        public PixelData* PixelAt(int x, int y)
        {
            return (PixelData*)(pBase + y * width + x * sizeof(PixelData));
        }
    }
    public struct PixelData
    {
        public byte B;
        public byte G;
        public byte R;
    }
    /*
     * ШУМ
     * белый (по всему)
     * аддитивынй (рандом число к каждому пикселю)
     * гаусовский (распределение рандома - колокол)
     */
}

