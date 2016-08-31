using NASA_CountDown.StateMachine;

namespace NASA_CountDown.States
{
    public abstract class BaseGuiState: KFSMState
    {
        protected KerbalFsmEx Machine;

        protected BaseGuiState(string name, KerbalFsmEx machine) : base(name)
        {
            Machine = machine;
        }

        protected virtual void OnGui() {}

        public void OnGuiUpdate()
        {
            OnGui();
        }
    }
}