using System;
using System.Collections;
using LaunchCountDown.Common;
using LaunchCountDown.Config;
using LaunchCountDown.Windows;
using UnityEngine;

namespace LaunchCountDown.States
{
    public class InitState : GuiState
    {
        private int _tick = 0;
        private LaunchStates _launchState = LaunchStates.Normal;
        private float _delta;
        private bool _buttonOpened;

        public InitState(string name) : base(name)
        {
            DrawCallback = () =>
            {
                WindowRect = KSPUtil.ClampRectToScreen(GUI.Window(99, WindowRect, Draw, new GUIContent(), StyleFactory.MainWindowStyle));
                GUI.BringWindowToFront(99);
            };
            OnUpdate = Update;
            updateMode = KFSMUpdateMode.UPDATE;
            WindowRect = LaunchCountdownConfig.Instance.Info.IsLoaded
                ? LaunchCountdownConfig.Instance.Info.WindowPosition
                : new Rect(0, 0, 459, 120);
        }

        private void Update()
        {
            if (_buttonOpened)
            {
                var openedRect = new Rect(WindowRect.xMin, WindowRect.yMin,
                    WindowRect.width, WindowRect.height + 29);
                CountDownScenario.Instance.RunCoroutine(openedRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) ? ShowBottomButton() : HideBottomButton());
            }
            else
            {
                CountDownScenario.Instance.RunCoroutine(WindowRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) ? ShowBottomButton() : HideBottomButton());
            }

        }

        public Rect WindowRect { get; private set; }

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

        private void Draw(int id)
        {
            GUILayout.BeginHorizontal();

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

            OnGUIEvery();

            GUI.DragWindow();
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
                        //_launcher.Run();
                        _launchState = LaunchStates.Launch;
                    }

                    //sequence
                    if (
                        GUI.Button(
                            new Rect(WindowRect.xMin + buttonWidth, WindowRect.yMax - _delta, buttonWidth,
                                buttonHeight),
                            string.Empty,
                            StyleFactory.ButtonSequenceStyle))
                    {
                       
                        //_launchSequenceWindow.WindowRect.x = WindowRect.x;
                        //_launchSequenceWindow.WindowRect.y = WindowRect.y;
                        //_launchSequenceWindow.Visible = true;
                        _launchState = LaunchStates.Sequence;
                    }

                    //settings
                    if (
                        GUI.Button(
                            new Rect(WindowRect.xMin + buttonWidth * 2, WindowRect.yMax - _delta, buttonWidth,
                                buttonHeight),
                            string.Empty,
                            StyleFactory.ButtonSettingsStyle))
                    {
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
                        //_launcher.Abort();
                        _launchState = LaunchStates.Normal;
                        _tick = 0;
                    }
                    TimeWarp.SetRate(0, false);
                    break;
                case LaunchStates.Abort:
                case LaunchStates.Settings:
                case LaunchStates.Fly:
                    break;
            }
        }

        private IEnumerator ShowBottomButton()
        {
            while (_delta > 0)
            {
                _delta--;
                yield return null;
            }

            _buttonOpened = true;
        }

        private IEnumerator HideBottomButton()
        {
            while (_delta < 34)
            {
                _delta++;
                yield return null;
            }

            _buttonOpened = false;
        }
    }
}