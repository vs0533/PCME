using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure
{
    public class ImageHelper
    {
        public static string ImgToBase64String(string Imagefilename)
        {
            try
            {
                string base64 = string.Empty;
                using (var ms = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(Imagefilename);
                    Bitmap bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
                    //将第一个bmp拷贝到bmp2中
                    Graphics draw = Graphics.FromImage(bmp2);
                    draw.DrawImage(bmp, 0, 0);
                    draw.Dispose();
                    bmp2.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    bmp2.Dispose();
                    bmp.Dispose();
                    base64 = Convert.ToBase64String(arr);
                }
                return base64;
            }
            catch
            {
                return null;
            }
        }
        public static void Base64StringToImage(string inputStr,string path)
        {
            if (string.IsNullOrWhiteSpace(inputStr))
                return;
            try
            {
                string filePath = path;//hostingEnv.WebRootPath + $@"\Files";
                //string filePath = "E:\\bb.jpg";
                byte[] arr = Convert.FromBase64String(inputStr);//(inputStr.Substring(inputStr.IndexOf("base64,") + 7).Trim('\0'));
                using (MemoryStream ms = new MemoryStream(arr))
                {
                    Bitmap bmp = new Bitmap(ms);
                    //新建第二个bitmap类型的bmp2变量。
                    //Bitmap bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
                    //将第一个bmp拷贝到bmp2中
                    //Graphics draw = Graphics.FromImage(bmp2);
                    //draw.DrawImage(bmp, 0, 0);
                    //draw.Dispose();
                    bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Close();
                }
            }
            catch
            {

            }
            finally
            {

            }
        }
    }
}
