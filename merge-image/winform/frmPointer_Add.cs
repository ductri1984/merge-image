using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MyHelper;
using System.Drawing.Imaging;

namespace winform
{
    public partial class frmPointer_Add : Form
    {
        public frmPointer_Add()
        {
            InitializeComponent();
        }

        private void frmPointer_Add_Load(object sender, EventArgs e)
        {
            var setting = HelperSetting.GetSetting();
            if (setting != null && !string.IsNullOrEmpty(setting.FolderPointer))
            {
                string[] strs = Directory.GetFiles(setting.FolderPointer);
                ImageList lst = new ImageList();
                foreach (var str in strs)
                {
                    if (str.LastIndexOf(".jpg") > 0 || str.LastIndexOf(".JPG") > 0)
                    {
                        listView1.Items.Add(str.Replace(setting.FolderPointer + "\\", ""), lst.Images.Count);
                        lst.ImageSize = new System.Drawing.Size(80, 80);
                        lst.Images.Add(ResizeImage(80, 80, str));
                    }
                }
                listView1.LargeImageList = lst;
            }
        }

        public Image ResizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }
    }
}
