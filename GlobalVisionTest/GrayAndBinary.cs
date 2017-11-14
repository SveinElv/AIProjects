using System;
using System.Drawing;

namespace GlobalVisionTest
{
    public class GrayAndBinary
    {
        public static void ToGrey(Bitmap img1)
        {
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    int grey = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    Color newColor = Color.FromArgb(grey, grey, grey);
                    img1.SetPixel(i, j, newColor);
                }
            }
            img1.Save(Utility.GRAYED_IMG_PATH);
        }
        public static void Thresholding(Bitmap img1)
        {
            int[] histogram = new int[256];
            int minGrayValue = 255, maxGrayValue = 0;
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    histogram[pixelColor.R]++;
                    if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
                    if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
                }
            }
            int threshold = -1;
            int newThreshold = (minGrayValue + maxGrayValue) / 2;
            for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
            {
                threshold = newThreshold;
                int lP1 = 0;
                int lP2 = 0;
                int lS1 = 0;
                int lS2 = 0;
                for (int i = minGrayValue; i < threshold; i++)
                {
                    lP1 += histogram[i] * i;
                    lS1 += histogram[i];
                }
                int mean1GrayValue = (lP1 / lS1);
                for (int i = threshold + 1; i < maxGrayValue; i++)
                {
                    lP2 += histogram[i] * i;
                    lS2 += histogram[i];
                }
                int mean2GrayValue = (lP2 / lS2);
                newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
            }
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    if (pixelColor.R > threshold) img1.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else img1.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            img1.Save(Utility.BINARIED_IMG_PATH);
        }

        /*
        static void Main(string[] args)
        {
            try
            {
                //打开位图文件
                Bitmap img1 = new Bitmap("test.jpg", true);
                //灰度化
                ToGrey(img1);
                //二值化
                Thresholding(img1);
                //写回位图文件
                img1.Save("output.jpg");
                Console.WriteLine("Converted.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid usage!");
                Console.WriteLine("Usage: bmp2grey source object");
            }
        }
        **/
    }
}