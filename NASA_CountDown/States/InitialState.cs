using System;
using System.Collections;
using NASA_CountDown.Config;
using NASA_CountDown.Helpers;
using NASA_CountDown.StateMachine;
using UnityEngine;

namespace NASA_CountDown.States
{
    public class InitialState : BaseGuiState
    {
        public Rect _windowRect;
        protected int _tick = 0;
        protected float _delta;
        private bool _buttonOpened = false;
        protected GameObject _obj;

        protected DummyComponent _dummy;


        public InitialState(string name, KerbalFsmEx machine) : base(name, machine)
        {
            OnEnter = OnEnterToState;
            OnLeave = OnLeaveFromState;
        }

        protected virtual void OnLeaveFromState(KFSMState kfsmState)
        {
            if (_dummy == null)
                return;
            _dummy.StopAllCoroutines();
            _obj.DestroyGameObjectImmediate();
           
            FlightInputHandler.state.mainThrottle = HighLogic.CurrentGame.Parameters.CustomParams<NC>().defaultThrottle;
        }

        //bool initialPos = false;
        protected virtual void OnEnterToState(KFSMState kfsmState)
        {
            if (FlightGlobals.ActiveVessel == null)
                return;
            if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
            {
                Machine.RunEvent("Finish");
            }

            _obj = new GameObject("Helper");
            _dummy = _obj.AddComponent<DummyComponent>();
            StyleFactory.Scale = ConfigInfo.Instance.Scale;
            StyleFactory.Reload();

#if false
            if (!initialPos)
            {
                initialPos = true;
                _windowRect = ScaleRect(GUIUtil.ScreenCenteredRect(459, 120));
            }
#else
            _windowRect = CountDownMain.saveLoadWinPos.initialWindow;
            //    CountDownMain.saveLoadWinPos.initialWindow;
            //if (!initialPos)
            //{
            //    initialPos = true;
            //    _windowRect = ScaleRect(_windowRect);
            //}
#endif            
        }

        public override void Draw()
        {
            if (_dummy == null)
                return;
          
            _windowRect = KSPUtil.ClampRectToScreen(GUI.Window(99, _windowRect, DrawMainWindow, "", StyleFactory.MainWindowStyle));

            ConfigInfo.Instance.WindowPosition = _windowRect;
            if (!HighLogic.CurrentGame.Parameters.CustomParams<NC>().keepButtonsVisible)
            {
                if (Event.current.type == EventType.Repaint)
                {

                    if (_buttonOpened)
                    {
                        var openedRect = new Rect(_windowRect.xMin, _windowRect.yMin,
                            _windowRect.width, _windowRect.height + 29);

                        _dummy.StartCoroutine(
                            openedRect.Contains(new Vector2(Input.mousePosition.x,
                                Screen.height - Input.mousePosition.y))
                                ? ShowBottomButton()
                                : HideBottomButton());

                    }
                    else
                    {
                        _dummy.StartCoroutine(
                            _windowRect.Contains(new Vector2(Input.mousePosition.x,
                                Screen.height - Input.mousePosition.y))
                                ? ShowBottomButton()
                                : HideBottomButton());
                    }
                }

            }
            else
            {
                _buttonOpened = true;
                _delta = 0;
            }

            DrawButtons();

            GUI.BringWindowToFront(99);
        }

        private void DrawMainWindow(int id)
        {
            GUILayout.BeginHorizontal();

            GUI.DrawTexture(ScaleRect(new Rect(13, 41, 25, 27)), StyleFactory.GetTexture("minus"));
            GUI.DrawTexture(ScaleRect(new Rect(45, 14, 54, 77)), StyleFactory.GetTexture("Digit0"));
            GUI.DrawTexture(ScaleRect(new Rect(98, 14, 54, 77)), StyleFactory.GetTexture("Digit0"));

            GUI.DrawTexture(ScaleRect(new Rect(166, 28, 16, 51)), StyleFactory.GetTexture("colon"));

            GUI.DrawTexture(ScaleRect(new Rect(190, 14, 54, 77)), StyleFactory.GetTexture("Digit0"));
            GUI.DrawTexture(ScaleRect(new Rect(247, 14, 54, 77)), StyleFactory.GetTexture("Digit0"));

            GUI.DrawTexture(ScaleRect(new Rect(316, 28, 16, 51)), StyleFactory.GetTexture("colon"));

            int firstDigit = _tick / 10;
            int secondDigit = _tick % 10;

            //seconds
            GUI.DrawTexture(ScaleRect(new Rect(342, 14, 54, 77)), StyleFactory.GetTexture($"Digit{firstDigit}"));
            GUI.DrawTexture(ScaleRect(new Rect(396, 14, 54, 77)), StyleFactory.GetTexture($"Digit{secondDigit}"));

            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }


        protected virtual void DrawButtons()
        {
            var buttonWidth = StyleFactory.ButtonLaunchStyle.fixedWidth;
            var buttonHeight = StyleFactory.ButtonLaunchStyle.fixedHeight;

            //launch
            if (GUI.Button(new Rect(_windowRect.xMin, _windowRect.yMax - _delta, buttonWidth, buttonHeight),
                string.Empty,
                StyleFactory.ButtonLaunchStyle))
            {
                CountDownMain.saveLoadWinPos.initialWindow = CountDownMain.instance.initial._windowRect;
                CountDownMain.saveLoadWinPos.SaveSettings();
                
                Machine.RunEvent("Launch");
            }

            //sequence
            if (
                GUI.Button(
                    new Rect(_windowRect.xMin + buttonWidth, _windowRect.yMax - _delta, buttonWidth, buttonHeight),
                    string.Empty,
                    StyleFactory.ButtonSequenceStyle))
            {
                CountDownMain.saveLoadWinPos.initialWindow = CountDownMain.instance.initial._windowRect;
                CountDownMain.saveLoadWinPos.SaveSettings();
                Machine.RunEvent("Sequence");
            }

            //settings
            if (
                GUI.Button(
                    new Rect(_windowRect.xMin + buttonWidth * 2, _windowRect.yMax - _delta, buttonWidth, buttonHeight),
                    string.Empty,
                    StyleFactory.ButtonSettingsStyle))
            {
                CountDownMain.saveLoadWinPos.initialWindow = CountDownMain.instance.initial._windowRect;
                CountDownMain.saveLoadWinPos.SaveSettings();
                Machine.RunEvent("Settings");
            }
        }

        private Rect ScaleRect(Rect r)
        {
            var factor = ConfigInfo.Instance.Scale;
            return new Rect(
                r.xMin * factor,
                r.yMin * factor,
                r.width * factor,
                r.height * factor
               );
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
    }
}