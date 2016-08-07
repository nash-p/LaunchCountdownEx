using System;
using System.Collections;
using KspHelper.Behavior;
using LaunchCountDown.Common;
using LaunchCountDown.Config;
using LaunchCountDown.States;
using UnityEngine;

namespace LaunchCountDown.Windows
{
    public delegate void OnDrawCallback();

    public class GuiState : KFSMState
    {
        public GuiState(string name) : base(name)
        {
            DrawCallback = () => { };
        }

        public OnDrawCallback DrawCallback { get; set; }
    }

    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class CountDownScenario : ScenarioModule
    {
        private int _tick;
        private LaunchStates _launchState;
        private bool _buttonOpened;
        private SettingsWindow _settingsWindow;
        private LaunchSequenceWindow _launchSequenceWindow;
        private LaunchControl _launcher;
        private ApplicationLauncherButton _launcherButton;
        private KerbalFSM _machine;
        private string _stateName = "Init";

        public static CountDownScenario Instance { get; private set; }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            _stateName = node.GetValue("stateName") ?? "Init";
        }

        public override void OnSave(ConfigNode node)
        {
            node.AddValue("stateName", _stateName);
            base.OnSave(node);
        }

        public void OnGUI()
        {
            GuiState current = _machine.CurrentState as GuiState;
            current?.DrawCallback();
        }

        public Rect WindowRect { get; private set; }

        public bool Visible { get; set; }

        public void RunCoroutine(IEnumerator data)
        {
            StartCoroutine(data);
        }

        public override void OnAwake()
        {
            Instance = this;

            _launcherButton = ApplicationLauncher.Instance.AddModApplication(() => Visible = true, () => Visible = false,
                () => { }, () => { },
                () => { }, () => { }, ApplicationLauncher.AppScenes.FLIGHT,
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Icons/launch_icon_normal", false));

            WindowRect = LaunchCountdownConfig.Instance.Info.IsLoaded
                ? LaunchCountdownConfig.Instance.Info.WindowPosition
                : new Rect(0, 0, 459, 120);

            WindowStyle = StyleFactory.MainWindowStyle;

            _machine = new KerbalFSM();

            InitMachine();

            //_launchSequenceWindow = AddComponent<LaunchSequenceWindow>();
            //_launchSequenceWindow.WindowStyle = StyleFactory.LaunchSequenceStyle;
            //_launchSequenceWindow.OnClosed += WindowOnClosed;

            //_settingsWindow = AddComponent<SettingsWindow>();
            //_settingsWindow.WindowStyle = StyleFactory.SettingsStyle;
            //_settingsWindow.OnClosed += WindowOnClosed;

            //_launcher = AddComponent<LaunchControl>();
            //_launcher.OnTick += _launcher_OnTick;
            //_launcher.OnVesselLaunched += _launcher_OnVesselLaunched;
            //_launcher.OnVesselAborted += _launcher_OnVesselAborted;

            //LaunchCountdownConfig.Instance.Info.OnChanged += Instance_OnChanged;
        }

        private void InitMachine()
        {
            var initState = new InitState("Init") { OnEnter = state => _stateName = "Init" };

            _machine.AddState(initState);
            _machine.StartFSM(_stateName);
        }

        public GUIStyle WindowStyle { get; set; }

        void Instance_OnChanged(object sender, ConfigEventArgs e)
        {
            if (e.Data != ConfigProperties.Scale) return;
            StyleFactory.Scale = LaunchCountdownConfig.Instance.Info.Scale;
            StyleFactory.Reload();
            WindowStyle = StyleFactory.MainWindowStyle;
        }

        void _launcher_OnVesselAborted(object sender, EventArgs e)
        {
            _tick = 0;
            _launchState = LaunchStates.Normal;
        }

        void _launcher_OnVesselLaunched(object sender, EventArgs e)
        {
            Visible = false;
            _launcherButton.Disable(false);
            _launcherButton.SetTexture(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Icons/launch_icon_disabled",
                false));
        }

        void _launcher_OnTick(object sender, LaunchEvenArgs e)
        {
            _tick = e.Tick;
        }

        void WindowOnClosed(object sender, EventArgs eventArgs)
        {
            _launchState = LaunchStates.Normal;
            Visible = true;
        }

        public void Update()
        {
            if (_machine.Started)
            {
                _machine.UpdateFSM();
            }
        }

        public void FixedUpdate()
        {
            if (_machine.Started)
            {
                _machine.FixedUpdateFSM();
            }
        }

        public void LateUpdate()
        {
            if (_machine.Started)
            {
                _machine.LateUpdateFSM();
            }
        }



        public void OnDestroy()
        {
            LaunchCountdownConfig.Instance.Info.WindowPosition = WindowRect;
            LaunchCountdownConfig.Instance.Info.Save();

            LaunchCountdownConfig.Instance.Info.OnChanged -= Instance_OnChanged;

            //_settingsWindow.OnClosed -= WindowOnClosed;
            //_launchSequenceWindow.OnClosed -= WindowOnClosed;

            _launcher.OnTick -= _launcher_OnTick;
            _launcher.OnVesselLaunched -= _launcher_OnVesselLaunched;
            _launcher.OnVesselAborted -= _launcher_OnVesselAborted;

            ApplicationLauncher.Instance.RemoveModApplication(_launcherButton);
        }
    }
}