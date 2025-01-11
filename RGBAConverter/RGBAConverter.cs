using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Color = System.Drawing.Color;
using System.Drawing.Imaging;
using System.IO;

//RGBAConverter-lhx077
//Help you convert RGBA in te easiest way!
namespace RGBAConverter
{
    public class RGBAConverter
    {
        private static Bitmap Rgba2Png(int width, int height, byte[] imageData)
        {
            byte[] newData = new byte[imageData.Length];
    
            for (int x = 0; x < imageData.Length; x += 4)
            {
                byte[] pixel = new byte[4];
                Array.Copy(imageData, x, pixel, 0, 4);
    
                byte r = pixel[0];
                byte g = pixel[1];
                byte b = pixel[2];
                byte a = pixel[3];
    
                byte[] newPixel = new[] { b, g, r, a };
    
                Array.Copy(newPixel, 0, newData, x, 4);
            }
    
            imageData = newData;
    
            using (var stream = new MemoryStream(imageData))
            using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0,
                        bmp.Width,
                        bmp.Height),
                    ImageLockMode.WriteOnly,
                    bmp.PixelFormat);
    
                IntPtr pNative = bmpData.Scan0;
                Marshal.Copy(imageData, 0, pNative, imageData.Length);
    
                bmp.UnlockBits(bmpData);
                return bmp;
            }
        }
    
        private static byte[] Png2Rgba(Bitmap PNG)
        {
            byte[] rgbaB = new byte[4 * (PNG.Width * PNG.Height)];
    
            int i = 0;
    
            for (var y = 0; y < PNG.Height; y++)
            {
                for (var x = 0; x < PNG.Width; x++)
                {
                    Color pix = PNG.GetPixel(x, y);
    
                    rgbaB[i++] = pix.R;
                    rgbaB[i++] = pix.G;
                    rgbaB[i++] = pix.B;
                    rgbaB[i++] = pix.A;
                }
            }
    
            return rgbaB;
        }

        /// <summary>
        /// Convert PNG File To Raw RGBA Data
        /// </summary>
        /// <param name="PNGPath">The original PNG file path(Include file name)</param>
        /// <returns>The original byte data of RGBA File</returns>
        public static byte[] PNG2RGBA(string PNGPath)=>Png2Rgba(new Bitmap(PNGPath));

        /// <summary>
        /// Convert Raw RGBA Data To PNG File(Need Provide the target PNG File's Weight and Height)
        /// </summary>
        /// <param name="RawRGBAData">Raw RGBA Data</param>
        /// <param name="targetWeight">Target PNG File's Weight</param>
        /// <param name="targetHeight">Target PNG File's Weight</param>
        /// <returns>The PNG file's Bitmap</returns>
        public static Bitmap RGBA2PNG(byte[] RawRGBAData, int targetWeight, int targetHeight) => Rgba2Png( targetWeight, targetHeight,RawRGBAData);
        
        /// <summary>
        /// Convert Raw RGBA Data To PNG File,and save the Target PNG File(Need Provide the target PNG File's Weight and Height)
        /// </summary>
        /// <param name="RawRGBAData">Raw RGBA Data</param>
        /// <param name="targetWeight">Target PNG File's Weight</param>
        /// <param name="targetHeight">Target PNG File's Weight</param>
        public static void RGBA2PNG(byte[] RawRGBAData, int targetWeight, int targetHeight, string PNGSavingPath)
        {
            Rgba2Png(targetWeight, targetHeight,RawRGBAData).Save(PNGSavingPath);
        }
        
    }
}

