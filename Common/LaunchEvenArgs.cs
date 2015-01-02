using System;

namespace LaunchCountDown.Common
{
    class LaunchEvenArgs:EventArgs
    {
        public int Tick { get; private set; }

        public LaunchEvenArgs(int tick)
        {
            Tick = tick;
        }
    }
}
