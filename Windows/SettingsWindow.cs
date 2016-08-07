using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using KspHelper.Behavior;
using LaunchCountDown.Common;
using LaunchCountDown.Config;
using UnityEngine;

namespace LaunchCountDown.Windows
{
    public class SettingsWindow : KspBehavior
    {
        private int _audioSet;
        private List<string> _soundsList = new List<string>();

        protected override void Awake()
        {
            var parent = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            if (Directory.Exists(Path.Combine(parent.FullName, "Sounds")))
            {
                _soundsList = Directory.GetDirectories(Path.Combine(parent.FullName, "Sounds")).Select(x => new DirectoryInfo(x)).Select(x => x.Name).ToList();
                if (_soundsList.Any() && !_soundsList.Contains(LaunchCountdownConfig.Instance.Info.SoundSet))
                {
                    LaunchCountdownConfig.Instance.Info.SoundSet = _soundsList.First();
                }
            }
            else
            {
                LaunchCountdownConfig.Instance.Info.IsSoundEnabled = false;
            }
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            LaunchCountdownConfig.Instance.Info.IsDebug = GUILayout.Toggle(LaunchCountdownConfig.Instance.Info.IsDebug, "Debug Mode", StyleFactory.ToggleStyle);

            LaunchCountdownConfig.Instance.Info.AbortExecuted = GUILayout.Toggle(LaunchCountdownConfig.Instance.Info.AbortExecuted, "Abort execute", StyleFactory.ToggleStyle);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format("Scale window {0}", Math.Round(LaunchCountdownConfig.Instance.Info.Scale, 2), StyleFactory.LabelStyle));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            LaunchCountdownConfig.Instance.Info.Scale = GUILayout.HorizontalSlider(LaunchCountdownConfig.Instance.Info.Scale,.2f, 1f, GUILayout.MaxWidth(160f), GUILayout.MinWidth(140f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            LaunchCountdownConfig.Instance.Info.IsSoundEnabled = GUILayout.Toggle(LaunchCountdownConfig.Instance.Info.IsSoundEnabled, "Sound enabled", StyleFactory.ToggleStyle);
            
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
                    LaunchCountdownConfig.Instance.Info.SoundSet = _soundsList[_audioSet];
                }

                GUILayout.FlexibleSpace();

                GUILayout.Label(LaunchCountdownConfig.Instance.Info.SoundSet, StyleFactory.LabelStyle);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("", StyleFactory.ButtonSoundNextStyle))
                {
                    _audioSet = _audioSet >= _soundsList.Count - 1 ? 0 : _audioSet + 1;
                    LaunchCountdownConfig.Instance.Info.SoundSet = _soundsList[_audioSet];
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

        private void Close()
        {
            
        }

        public bool Visible { get; set; }

        public Rect WindowRect { get; set; }
    }
}
