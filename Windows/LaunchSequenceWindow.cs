using LaunchCountDown.Common;
using LaunchCountDown.Config;
using PluginFramework;
using UnityEngine;

namespace LaunchCountDown.Windows
{
    [WindowInitials(Caption = "", ClampToScreen = true, DragEnabled = true)]
    public class LaunchSequenceWindow : MonoBehaviorWindowExtended
    {
        private string _vesselName;

        public override void Awake()
        {
            GameEvents.onVesselCreate.Add(VesselCreated);
            _vesselName = "default";
        }

        private void VesselCreated(Vessel data)
        {
            _vesselName = data.vesselName;

            DebugHelper.WriteMessage("Vessel created {0}", _vesselName);

            if (!LaunchCountdownConfig.Instance.Sequences.ContainsKey(_vesselName))
            {
                LaunchCountdownConfig.Instance.Sequences.Add(_vesselName, new[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty });
            }

            GameEvents.onVesselCreate.Remove(VesselCreated);
        }

        public override void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(_vesselName, StyleFactory.LabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            LaunchCountdownConfig.Instance.EngineControl = GUILayout.Toggle(LaunchCountdownConfig.Instance.EngineControl, "Engine control", StyleFactory.ToggleStyle);

            SendMessage("SetEngineControl", LaunchCountdownConfig.Instance.EngineControl);

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

                if (!int.TryParse(LaunchCountdownConfig.Instance.Sequences[_vesselName][i], out buff) || Staging.StageCount - 1 < buff || buff < 0)
                {
                    LaunchCountdownConfig.Instance.Sequences[_vesselName][i] = GUILayout.TextField(LaunchCountdownConfig.Instance.Sequences[_vesselName][i], StyleFactory.ErrorTextBoxStyle);
                }
                else
                {
                    LaunchCountdownConfig.Instance.Sequences[_vesselName][i] = GUILayout.TextField(LaunchCountdownConfig.Instance.Sequences[_vesselName][i], StyleFactory.TextBoxStyle);
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


        public override void OnDestroy()
        {
            GameEvents.onVesselCreate.Remove(VesselCreated);

            base.OnDestroy();
        }
    }
}
