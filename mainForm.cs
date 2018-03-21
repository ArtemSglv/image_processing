using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace obr_iso
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        //сделать функцию для w и рекрсивный филтр!!
        double w1, w2, w3;
        System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
        BackgroundWorker bw = new BackgroundWorker();

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Bitmap bmp;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(ofd.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void UnsafeShadesOfGray()
        {
            UnsafeBitmap bmp2 = new UnsafeBitmap((Bitmap)pictureBox1.Image.Clone());
            int x, y, r, g, b; PixelData c;
            int f;
            bmp2.LockBitmap();
            for (y = 0; y < bmp2.Bitmap.Height; ++y)
            {
                for (x = 0; x < bmp2.Bitmap.Width; ++x)
                {
                    c = bmp2.GetPixel(x, y);
                    r = c.R;
                    g = c.G;
                    b = c.B;

                    f = (r + g + b) / 3;
                    //c = Color.FromArgb(255-r,255-g,255-b); //инверсия
                    //c = Color.FromArgb(f, f, f); // чб
                    c.R = (byte)f;
                    c.G = (byte)f;
                    c.B = (byte)f;

                    bmp2.SetPixel(x, y, c);
                }
            }
            bmp2.UnlockBitmap();
            pictureBox2.Image = bmp2.Bitmap;
        }

        private void ShadesOfGray()
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
                    c = Color.FromArgb(f, f, f); // чб
                    //c = Color.FromArgb(r, g, b);
                    bmp2.SetPixel(y, x, c);
                }
            }
            pictureBox2.Image = bmp2;
        }

        private void оттенкиСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnsafeShadesOfGray();
        }

        private void волныToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        double GaussNormalRandom(Random rnd)
        {
            double sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += rnd.NextDouble();
            }
            return sum - 6;
        }

        private int GaussRandomInt(Random rnd, int s, int f) // fix it!!!!!
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += rnd.Next(s, f);
            }
            return sum - f / 2;
        }

        private void GaussianNoise()
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

                    d = GaussNormalRandom(rnd);
                    //r += Convert.ToInt32(D*(rnd.NextDouble()-0.5));
                    //g += Convert.ToInt32(D * (rnd.NextDouble() - 0.5));
                    //b += Convert.ToInt32(D * (rnd.NextDouble() - 0.5));
                    r += Convert.ToInt32(D * (d));
                    g += Convert.ToInt32(D * (d));
                    b += Convert.ToInt32(D * (d));

                    if (r < 0) r = 0;
                    else if (r > 255) r = 255;
                    if (g < 0) g = 0;
                    else if (g > 255) g = 255;
                    if (b < 0) b = 0;
                    else if (b > 255) b = 255;

                    c = Color.FromArgb(r, g, b);
                    bmp2.SetPixel(y, x, c);
                }
            }
            pictureBox2.Image = bmp2;
        }

        private void шумПоГауссуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GaussianNoise();
        }

        private void paperSaltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Bitmap bmp2 = (Bitmap)pictureBox1.Image.Clone();
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
            pictureBox2.Image = bmp2;
        }

        private double[,] random() //рандомная матрица 5х5 с суммой элементов = 1   
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

        private void Conver()
        {
            Random rnd = new Random();
            double[,] matrix = random();
            /*{
                              { 0.01, 0.02, 0.03, 0.04, 0.05 },
                              {-0.75,0,0.2,1,-1 },
                              { 0,0,-0.2,1,-1},
                              { 0,-0.5,0.2,1,-1},
                              { 0,0,-0.2,1,-1}
               };
             */
            Bitmap bmp2 = (Bitmap)pictureBox1.Image.Clone();
            int x, y;
            double r = 0, g = 0, b = 0;
            Color c;
            
            swatch.Start();
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
                    r = r < 0 ? 0 : (r > 255) ? 255 : r;
                    g = g < 0 ? 0 : (g > 255) ? 255 : g;
                    b = b < 0 ? 0 : (b > 255) ? 255 : b;

                    c = Color.FromArgb(Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
                    bmp2.SetPixel(x, y, c);
                }
                bw.ReportProgress(Convert.ToInt32(swatch.Elapsed.TotalMilliseconds));
            }
            swatch.Stop();
            
            pictureBox2.Image = bmp2;
        }

        private void FastConver()
        {
            Random rnd = new Random();
            double[,] matrix = random();
            /*{
                              { 0.01, 0.02, 0.03, 0.04, 0.05 },
                              {-0.75,0,0.2,1,-1 },
                              { 0,0,-0.2,1,-1},
                              { 0,-0.5,0.2,1,-1},
                              { 0,0,-0.2,1,-1}
               };
             */
            UnsafeBitmap bmp2 = new UnsafeBitmap((Bitmap)pictureBox1.Image.Clone());
            int x, y;
            double r = 0, g = 0, b = 0;
            PixelData c=new PixelData();

            swatch.Start();
            bmp2.LockBitmap();
            for (y = 2; y < bmp2.Bitmap.Height - 2; y++)
            {
                for (x = 2; x < bmp2.Bitmap.Width - 2; x++)
                {
                    r = 0; g = 0; b = 0;
                    for (int i = -2; i < 3; i++)
                        for (int j = -2; j < 3; j++)
                        {
                            c = bmp2.GetPixel(x + i, y + j);
                            //r += c.R / 25; g += c.G / 25; b += c.B / 25;
                            r += c.R * matrix[i + 2, j + 2]; g += c.G * matrix[i + 2, j + 2]; b += c.B * matrix[i + 2, j + 2];
                        }
                    r = r < 0 ? 0 : (r > 255) ? 255 : r;
                    g = g < 0 ? 0 : (g > 255) ? 255 : g;
                    b = b < 0 ? 0 : (b > 255) ? 255 : b;

                    c.R = (byte)r;
                    c.G = (byte)g;
                    c.B = (byte)b;
                    bmp2.SetPixel(x, y, c);
                }
                
            }
            bmp2.UnlockBitmap();
            swatch.Stop();

            pictureBox2.Image = bmp2.Bitmap;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            Conver();     
            
        }
        private void bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            label1.Text = e.ProgressPercentage.ToString();
        }

        private void fastСверткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FastConver();
            label1.Text = Convert.ToInt32(swatch.Elapsed.TotalMilliseconds).ToString();
        }

        private void сверткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            bw.DoWork+= bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
                //label1.Text = swatch.Elapsed.ToString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "1 фильтр": { w1 = 0.44; w2 = 0.07; w3 = 0.0; break; }
                case "2 фильтр": { w1 = 0.27; w2 = 0.08; w3 = 0.005; break; }
                case "3 фильтр": { w1 = 0.15; w2 = 0.06; w3 = 0.02; break; }
                case "4 фильтр": { w1 = 0.1; w2 = 0.06; w3 = 0.02; break; }
                case "5 фильтр": { w1 = 0.06; w2 = 0.045; w3 = 0.03; break; }
                case "6 фильтр": { w1 = 0.06; w2 = 0.06; w3 = 0.025; break; }
                case "рандом": { Conver(); break; }
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

