using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_console
{
    class Program
    {
        static void Main(string[] args)
        {
            string imgSourcePath = @"D:\_Working\01.jpg";
            string imgTarget1Path = @"D:\_Working\01_crop1.png";
            string imgTarget2Path = @"D:\_Working\01_crop2.png";
            string imgTarget3Path = @"D:\_Working\01_crop3.png";
            string imgTarget4Path = @"D:\_Working\01_crop4.png";

            CropRectangle(imgSourcePath, imgTarget1Path, 100, 100, 200, 200);
            CropRectangle(imgSourcePath, imgTarget2Path, 100, 100, 200, 200, 5, 5);
            CropEllipse(imgSourcePath, imgTarget3Path, 100, 100, 200, 100);
            Resize(imgSourcePath, imgTarget4Path, 300, 200);
        }

        private static void CropRectangle(string pathSource, string pathTarget, int x, int y, int width, int height, int rtopleft = 0, int rtopright = 0, int rbotleft = 0, int rbotright = 0)
        {
            if (System.IO.File.Exists(pathSource))
            {
                if (System.IO.File.Exists(pathTarget))
                {
                    System.IO.File.Delete(pathTarget);
                }

                System.Drawing.Image imgSource = System.Drawing.Image.FromFile(pathSource);
                System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(x, y, width, height);
                System.Drawing.Bitmap imgTarget = new System.Drawing.Bitmap(width, height);

                if (rtopleft > 0 || rtopright > 0 || rbotleft > 0 || rbotright > 0)
                {
                    rtopright++;
                    rbotleft++;
                    rbotright++;

                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget))
                    {
                        var path = new System.Drawing.Drawing2D.GraphicsPath();

                        //draw line top and top right
                        path.AddLine(rtopleft, 0, width - (rtopleft + rtopright), 0);
                        if (rtopright > 0)
                            path.AddArc(width - (rtopright * 2), 0, rtopright * 2, rtopright * 2, 270, 90);
                        //draw line right and bottom right
                        path.AddLine(width, rtopright, width, height - (rtopright + rbotright));
                        if (rbotright > 0)
                            path.AddArc(width - (rbotright * 2), height - (rbotright * 2), rbotright * 2, rbotright * 2, 0, 90);
                        //draw line bottom and bottom left
                        path.AddLine(width - (rbotleft + rbotright), height, rbotleft, height);
                        if (rbotleft > 0)
                            path.AddArc(0, height - (rbotleft * 2), rbotleft * 2, rbotleft * 2, 90, 90);
                        //draw line left and top left
                        path.AddLine(0, height - (rtopleft + rbotleft), 0, rtopleft);
                        if (rtopleft > 0)
                            path.AddArc(0, 0, rtopleft * 2, rtopleft * 2, 180, 90);

                        path.CloseFigure();

                        g.SetClip(path, System.Drawing.Drawing2D.CombineMode.Replace);
                        g.DrawImage(imgSource, new System.Drawing.Rectangle(0, 0, imgTarget.Width, imgTarget.Height), cropArea, System.Drawing.GraphicsUnit.Pixel);
                        imgTarget.Save(pathTarget, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                else
                {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget))
                    {
                        g.DrawImage(imgSource, new System.Drawing.Rectangle(0, 0, imgTarget.Width, imgTarget.Height), cropArea, System.Drawing.GraphicsUnit.Pixel);
                        imgTarget.Save(pathTarget, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        private static void CropEllipse(string pathSource, string pathTarget, int x, int y, int radiusWidth, int radiusHeight)
        {
            if (System.IO.File.Exists(pathSource))
            {
                if (System.IO.File.Exists(pathTarget))
                {
                    System.IO.File.Delete(pathTarget);
                }

                System.Drawing.Image imgSource = System.Drawing.Image.FromFile(pathSource);
                System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(x, y, radiusWidth * 2, radiusHeight * 2);
                System.Drawing.Bitmap imgTarget = new System.Drawing.Bitmap(radiusWidth * 2, radiusHeight * 2);

                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget))
                {
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddEllipse(0, 0, radiusWidth * 2, radiusHeight * 2);
                    path.CloseFigure();

                    g.SetClip(path, System.Drawing.Drawing2D.CombineMode.Replace);
                    g.DrawImage(imgSource, new System.Drawing.Rectangle(0, 0, imgTarget.Width, imgTarget.Height), cropArea, System.Drawing.GraphicsUnit.Pixel);
                    imgTarget.Save(pathTarget, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private static void Resize(string pathSource, string pathTarget, int width, int height)
        {
            if (System.IO.File.Exists(pathSource))
            {
                if (System.IO.File.Exists(pathTarget))
                {
                    System.IO.File.Delete(pathTarget);
                }

                System.Drawing.Image imgSource = System.Drawing.Image.FromFile(pathSource);

                // Get the image's original width and height
                int originalWidth = imgSource.Width;
                int originalHeight = imgSource.Height;

                // To preserve the aspect ratio
                float ratio = 1;
                if (width > 0 && height > 0)
                {
                    ratio = 0;
                }
                else if(width > 0)
                {
                    ratio = (float)width / (float)originalWidth;
                }
                else if(height > 0)
                {
                    ratio = (float)height / (float)originalHeight;
                }

                // New width and height based on aspect ratio
                int newWidth = (int)(originalWidth * ratio);
                int newHeight = (int)(originalHeight * ratio);
                if(ratio == 0)
                {
                    newWidth = width;
                    newHeight = height;
                }

                System.Drawing.Bitmap imgTarget = new System.Drawing.Bitmap(newWidth, newHeight);

                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    g.DrawImage(imgSource, 0, 0, imgTarget.Width, imgTarget.Height);
                    imgTarget.Save(pathTarget, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
    }
}
