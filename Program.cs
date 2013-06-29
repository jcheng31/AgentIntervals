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

        private const int LongIntervalTime = 30;
        private const int MediumIntervalTime = 20;
        private const int ShortIntervalTime = 10;

        private static int _secondsLeft = ShortIntervalTime;
        private static IntervalType _type;

        enum IntervalType
        {
            ShortInterval,
            MediumInterval,
            LongInterval
        }

        public static void Main()
        {
            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            Timer secondTimer = new Timer(SecondTimerCallback, null, 0, 1000);

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        private static void SecondTimerCallback(object state)
        {
            DrawDisplay(_secondsLeft);
            _secondsLeft--;

            if (_secondsLeft <= 0)
            {
                _type = (IntervalType)((int)(_type + 1) % 3);

                switch (_type)
                {
                    case IntervalType.LongInterval:
                        _secondsLeft = LongIntervalTime;
                        break;
                    case IntervalType.MediumInterval:
                        _secondsLeft = MediumIntervalTime;
                        break;
                    case IntervalType.ShortInterval:
                        _secondsLeft = ShortIntervalTime;
                        break;
                }
            }


        }

        private static void DrawDisplay(int secondsLeft)
        {
            _display.Clear();

            DrawIntervalType();
            DrawSeconds(secondsLeft);

            _display.Flush();
        }

        private static void DrawIntervalType()
        {
            Font fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);
            String intervalType = null;
            int leftMargin = 20;
            switch (_type)
            {
                case IntervalType.LongInterval:
                    intervalType = "Low Intensity";
                    break;
                case IntervalType.MediumInterval:
                    intervalType = "Moderate Intensity";
                    leftMargin = 2;
                    break;
                case IntervalType.ShortInterval:
                    intervalType = "High Intensity";
                    leftMargin = 18;
                    break;
            }
            _display.DrawText(intervalType, fontNinaB, Color.White, leftMargin, 15);
        }


        static void DrawSeconds(int digit)
        {
            int onesDigit = digit % 10;
            int tensDigit = digit / 10;
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
