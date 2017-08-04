using System;
using System.Linq;
using NASA_CountDown.States;

using UnityEngine;

namespace NASA_CountDown.StateMachine
{
    public class KerbalFsmEx: KerbalFSM
    {
        public void RunEvent(string eName)
        {
            var foundEvent =
                this.CurrentState.StateEvents.FirstOrDefault(
                    x => x.name.Equals(eName, StringComparison.OrdinalIgnoreCase));

            if (foundEvent == null)
            {
                Log.Info("Event not found");
                return;
            }
            this.RunEvent(foundEvent);
        }
    }
}