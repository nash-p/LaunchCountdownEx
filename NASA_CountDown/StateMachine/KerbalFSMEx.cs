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
            Debug.Log("NASA_CountDown.RunEvent: " + eName);
            var foundEvent =
                this.CurrentState.StateEvents.FirstOrDefault(
                    x => x.name.Equals(eName, StringComparison.OrdinalIgnoreCase));

            if (foundEvent == null)
            {
                Debug.Log("Event not found");
                return;
            }
            Debug.Log("running event: " + eName);
            this.RunEvent(foundEvent);
        }
    }
}