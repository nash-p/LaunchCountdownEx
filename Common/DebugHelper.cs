using LaunchCountDown.Config;
using UnityEngine;

namespace LaunchCountDown.Common
{
    static class DebugHelper
    {
        public static bool IsDebug
        {
            get { return LaunchCountdownConfig.Instance.Info.IsDebug; }
        }

        public static void WriteMessage(string message, params object[] info)
        {
            if (IsDebug)
            {
                Debug.Log(info == null ? message : string.Format(message, info));
            }
        }
    }
}
