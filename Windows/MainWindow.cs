using System;
using System.Collections;
using LaunchCountDown.Common;
using LaunchCountDown.Config;
using LaunchCountDown.States;
using UnityEngine;

namespace LaunchCountDown.Windows
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public partial class MainWindow : ScenarioModule
    {
        private int _tick;
        private LaunchStates _launchState = LaunchStates.Normal;
        private float _delta = 0;
        private bool _buttonOpened;
        private LaunchControl _launcher;
        private ApplicationLauncherButton _launcherButton;

        private KerbalFSM _machine;
        private string _stateName = "Init";


        private Rect ScaleRect(Rect r)
        {
            var factor = LaunchCountdownConfig.Instance.Info.Scale;
            return new Rect(
                r.xMin * factor,
                r.yMin * factor,
                r.width * factor,
                r.height * factor
               );
        }

        private void DrawMainWindow(int id)
        {
            GUILayout.BeginHorizontal();

            //GUILayout.FlexibleSpace();

            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/minus", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/colon", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/colon", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            //GUILayout.Label(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));

            //GUILayout.FlexibleSpace();

            GUI.DrawTexture(ScaleRect(new Rect(13, 41, 25, 27)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/minus", false));
            GUI.DrawTexture(ScaleRect(new Rect(45, 14, 54, 77)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            GUI.DrawTexture(ScaleRect(new Rect(98, 14, 54, 77)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));

            GUI.DrawTexture(ScaleRect(new Rect(166, 28, 16, 51)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/colon", false));

            GUI.DrawTexture(ScaleRect(new Rect(190, 14, 54, 77)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));
            GUI.DrawTexture(ScaleRect(new Rect(247, 14, 54, 77)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/Digit0", false));

            GUI.DrawTexture(ScaleRect(new Rect(316, 28, 16, 51)),
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/colon", false));

            int firstDigit = _tick / 10;
            int secondDigit = _tick % 10;

            //seconds
            GUI.DrawTexture(ScaleRect(new Rect(342, 14, 54, 77)),
                GameDatabase.Instance.GetTexture($"LaunchCountDownEx/Images/Digit{firstDigit}", false));
            GUI.DrawTexture(ScaleRect(new Rect(396, 14, 54, 77)),
                GameDatabase.Instance.GetTexture($"LaunchCountDownEx/Images/Digit{secondDigit}", false));

            GUILayout.EndHorizontal();

            GUI.DragWindow();

            //if (Event.current.type == EventType.Repaint)
            //{
            //    if (this.WindowRect.Contains(new Vector3(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y, Input.mousePosition.z)))
            //    {
            //        if (_buttonOpened)
            //        {
            //            var openedRect = new Rect(WindowRect.xMin, WindowRect.yMin,
            //                WindowRect.width, WindowRect.height + 29);
            //            StartCoroutine(
            //                openedRect.Contains(new Vector2(Input.mousePosition.x,
            //                    Screen.height - Input.mousePosition.y))
            //                    ? ShowBottomButton()
            //                    : HideBottomButton());
            //        }
            //        else
            //        {
            //            StartCoroutine(
            //                WindowRect.Contains(new Vector2(Input.mousePosition.x,
            //                    Screen.height - Input.mousePosition.y))
            //                    ? ShowBottomButton()
            //                    : HideBottomButton());
            //        }
            //    }
            //}
        }

        private void OnGUIEvery()
        {
            var buttonWidth = StyleFactory.ButtonLaunchStyle.fixedWidth;
            var buttonHeight = StyleFactory.ButtonLaunchStyle.fixedHeight;


            switch (_launchState)
            {
                case LaunchStates.Normal:
                    //launch
                    if (GUI.Button(new Rect(WindowRect.xMin, WindowRect.yMax - _delta, buttonWidth, buttonHeight),
                        string.Empty,
                        StyleFactory.ButtonLaunchStyle))
                    {
                        _launcher.Run();
                        _launchState = LaunchStates.Launch;
                    }

                    //sequence
                    if (
                        GUI.Button(
                            new Rect(WindowRect.xMin + buttonWidth, WindowRect.yMax - _delta, buttonWidth, buttonHeight),
                            string.Empty,
                            StyleFactory.ButtonSequenceStyle))
                    {
                        Visible = false;
                        //_launchSequenceWindow.WindowRect.x = WindowRect.x;
                        //_launchSequenceWindow.WindowRect.y = WindowRect.y;
                        //_launchSequenceWindow.Visible = true;
                        _launchState = LaunchStates.Sequence;
                    }

                    //settings
                    if (
                        GUI.Button(
                            new Rect(WindowRect.xMin + buttonWidth * 2, WindowRect.yMax - _delta, buttonWidth, buttonHeight),
                            string.Empty,
                            StyleFactory.ButtonSettingsStyle))
                    {
                        Visible = false;
                        //_settingsWindow.WindowRect.x = WindowRect.x;
                        //_settingsWindow.WindowRect.y = WindowRect.y;
                        //_settingsWindow.Visible = true;
                        _launchState = LaunchStates.Settings;
                    }

                    break;
                case LaunchStates.Launch:
                    //abort
                    if (GUI.Button(new Rect(WindowRect.xMin, WindowRect.yMax - _delta, buttonWidth, buttonHeight),
                        string.Empty,
                        StyleFactory.ButtonAbortStyle))
                    {
                        _launcher.Abort();
                        _launchState = LaunchStates.Normal;
                        _tick = 0;
                    }
                    TimeWarp.SetRate(0, false);
                    break;
                case LaunchStates.Abort:
                case LaunchStates.Settings:
                case LaunchStates.Fly:
                    DebugHelper.WriteMessage("Current launch state {0}", _launchState);
                    break;
            }
        }

        public override void OnAwake()
        {
            Visible = true;

            _machine = new KerbalFSM();

            MachineInit();

            _launcherButton = ApplicationLauncher.Instance.AddModApplication(() => Visible = true, () => Visible = false, () => { }, () => { },
                () => { }, () => { }, ApplicationLauncher.AppScenes.FLIGHT,
                GameDatabase.Instance.GetTexture("LaunchCountDownEx/Icons/launch_icon_normal", false));

            WindowRect = LaunchCountdownConfig.Instance.Info.IsLoaded ? LaunchCountdownConfig.Instance.Info.WindowPosition : GUIUtil.ScreenCenteredRect(459, 120);

            WindowStyle = StyleFactory.MainWindowStyle;

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

        private void MachineInit()
        {
            var initState = new KspGuiState("Init")
            {
                OnEnter = state => Visible = true,
                Draw = () =>
                {
                    WindowRect = KSPUtil.ClampRectToScreen(GUI.Window(99, WindowRect, DrawMainWindow, "", StyleFactory.MainWindowStyle));

                    OnGUIEvery();

                    GUI.BringWindowToFront(99);
                }
            };


            _machine.AddState(initState);

            _machine.StartFSM(_stateName);
        }

        public GUIStyle WindowStyle { get; set; }

        public Rect WindowRect { get; set; }

        public bool Visible { get; set; }

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
            _launcherButton.SetTexture(GameDatabase.Instance.GetTexture("LaunchCountDownEx/Icons/launch_icon_disabled", false));
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

        public void OnGUI()
        {
            if (_machine.Started && Visible)
            {
                (_machine.CurrentState as KspGuiState)?.Draw();
            }

            var buttonWidth = StyleFactory.ButtonLaunchStyle.fixedWidth;
            var buttonHeight = StyleFactory.ButtonLaunchStyle.fixedHeight;

            GUI.Button(new Rect(10, 10, buttonWidth, buttonHeight), string.Empty, StyleFactory.ButtonLaunchStyle);
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


        public void Update()
        {
            if (_machine.Started)
            {
                _machine.UpdateFSM();
            }
        }

        private IEnumerator ShowBottomButton()
        {
            while (_delta > 0)
            {
                _delta--;
                yield return new WaitForEndOfFrame();
            }

            _buttonOpened = true;
        }

        private IEnumerator HideBottomButton()
        {
            while (_delta < 34)
            {
                _delta++;
                yield return new WaitForEndOfFrame();
            }

            _buttonOpened = false;
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
