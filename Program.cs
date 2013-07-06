using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;

namespace AgentIntervals
{
    public class Program
    {
        static Bitmap _display;

        private static int LongIntervalTime = 30;
        private static int MediumIntervalTime = 20;
        private static int ShortIntervalTime = 10;

        private static int _secondsLeft = LongIntervalTime;
        private static IntervalType _currentIntervalType;

        private static InterruptPort _upButton;
        private static InterruptPort _selectButton;
        private static InterruptPort _downButton;

        private static Timer _timer;

        private static Font _fontNinaB;
        private static Font _fontConsolaMonoBold32;

        private static bool _shouldDrawTime = true;

        enum IntervalType
        {
            LongInterval,
            MediumInterval,
            ShortInterval
        }

        public static void Main()
        {
            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // Keep references to the buttons.
            _upButton = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_UP), false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);
            _selectButton = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_SELECT), false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);
            _downButton = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_DOWN), false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);

            // Get our fonts.
            _fontConsolaMonoBold32 = Resources.GetFont(Resources.FontResources.ConsolaMonoBold32);
            _fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);

            EnterTimerMode();

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        private static void EnterTimerMode()
        {
            StopTimer();
            SetTimerModeButtons();
            _currentIntervalType = IntervalType.LongInterval;
            _secondsLeft = LongIntervalTime;
            DrawTimerDisplay(_secondsLeft);
        }

        private static void EnterAdjustMode(uint data1, uint data2, DateTime time)
        {
            StopTimer();
            SetAdjustModeButtons();

            _currentIntervalType = IntervalType.LongInterval;

            _timer = new Timer(AdjustModeTimerCallback, null, 0, 250);
        }

        private static void AdjustModeTimerCallback(object state)
        {
            _display.Clear();

            DrawIntervalType();

            if (_shouldDrawTime)
            {
                switch (_currentIntervalType)
                {
                    case IntervalType.LongInterval:
                        DrawSeconds(LongIntervalTime);
                        break;
                    case IntervalType.MediumInterval:
                        DrawSeconds(MediumIntervalTime);
                        break;
                    case IntervalType.ShortInterval:
                        DrawSeconds(ShortIntervalTime);
                        break;
                }
            }
            _shouldDrawTime = !_shouldDrawTime;

            _display.Flush();
        }

        private static void SetTimerModeButtons()
        {
            _downButton.OnInterrupt -= AdjustTimeDown;
            _upButton.OnInterrupt -= AdjustTimeUp;

            _downButton.OnInterrupt += ResetCounter;
            _upButton.OnInterrupt += ToggleTimer;
            _selectButton.OnInterrupt += EnterAdjustMode;
        }

        private static void SetAdjustModeButtons()
        {
            _downButton.OnInterrupt -= ResetCounter;
            _upButton.OnInterrupt -= ToggleTimer;
            _selectButton.OnInterrupt -= EnterAdjustMode;

            _downButton.OnInterrupt += AdjustTimeDown;
            _upButton.OnInterrupt += AdjustTimeUp;
        }

        private static void AdjustTimeUp(uint data1, uint data2, DateTime time)
        {
            switch (_currentIntervalType)
            {
                case IntervalType.LongInterval:
                    LongIntervalTime++;
                    break;
                case IntervalType.MediumInterval:
                    MediumIntervalTime++;
                    break;
                case IntervalType.ShortInterval:
                    ShortIntervalTime++;
                    break;
            }
        }

        private static void AdjustTimeDown(uint data1, uint data2, DateTime time)
        {
            switch (_currentIntervalType)
            {
                case IntervalType.LongInterval:
                    LongIntervalTime--;
                    break;
                case IntervalType.MediumInterval:
                    MediumIntervalTime--;
                    break;
                case IntervalType.ShortInterval:
                    ShortIntervalTime--;
                    break;
            }
        }

        private static void ToggleTimer(uint data1, uint data2, DateTime time)
        {
            if (_timer == null)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }

        private static void StartTimer()
        {
            _timer = new Timer(SecondTimerCallback, null, 250, 1000);
            _selectButton.OnInterrupt -= EnterAdjustMode;
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

            DrawTimerDisplay(_secondsLeft);
            if (_timer != null)
            {
                ResetTimer();
            }
        }

        private static void ResetTimer()
        {
            StopTimer();
            StartTimer();
        }

        private static void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            _selectButton.OnInterrupt += EnterAdjustMode;
        }

        private static void SecondTimerCallback(object state)
        {
            _secondsLeft--;
            DrawTimerDisplay(_secondsLeft);

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

        private static void DrawTimerDisplay(int secondsLeft)
        {
            _display.Clear();

            DrawIntervalType();
            DrawSeconds(secondsLeft);

            _display.Flush();
        }

        private static void DrawIntervalType()
        {
            String intervalType = null;
            switch (_currentIntervalType)
            {
                case IntervalType.LongInterval:
                    intervalType = "Low Intensity";
                    break;
                case IntervalType.MediumInterval:
                    intervalType = "Moderate Intensity";
                    break;
                case IntervalType.ShortInterval:
                    intervalType = "High Intensity";
                    break;
            }

            int width, height;
            _fontNinaB.ComputeExtent(intervalType, out width, out height);

            int leftMargin = (128 - width) / 2 + 2;

            _display.DrawText(intervalType, _fontNinaB, Color.White, leftMargin, 15);
        }


        static void DrawSeconds(int digit)
        {
            String timeLeft = digit.ToString("D2");
            int width, height;
            _fontConsolaMonoBold32.ComputeExtent(timeLeft, out width, out height);

            int leftMargin = (128 - width) / 2;

            _display.DrawText(timeLeft, _fontConsolaMonoBold32, Color.White, leftMargin, 32);
        }
    }
}
