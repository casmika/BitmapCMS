using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BitmapCMS
{
    class CMSImage
    {
        public CMSImage()
        {
            xc = 0;
            yc = 0;
            Ac = 0;
        }
        public static double xc, yc, Ac;
        public static bool FireDetection(Bitmap b, Color rgb, Color range, int Aref)
        {
            Bitmap Temp = new Bitmap(b);
            Point pos = new Point(0, 0);
            double x, y, A;
            x = 0;
            y = 0;
            A = 0;
            for (int i = 1; i < Temp.Width; i++)
            {
                for (int j = 1; j < Temp.Height; j++)
                {
                    Color rgb_get = Temp.GetPixel(i, j);
                    Color I = Color.FromArgb(0, 0, 0);
                    if(Math.Abs(rgb_get.R - rgb.R) < range.R)
                    {
                        if (Math.Abs(rgb_get.G - rgb.G) < range.G)
                        {
                            if (Math.Abs(rgb_get.B - rgb.B) < range.B)
                            {
                                I = rgb_get;
                                A++;
                                x += i;
                                y += j;
                            }
                        }
                    }

                    b.SetPixel(i, j, I);
                }
            }
            x = x / A;
            y = y / A;
            Ac = A;
            xc = x;
            yc = y;
            if (A > Aref) return true;
            else return false;
        }
        public static Image Resize(Bitmap image, int x, int y)
        {
            Bitmap bmp = new Bitmap(image, x, y);
            Graphics gr = Graphics.FromImage(bmp);
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }

        public static Color cekRGB(Bitmap image, int x, int y, int l)
        {
            int r = 0, g = 0, b = 0, cc = 0;
            for (int i = x - l; i < x + l; i++)
            {
                for (int j = y - l; j < y + l; j++)
                {
                    Color temp = image.GetPixel(i, j);
                    r += temp.R;
                    g += temp.G;
                    b += temp.B;
                    cc++;
                }
            }
            return Color.FromArgb(r / cc, g / cc, b / cc);
        }

        public static void SetColor(Bitmap image,  Color set)
        {
            Bitmap Temp = new Bitmap(image);
            for (int i = 0; i < Temp.Width; i++)
            {
                for (int j = 0; j < Temp.Height; j++)
                {
                    image.SetPixel(i, j, set);
                }
            }
        }
    }
}
