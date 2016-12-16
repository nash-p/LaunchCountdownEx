using KSP.UI.Screens;
using NASA_CountDown.Common;
using NASA_CountDown.Config;
using NASA_CountDown.StateMachine;
using NASA_CountDown.States;

namespace NASA_CountDown
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class CountDownMain : ScenarioModule
    {
        private KerbalFsmEx _machine;
        private ApplicationLauncherButton _button;

        public override void OnAwake()
        {
            _machine = new KerbalFsmEx();
            InitMachine();
            _button = ApplicationLauncher.Instance.AddModApplication(() => _machine.RunEvent("Finish"),
                () => _machine.RunEvent("Init"), () => { }, () => { }, () => { }, () => { },
                ApplicationLauncher.AppScenes.FLIGHT,
                GameDatabase.Instance.GetTexture("NASA_CountDown/Icons/launch_icon_normal", false));
        }

        private void InitMachine()
        {
            var initial = new InitialState("Init", _machine);
            var settings = new SettingState("Settings", _machine);
            var sequence = new SequenceState("Sequence", _machine);
            var launch = new LaunchState("Launch", _machine);
            var launched = new LaunchedState("Launched", _machine);
            var finish = new KFSMState("Finish");

            var go2Finish = new KFSMEvent("Finish")
            {
                GoToStateOnEvent = finish,
                updateMode = KFSMUpdateMode.MANUAL_TRIGGER
            };

            var go2Settings = new KFSMEvent("Settings") { GoToStateOnEvent = settings, updateMode = KFSMUpdateMode.MANUAL_TRIGGER };
            initial.AddEvent(go2Settings);

            var go2Init = new KFSMEvent("Init") { GoToStateOnEvent = initial, updateMode = KFSMUpdateMode.MANUAL_TRIGGER };
            settings.AddEvent(go2Init);
            sequence.AddEvent(go2Init);
            finish.AddEvent(go2Init);

            var go2Sequence = new KFSMEvent("Sequence") { GoToStateOnEvent = sequence, updateMode = KFSMUpdateMode.MANUAL_TRIGGER };
            initial.AddEvent(go2Sequence);

            var go2Launch = new KFSMEvent("Launch") { GoToStateOnEvent = launch, updateMode = KFSMUpdateMode.MANUAL_TRIGGER };
            initial.AddEvent(go2Launch);
            launch.AddEvent(go2Init);
            launch.AddEvent(go2Finish);

            var go2Launched = new KFSMEvent("Launched") { GoToStateOnEvent = launched, updateMode = KFSMUpdateMode.MANUAL_TRIGGER };
            launch.AddEvent(go2Launched);

            initial.AddEvent(go2Finish);
            launched.AddEvent(go2Finish);

            _machine.AddState(initial);
            _machine.AddState(settings);
            _machine.AddState(sequence);
            _machine.AddState(launch);
            _machine.AddState(finish);
        }

        public override void OnLoad(ConfigNode node)
        {
            ConfigInfo.Instance.Load(node);
            _machine.StartFSM("Init");
        }

        public override void OnSave(ConfigNode node)
        {
            ConfigInfo.Instance.Save(node);
        }

        #region Unity

        public void FixedUpdate()
        {
            if (_machine.Started)
            {
                _machine.FixedUpdateFSM();
            }
        }

        public void Update()
        {
            if (_machine.Started)
            {
                _machine.UpdateFSM();
            }
        }

        public void LateUpdate()
        {
            if (_machine.Started)
            {
                _machine.LateUpdateFSM();
            }
        }

        public void OnGUI()
        {
            if (_machine.Started)
            {
                var state =_machine.CurrentState as IGuiBehavior;
                if (state == null) return;

                state.Draw();
            }
        }

        public void OnDestroy()
        {
            _machine = null;
            ApplicationLauncher.Instance.RemoveModApplication(_button);
        }

        #endregion
    }
}
