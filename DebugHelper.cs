using UnityEngine;

namespace LaunchCountDown
{
    static class DebugHelper
    {
        public static bool IsDebug { get; set; }
        public static void WriteMessage(string message, params object[] info)
        {
            if (IsDebug)
            {
                Debug.Log(string.Format(message, info));
            }
        }
    }
}
