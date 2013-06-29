using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;

namespace AgentIntervals
{
    public class Program
    {
        static Bitmap _display;

        public static void Main()
        {
            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // sample "hello world" code
            _display.Clear();
            Font fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);
            _display.DrawText("Hello world.", fontNinaB, Color.White, 10, 64);
            _display.Flush();


        static void DrawDigitOnBitmap(int digit)
        {
            int onesDigit = digit%10;
            int tensDigit = digit/10;
            _display.DrawImage(64, 32, GetDigit(onesDigit), 12, 0, 40, 64);
            _display.DrawImage(24, 32, GetDigit(tensDigit), 12, 0, 40, 64);
        }

        static Bitmap GetDigit(int digitNumber)
        {
            switch (digitNumber)
            {
                case 0:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._0), Bitmap.BitmapImageType.Gif);
                case 1:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._1), Bitmap.BitmapImageType.Gif);
                case 2:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._2), Bitmap.BitmapImageType.Gif);
                case 3:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._3), Bitmap.BitmapImageType.Gif);
                case 4:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._4), Bitmap.BitmapImageType.Gif);
                case 5:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._5), Bitmap.BitmapImageType.Gif);
                case 6:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._6), Bitmap.BitmapImageType.Gif);
                case 7:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._7), Bitmap.BitmapImageType.Gif);
                case 8:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._8), Bitmap.BitmapImageType.Gif);
                case 9:
                    return new Bitmap(Resources.GetBytes(Resources.BinaryResources._9), Bitmap.BitmapImageType.Gif);
                default:
                    return null;
            }
        }

    }
}
