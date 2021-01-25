﻿using SCapture.Properties;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SCapture.Classes
{
    class ScreenCapturer
    {
        /// <summary>
        /// Captures the graphical content of the given region
        /// </summary>
        /// <returns>The image captured</returns>
        public static BitmapSource CaptureRegion(int Left, int Top, int Width, int Height)
        {
            IntPtr dc1 = NativeMethods.GetDC(NativeMethods.GetDesktopWindow());
            IntPtr dc2 = NativeMethods.CreateCompatibleDC(dc1);

            // Create Bitmap
            IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(dc1, Width, Height);

            NativeMethods.SelectObject(dc2, hBitmap);
            NativeMethods.BitBlt(dc2, 0, 0, Width, Height, dc1, Left, Top, 0x00CC0020);

            // Get BitmapSource
            BitmapSource bSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            // Release resources
            NativeMethods.DeleteObject(hBitmap);
            NativeMethods.ReleaseDC(IntPtr.Zero, dc1);
            NativeMethods.ReleaseDC(IntPtr.Zero, dc2);

            return bSource;
        }

        /// <summary>
        /// Saves the capture to a file.
        /// </summary>
        /// <param name="bSource">The image captured</param>
        /// <returns>Capture saved successfully?</returns>
        public static bool Save(BitmapSource bSource)
        {
            var path = Settings.Default.SavePath;
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string extension = ".jpg";

            switch (Settings.Default.SaveFileFormat)
            {
                case 0:
                    extension = ".bmp";
                    break;
                case 1:
                    extension = ".jpeg";
                    break;
                default:
                    extension = ".png";
                    break;

            }



            string fileName = path + $"/screenshots" + extension;

            return Save(fileName, bSource);
        }

        /// <summary>
        /// Saves the capture to a file
        /// </summary>
        /// <param name="fileName">What's the file name?</param>
        /// <param name="bSource">The image captured</param>
        /// <returns></returns>
        public static bool Save(string fileName, BitmapSource bSource)
        {
            try
            {
                BitmapEncoder encoder;
                var extension = Path.GetExtension(fileName);
                switch (extension)
                {
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    case ".jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    default:
                        encoder = new PngBitmapEncoder();
                        break;
                }

                encoder.Frames.Add(BitmapFrame.Create(bSource));

                using (var stream = File.Create(fileName))
                {
                    encoder.Save(stream);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
