using System;
using LaunchCountDown.Common;
using LaunchCountDown.Config;
using PluginFramework;
using UnityEngine;

namespace LaunchCountDown.Windows
{
    [WindowInitials(Caption = "", ClampToScreen = true, DragEnabled = true)]
    public class LaunchSequenceWindow : MonoBehaviorWindowExtended
    {
        private Guid _vesselId;

        protected override void Start()
        {
            base.Start();

            _vesselId = FlightGlobals.ActiveVessel.id;

            DebugHelper.WriteMessage("Vessel created {0}", _vesselId);

            if (!LaunchCountdownConfig.Instance.Info.Sequences.ContainsKey(_vesselId))
            {
                LaunchCountdownConfig.Instance.Info.Sequences.Add(_vesselId, new[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty });
            }
        }

        public override void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //GUILayout.Label(_vesselId, StyleFactory.LabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            LaunchCountdownConfig.Instance.Info.EngineControl = GUILayout.Toggle(LaunchCountdownConfig.Instance.Info.EngineControl, "Engine control", StyleFactory.ToggleStyle);

            SendMessage("SetEngineControl", LaunchCountdownConfig.Instance.Info.EngineControl);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Elapsed time", StyleFactory.LabelStyle);
            GUILayout.Label("Stage number", StyleFactory.LabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            for (int i = 0; i < 10; i++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                GUILayout.Label(string.Format("{0} seconds", i + 1), StyleFactory.LabelStyle);

                GUILayout.FlexibleSpace();

                int buff;

                if (!int.TryParse(LaunchCountdownConfig.Instance.Info.Sequences[_vesselId][i], out buff) || Staging.StageCount - 1 < buff || buff < 0)
                {
                    LaunchCountdownConfig.Instance.Info.Sequences[_vesselId][i] = GUILayout.TextField(LaunchCountdownConfig.Instance.Info.Sequences[_vesselId][i], StyleFactory.ErrorTextBoxStyle);
                }
                else
                {
                    LaunchCountdownConfig.Instance.Info.Sequences[_vesselId][i] = GUILayout.TextField(LaunchCountdownConfig.Instance.Info.Sequences[_vesselId][i], StyleFactory.TextBoxStyle);
                }

                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();
            }

            if (
                GUI.Button(
                    new Rect((StyleFactory.LaunchSequenceStyle.fixedWidth - StyleFactory.ButtonBackStyle.fixedWidth) / 2,
                        StyleFactory.LaunchSequenceStyle.fixedHeight - StyleFactory.ButtonBackStyle.fixedHeight - 10,
                        StyleFactory.ButtonBackStyle.fixedWidth, StyleFactory.ButtonBackStyle.fixedHeight), string.Empty,
                    StyleFactory.ButtonBackStyle))
            {
                this.Visible = false;

                Close();
            }

            GUILayout.EndVertical();
        }

    }
}
