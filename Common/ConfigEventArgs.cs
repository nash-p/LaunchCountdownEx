using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchCountDown.Common
{
    class ConfigEventArgs: EventArgs
    {
        public ConfigProperties Data { get; private set; }

        public ConfigEventArgs(ConfigProperties data)
        {
            Data = data;
        }
    }
}
