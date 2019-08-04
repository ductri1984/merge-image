using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo_form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string imgSourcePath = @"D:\_Working\01.jpg";
            string imgTarget1Path = @"D:\_Working\01_crop1.png";
            string imgTarget2Path = @"D:\_Working\01_crop2.png";
            string imgTarget3Path = @"D:\_Working\01_crop3.png";
            if (System.IO.File.Exists(imgTarget1Path))
            {
                System.IO.File.Delete(imgTarget1Path);
            }
            if (System.IO.File.Exists(imgTarget2Path))
            {
                System.IO.File.Delete(imgTarget2Path);
            }
            if (System.IO.File.Exists(imgTarget3Path))
            {
                System.IO.File.Delete(imgTarget3Path);
            }

            System.Drawing.Image imgSource = System.Drawing.Image.FromFile(imgSourcePath);
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(100, 100, 200, 200);
            System.Drawing.Bitmap imgTarget1 = new System.Drawing.Bitmap(200, 200);
            System.Drawing.Bitmap imgTarget2 = new System.Drawing.Bitmap(200, 200);
            System.Drawing.Bitmap imgTarget3 = new System.Drawing.Bitmap(200, 200);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget1))
            {
                //Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                //Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //Graphic.DrawImage(OriginalImage, new SD.Rectangle(0, 0, Width, Height), X, Y, Width, Height, SD.GraphicsUnit.Pixel);
                //MemoryStream ms = new MemoryStream();
                //bmp.Save(ms, OriginalImage.RawFormat);

                g.DrawImage(imgSource, new Rectangle(0, 0, imgTarget1.Width, imgTarget1.Height), cropArea, GraphicsUnit.Pixel);
                imgTarget1.Save(imgTarget1Path, System.Drawing.Imaging.ImageFormat.Png);
            }

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget2))
            {
                System.Drawing.Drawing2D.GraphicsPath path = DrawRectangleRadius(0, 0, 200, 200, 5, 6, 6, 7);
                g.SetClip(path, System.Drawing.Drawing2D.CombineMode.Replace);
                g.DrawImage(imgSource, new Rectangle(0, 0, imgTarget2.Width, imgTarget2.Height), cropArea, GraphicsUnit.Pixel);

                imgTarget2.Save(imgTarget2Path, System.Drawing.Imaging.ImageFormat.Png);
            }

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgTarget3))
            {
                System.Drawing.Drawing2D.GraphicsPath path = DrawEllipse(0, 0, 50, 100);
                g.SetClip(path, System.Drawing.Drawing2D.CombineMode.Replace);
                g.DrawImage(imgSource, new Rectangle(0, 0, imgTarget3.Width, imgTarget3.Height), cropArea, GraphicsUnit.Pixel);

                imgTarget3.Save(imgTarget3Path, System.Drawing.Imaging.ImageFormat.Png);
            }


            //// Crop Image Here & Save  
            //string fileName = Path.GetFileName(imgUpload.ImageUrl);
            //string filePath = Path.Combine(Server.MapPath("~/UploadImages"), fileName);
            //string cropFileName = "";
            //string cropFilePath = "";
            //if (File.Exists(filePath))
            //{
            //    System.Drawing.Image orgImg = System.Drawing.Image.FromFile(filePath);
            //    Rectangle CropArea = new Rectangle(Convert.ToInt32(X.Value), Convert.ToInt32(Y.Value), Convert.ToInt32(W.Value), Convert.ToInt32(H.Value));
            //    try
            //    {
            //        Bitmap bitMap = new Bitmap(CropArea.Width, CropArea.Height);
            //        using (Graphics g = Graphics.FromImage(bitMap))
            //        {
            //            g.DrawImage(orgImg, new Rectangle(0, 0, bitMap.Width, bitMap.Height), CropArea, GraphicsUnit.Pixel);
            //            //g.DrawImage()

            //            //g.FillClosedCurve
            //        }

            //        cropFileName = "crop_" + fileName;
            //        cropFilePath = Path.Combine(Server.MapPath("~/UploadImages"), cropFileName);
            //        bitMap.Save(cropFilePath);
            //        Response.Redirect("~/UploadImages/" + cropFileName, false);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}
        }

        public static System.Drawing.Drawing2D.GraphicsPath DrawRectangleRadius(int x, int y, int height, int width, int rtopleft, int rtopright, int rbotleft, int rbotright)
        {
            var result = new System.Drawing.Drawing2D.GraphicsPath();

            //draw line top and top right
            result.AddLine(x + rtopleft, y, x + width - (rtopleft + rtopright), y);
            if (rtopright > 0)
                result.AddArc(x + width - (rtopright * 2), y, rtopright * 2, rtopright * 2, 270, 90);
            //draw line right and bottom right
            result.AddLine(x + width, y + rtopright, x + width, y + height - (rtopright + rbotright));
            if (rbotright > 0)
                result.AddArc(x + width - (rbotright * 2), y + height - (rbotright * 2), rbotright * 2, rbotright * 2, 0, 90);
            //draw line bottom and bottom left
            result.AddLine(x + width - (rbotleft + rbotright), y + height, x + rbotleft, y + height);
            if (rbotleft > 0)
                result.AddArc(x, y + height - (rbotleft * 2), rbotleft * 2, rbotleft * 2, 90, 90);
            //draw line left and top left
            result.AddLine(x, y + height - (rtopleft + rbotleft), x, y + rtopleft);
            if (rtopleft > 0)
                result.AddArc(x, y, rtopleft * 2, rtopleft * 2, 180, 90);

            //Path.AddLine(XPosition + CornerRadius, YPosition, XPosition + Width - (CornerRadius * 2), YPosition);
            //Path.AddArc(XPosition + Width - (CornerRadius * 2), YPosition, CornerRadius * 2, CornerRadius * 2, 270, 90);
            //Path.AddLine(XPosition + Width, YPosition + CornerRadius, XPosition + Width, YPosition + Height - (CornerRadius * 2));
            //Path.AddArc(XPosition + Width - (CornerRadius * 2), YPosition + Height - (CornerRadius * 2), CornerRadius * 2, CornerRadius * 2, 0, 90);
            //Path.AddLine(XPosition + Width - (CornerRadius * 2), YPosition + Height, XPosition + CornerRadius, YPosition + Height);
            //Path.AddArc(XPosition, YPosition + Height - (CornerRadius * 2), CornerRadius * 2, CornerRadius * 2, 90, 90);
            //Path.AddLine(XPosition, YPosition + Height - (CornerRadius * 2), XPosition, YPosition + CornerRadius);
            //Path.AddArc(XPosition, YPosition, CornerRadius * 2, CornerRadius * 2, 180, 90);

            result.CloseFigure();

            return result;
        }

        public static System.Drawing.Drawing2D.GraphicsPath DrawEllipse(int x, int y, int height, int width)
        {
            var result = new System.Drawing.Drawing2D.GraphicsPath();

            result.AddEllipse(x, y, width * 2, height * 2);

            result.CloseFigure();

            return result;
        }

        // Draw a rectangle in the indicated Rectangle
        // rounding the indicated corners.
        private static System.Drawing.Drawing2D.GraphicsPath MakeRoundedRect(RectangleF rect, float xradius, float yradius,
            bool round_ul, bool round_ur, bool round_lr, bool round_ll)
        {
            // Make a GraphicsPath to draw the rectangle.
            PointF point1, point2;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            //path.FillMode = System.Drawing.Drawing2D.FillMode.Winding;

            // Upper left corner.
            if (round_ul)
            {
                RectangleF corner = new RectangleF(
                    rect.X, rect.Y,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 180, 90);
                point1 = new PointF(rect.X + xradius, rect.Y);
            }
            else point1 = new PointF(rect.X, rect.Y);

            // Top side.
            if (round_ur)
                point2 = new PointF(rect.Right - xradius, rect.Y);
            else
                point2 = new PointF(rect.Right, rect.Y);
            path.AddLine(point1, point2);

            // Upper right corner.
            if (round_ur)
            {
                RectangleF corner = new RectangleF(
                    rect.Right - 2 * xradius, rect.Y,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 270, 90);
                point1 = new PointF(rect.Right, rect.Y + yradius);
            }
            else point1 = new PointF(rect.Right, rect.Y);

            // Right side.
            if (round_lr)
                point2 = new PointF(rect.Right, rect.Bottom - yradius);
            else
                point2 = new PointF(rect.Right, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower right corner.
            if (round_lr)
            {
                RectangleF corner = new RectangleF(
                    rect.Right - 2 * xradius,
                    rect.Bottom - 2 * yradius,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 0, 90);
                point1 = new PointF(rect.Right - xradius, rect.Bottom);
            }
            else point1 = new PointF(rect.Right, rect.Bottom);

            // Bottom side.
            if (round_ll)
                point2 = new PointF(rect.X + xradius, rect.Bottom);
            else
                point2 = new PointF(rect.X, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower left corner.
            if (round_ll)
            {
                RectangleF corner = new RectangleF(
                    rect.X, rect.Bottom - 2 * yradius,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 90, 90);
                point1 = new PointF(rect.X, rect.Bottom - yradius);
            }
            else point1 = new PointF(rect.X, rect.Bottom);

            // Left side.
            if (round_ul)
                point2 = new PointF(rect.X, rect.Y + yradius);
            else
                point2 = new PointF(rect.X, rect.Y);
            path.AddLine(point1, point2);

            // Join with the start point.
            path.CloseFigure();

            return path;
        }
    }
}
