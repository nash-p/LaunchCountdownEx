using NASA_CountDown.Common;
using NASA_CountDown.StateMachine;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace NASA_CountDown.States
{
    public abstract class BaseGuiState: KFSMState, IGuiBehavior
    {
        protected KerbalFsmEx Machine;

        protected BaseGuiState(string name, KerbalFsmEx machine) : base(name)
        {
            Machine = machine;
        }

        public abstract void Draw();
    }
}