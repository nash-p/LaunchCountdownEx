using NASA_CountDown.Common;
using NASA_CountDown.StateMachine;

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