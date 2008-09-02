using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace littleRunner
{
    public class StopwatchExtended : Stopwatch
    {
        private TimeSpan startAt;

        public new TimeSpan Elapsed
        {
            get { return base.Elapsed + startAt; }
            set
            {
                bool wasRunning = base.IsRunning;

                startAt = value;
                base.Reset();

                if (wasRunning)
                    base.Start();
            }
        }

        public StopwatchExtended()
        {
            this.startAt = new TimeSpan();
        }
    }
}
