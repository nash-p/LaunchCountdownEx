using System;
using System.Linq;
using KSP.UI;
using KSP.UI.Screens;
using NASA_CountDown.Config;
using NASA_CountDown.Helpers;
using NASA_CountDown.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

using ClickThroughFix;


namespace NASA_CountDown.States
{
    public class SequenceState : BaseGuiState
    {
        public Rect _windowRect = GUIUtil.ScreenCenteredRect(270, 400);
        private bool _isEditorState;
        private int _stageIndex;


        public SequenceState(string name, KerbalFsmEx machine) : base(name, machine)
        {
            _windowRect = CountDownMain.saveLoadWinPos.sequenceWindow;
            OnEnter = state =>
            {
                if (!ConfigInfo.Instance.Sequences.ContainsKey(FlightGlobals.ActiveVessel.id))
                {
                    ConfigInfo.Instance.Sequences.Add(FlightGlobals.ActiveVessel.id, Enumerable.Repeat(-1, 10).ToArray());
                }

                StageManager.Instance.Stages.ForEach(@group => group.Icons.ForEach(icon => icon.radioButton.onClick.AddListener(OnClickButton)));
            };

            OnLeave = state =>
            {
                StageManager.Instance.Stages.ForEach(@group => group.Icons.ForEach(icon => icon.radioButton.onClick.RemoveListener(OnClickButton)));
            };

            updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
        }

        private void OnClickButton(PointerEventData arg0, UIRadioButton.State arg1, UIRadioButton.CallType arg2)
        {
            if (!_isEditorState) return;

            var stage = arg0.pointerPress.GetComponentInParent<StageGroup>();

            if (stage != null)
            {
                ConfigInfo.Instance.Sequences[FlightGlobals.ActiveVessel.id][_stageIndex] = stage.inverseStageIndex;
                _isEditorState = false;
            }
        }

        public override void Draw()
        {
            _windowRect = KSPUtil.ClampRectToScreen(ClickThruBlocker.GUIWindow(99, _windowRect, DrawSequenceWindow, "", StyleFactory.LaunchSequenceStyle));
            GUI.BringWindowToFront(99);
        }

        private void DrawSequenceWindow(int id)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            ConfigInfo.Instance.EngineControl = GUILayout.Toggle(ConfigInfo.Instance.EngineControl, "Engine control", StyleFactory.ToggleStyle);

            if (GravityTurnAPI.GTAvailable)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Use Gravity Turn", StyleFactory.LabelStyle);
                GUILayout.FlexibleSpace();
                ConfigInfo.Instance.useGravityTurn = GUILayout.Toggle(ConfigInfo.Instance.useGravityTurn, "");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Elapsed time", StyleFactory.LabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Stage number", StyleFactory.LabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();



            for (int i = 0; i < 10; i++)
            {

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();


                GUILayout.Label($"{i + 1} second", StyleFactory.LabelStyle);
                GUILayout.FlexibleSpace();
                GUILayout.Label(ConfigInfo.Instance.Sequences[FlightGlobals.ActiveVessel.id][i] < 0 ? "none" : ConfigInfo.Instance.Sequences[FlightGlobals.ActiveVessel.id][i].ToString(), StyleFactory.LabelStyle, GUILayout.MinWidth(40));

                bool flag = _isEditorState && i != _stageIndex;
                string label = _isEditorState && i == _stageIndex ? "Done" : "Set";

                GUI.enabled = !flag;

                if (GUILayout.Button(label))
                {
                    _isEditorState = !_isEditorState;
                    _stageIndex = i;
                }
                GUI.enabled = true;

                if (GUILayout.Button("Clear"))
                {
                    _isEditorState = false;
                    _stageIndex = -1;
                    ConfigInfo.Instance.Sequences[FlightGlobals.ActiveVessel.id][i] = -1;
                }


                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent(), StyleFactory.ButtonBackStyle))
            {
                Machine.RunEvent("Init");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUI.DragWindow();
        }
    }
}
