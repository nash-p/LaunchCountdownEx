using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LaunchCountDown.Common;
using LaunchCountDown.Config;
using PluginFramework;
using UnityEngine;

namespace LaunchCountDown.Windows
{
    [WindowInitials(Caption = "", ClampToScreen = true, DragEnabled = true)]
    public class SettingsWindow : MonoBehaviorWindowExtended
    {
        private int _audioSet;
        private List<string> _soundsList = new List<string>();

        public override void Awake()
        {
            var parent = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            if (Directory.Exists(Path.Combine(parent.FullName, "Sounds")))
            {
                _soundsList = Directory.GetDirectories(Path.Combine(parent.FullName, "Sounds")).Select(x => new DirectoryInfo(x)).Select(x => x.Name).ToList();
                if (_soundsList.Any() && !_soundsList.Contains(LaunchCountdownConfig.Instance.SoundSet))
                {
                    LaunchCountdownConfig.Instance.SoundSet = _soundsList.First();
                }
            }
            else
            {
                LaunchCountdownConfig.Instance.IsSoundEnabled = false;
            }
        }

        public override void DrawWindow(int id)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            LaunchCountdownConfig.Instance.IsDebug = GUILayout.Toggle(LaunchCountdownConfig.Instance.IsDebug, "Debug Mode", StyleFactory.ToggleStyle);

            LaunchCountdownConfig.Instance.AbortExecuted = GUILayout.Toggle(LaunchCountdownConfig.Instance.AbortExecuted, "Abort execute", StyleFactory.ToggleStyle);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format("Scale window {0}", Math.Round(LaunchCountdownConfig.Instance.Scale, 2), StyleFactory.LabelStyle));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            LaunchCountdownConfig.Instance.Scale = GUILayout.HorizontalSlider(LaunchCountdownConfig.Instance.Scale,.2f, 1f, GUILayout.MaxWidth(160f), GUILayout.MinWidth(140f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            LaunchCountdownConfig.Instance.IsSoundEnabled = GUILayout.Toggle(LaunchCountdownConfig.Instance.IsSoundEnabled, "Sound enabled", StyleFactory.ToggleStyle);
            
           if (_soundsList.Any())
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                
                GUILayout.Label("Sound set", StyleFactory.LabelStyle);

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("", StyleFactory.ButtonSoundBackStyle))
                {
                    _audioSet = _audioSet <= 0 ? _soundsList.Count - 1 : _audioSet - 1;
                    LaunchCountdownConfig.Instance.SoundSet = _soundsList[_audioSet];
                    DebugHelper.WriteMessage("Current sound {0}", _soundsList[_audioSet]);
                }

                GUILayout.FlexibleSpace();

                GUILayout.Label(LaunchCountdownConfig.Instance.SoundSet, StyleFactory.LabelStyle);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("", StyleFactory.ButtonSoundNextStyle))
                {
                    _audioSet = _audioSet >= _soundsList.Count - 1 ? 0 : _audioSet + 1;
                    LaunchCountdownConfig.Instance.SoundSet = _soundsList[_audioSet];
                    DebugHelper.WriteMessage("Current sound {0}", _soundsList[_audioSet]);
                }

                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                GUILayout.EndVertical(); 
            }

            if (
                GUI.Button(
                    new Rect((StyleFactory.SettingsStyle.fixedWidth - StyleFactory.ButtonBackStyle.fixedWidth) / 2,
                        StyleFactory.SettingsStyle.fixedHeight - StyleFactory.ButtonBackStyle.fixedHeight - 10,
                        StyleFactory.ButtonBackStyle.fixedWidth, StyleFactory.ButtonBackStyle.fixedHeight), string.Empty,
                    StyleFactory.ButtonBackStyle))
            {
                Visible = false;
                Close();
            }

            GUILayout.EndVertical();
        }
    }
}
