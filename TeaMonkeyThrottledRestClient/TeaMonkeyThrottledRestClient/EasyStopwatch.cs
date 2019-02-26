using System;
using System.Diagnostics;

namespace TeaMonkeyThrottledRestClient
{
    public class EasyStopwatch
    {
        public readonly Stopwatch _stopwatch;

        public EasyStopwatch()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start() { _stopwatch.Start(); }

        public void Stop() { _stopwatch.Stop(); }

        public void Restart() { _stopwatch.Restart(); }

        public int SecondsElapsed()
        {
            var milliseconds = _stopwatch.ElapsedMilliseconds;
            var seconds = Math.Floor((double) (milliseconds / 1000));
            return  Convert.ToInt32(seconds);
        } 

    }
}