using System;

namespace LaunchCountDown.States
{
    public class KspGuiState: KFSMState
    {
        public KspGuiState(string name) : base(name)
        {
        }

        public Action Draw { get; set; }
    }
}