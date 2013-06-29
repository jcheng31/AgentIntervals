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
        private static IntervalType _currentIntervalType;

        private static InterruptPort _upButton;
        private static InterruptPort _selectButton;
        private static InterruptPort _downButton;

        private static Timer _countdownTimer;

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

            // Keep references to the buttons.
            _upButton = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_UP), false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeLow);
            _selectButton = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_SELECT), false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeLow);
            _downButton = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_DOWN), false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeLow);

            _downButton.OnInterrupt += ResetCounter;

            ResetTimer();

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        private static void ResetCounter(uint data1, uint data2, DateTime time)
        {
            switch (_currentIntervalType)
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

            DrawDisplay(_secondsLeft);
            ResetTimer();
        }

        private static void ResetTimer()
        {
            if (_countdownTimer != null)
            {
                _countdownTimer.Dispose();
            }
            _countdownTimer = new Timer(SecondTimerCallback, null, 0, 1000);
        }

        private static void SecondTimerCallback(object state)
        {
            DrawDisplay(_secondsLeft);
            _secondsLeft--;

            if (_secondsLeft <= 0)
            {
                _currentIntervalType = (IntervalType)((int)(_currentIntervalType + 1) % 3);

                switch (_currentIntervalType)
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
            switch (_currentIntervalType)
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
            Font fontConsolaMonoBold32 = Resources.GetFont(Resources.FontResources.ConsolaMonoBold32);

            String digitText = digit.ToString("D2");
            _display.DrawText(digitText, fontConsolaMonoBold32, Color.White, 40, 32);
        }
    }
}
