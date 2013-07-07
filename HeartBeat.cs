using System;
using System.Threading;
using Microsoft.SPOT;

namespace AgentIntervals
{
    public delegate void HeartBeatEventHandler(object sender, EventArgs e);
    
    public class HeartBeat
    {
        private Timer _timer;
        private int _period;

        public event HeartBeatEventHandler OnHeartBeat;

        public HeartBeat(int period)
        {
            _period = period;
        }

        private void TimerCallback(object state)
        {
            if (OnHeartBeat != null)
            {
                OnHeartBeat(this, new EventArgs());
            }
        }

        public void Start(int delay)
        {
            if (_timer != null) return;

            _timer = new Timer(TimerCallback, null, delay, _period);
        }

        public void Stop()
        {
            if (_timer == null) return;

            _timer.Dispose();
            _timer = null;
        }

        public void ChangePeriod(int newPeriod)
        {
            _period = newPeriod;
        }
    }
}
