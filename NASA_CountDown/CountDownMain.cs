using KSP.UI.Screens;
using NASA_CountDown.Common;
using NASA_CountDown.Config;
using NASA_CountDown.StateMachine;
using NASA_CountDown.States;

using UnityEngine;

using ToolbarControl_NS;


namespace NASA_CountDown
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(CountDownMain.MODID, CountDownMain.MODNAME);
        }
    }

    //[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    //public class CountDownMain : ScenarioModule
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CountDownMain : MonoBehaviour
    {
        static public CountDownMain instance;
        private KerbalFsmEx _machine;
        //private ApplicationLauncherButton _button;
        ToolbarControl toolbarControl;
        public SaveLoadWinPos saveLoadWinPos; // = new SaveLoadWinPos();

        internal const string MODID = "Countdown_NS";
        internal const string MODNAME = "NASA CountDown Clock";

        public void Awake()
        //public override void OnAwake()
        {
            Log.Info("Awake");
            instance = this;

            // Create the state machine
            _machine = new KerbalFsmEx();
            saveLoadWinPos = new SaveLoadWinPos();
            InitMachine();

            GravityTurnAPI.VerifyGTVersion();
        }

        void Start()
        {
            Log.Info("Start");

            ConfigInfo.Instance.Load();
            GravityTurnAPI.GravityTurnActive = false;
            ConfigInfo.Instance.InitNewConfig();

            toolbarControl = gameObject.AddComponent<ToolbarControl>();
            toolbarControl.AddToAllToolbars(ToggleOn,
                 ToggleOff,
                ApplicationLauncher.AppScenes.FLIGHT,
                MODID,
                "countdownButton",
                "NASA_CountDown/Icons/launch_icon_normal_38",
                "NASA_CountDown/Icons/launch_icon_normal_24",
                MODNAME
            );
        }

        void ToggleOff()
        {
            Log.Info("ToggleOff");
            _machine.RunEvent("Finish");

            saveLoadWinPos.LoadWindowPositions();
            
            saveLoadWinPos.initialWindow = initial._windowRect;
            saveLoadWinPos.sequenceWindow = sequence._windowRect;
            saveLoadWinPos.settingsWindow = settings._windowRect;
            saveLoadWinPos.SaveSettings();
        }

        private void ToggleOn()
        {
            if (GravityTurnAPI.GravityTurnActive)
            {
                Debug.Log("ToggleOn, GravityturnActive");
                return;
            }

            Log.Info("ToggleOn");
            if (!_machine.Started)
            {
                _machine.StartFSM("Init");
            } else {
                _machine.RunEvent("Init");
            }

            saveLoadWinPos.LoadWindowPositions();
        }

        public InitialState initial;
        SettingState settings;
        SequenceState sequence;
        LaunchState launch;
        LaunchedState launched;
        KFSMState finish;

        private void InitMachine()
        {
            Log.Info("InitMachine");
          //  if (saveLoadWinPos == null)
           //     saveLoadWinPos = new SaveLoadWinPos();


            // Create the states

            initial = new InitialState("Init", _machine);
            settings = new SettingState("Settings", _machine);
            sequence = new SequenceState("Sequence", _machine);
            launch = new LaunchState("Launch", _machine);
            launched = new LaunchedState("Launched", _machine);
            finish = new KFSMState("Finish");

            // Add events to the states

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

            // Add states to the state  machine

            _machine.AddState(initial);
            _machine.AddState(settings);
            _machine.AddState(sequence);
            _machine.AddState(launch);
            _machine.AddState(finish);
        }
#if false
        public override void OnLoad(ConfigNode node)
        {
            Log.Info("OnLoad");
            ConfigInfo.Instance.Load(node);
            //_button.SetFalse();
            if (toolbarControl != null)
                toolbarControl.SetFalse(true);
            
        }

        public override void OnSave(ConfigNode node)
        {
            ConfigInfo.Instance.Save(node);
        }
#endif
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
            toolbarControl.UseBlizzy(false);
            if (_machine.Started)
            {
                var state = _machine.CurrentState as IGuiBehavior;
                if (state == null) return;

                state.Draw();
            }
            else
            {
                if (GravityTurnAPI.GravityTurnActive)
                    initial.Draw();

            }
        }

        public void OnDestroy()
        {
            Log.Info("OnDestroy");
            _machine = null;
#if false
            if (_button != null)
                ApplicationLauncher.Instance.RemoveModApplication(_button);
#endif
            if (toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }
        }

#endregion
    }
}
