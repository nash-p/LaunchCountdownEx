﻿using UnityEngine;

namespace NASA_CountDown.Helpers
{
    public static class StyleFactory
    {
        static StyleFactory()
        {
            Scale = 1f;

            Reload();
        }

        public static void Reload()
        {
            MainWindowStyle = new GUIStyle()
            {
                fixedWidth = Mathf.RoundToInt(459f * Scale),
                fixedHeight = Mathf.RoundToInt(120f * Scale),
                normal = { background = GetTexture("BGFull") },
                active = { background = GetTexture("BGFull") },
                hover = { background = GetTexture("BGFull") }
            };

            LaunchSequenceStyle = new GUIStyle()
            {
                fixedWidth = 270f,
                fixedHeight = 400f,
                normal = { background = GetTexture("LaunchSeqBG") },
                active = { background = GetTexture("LaunchSeqBG") },
                hover = { background = GetTexture("LaunchSeqBG") },
            };

            SettingsStyle = new GUIStyle(LaunchSequenceStyle)
            {
                fixedWidth = 250f,
                fixedHeight = 270f,
            };

            ButtonLaunchStyle = new GUIStyle()
            {
                fixedWidth = Mathf.RoundToInt(153f * Scale),
                fixedHeight = Mathf.RoundToInt(29f * Scale),
                normal =
                {
                    background = GetTexture("ButtonLaunchNormal")
                },
                hover =
                {
                    background = GetTexture("ButtonLaunchHover")
                },
                active =
                {
                    background = GetTexture("ButtonLaunchPressed")
                },
            };

            ButtonSettingsStyle = new GUIStyle(ButtonLaunchStyle)
            {
                normal =
                {
                    background = GetTexture("ButtonSettingNormal")
                },
                hover =
                {
                    background = GetTexture("ButtonSettingHover")
                },
                active =
                {
                    background = GetTexture("ButtonSettingPressed")
                },
            };

            ButtonSequenceStyle = new GUIStyle(ButtonLaunchStyle)
            {
                normal =
                {
                    background = GetTexture("ButtonLaunchSeqNormal")
                },
                hover =
                {
                    background = GetTexture("ButtonLaunchSeqHover")
                },
                active =
                {
                    background =
                        GetTexture("ButtonLaunchSeqPressed")
                },
            };

            ButtonSoundBackStyle = new GUIStyle()
            {
                fixedHeight = 29,
                fixedWidth = 29,
                normal =
                {
                    background = GetTexture("ButtonArrowBackNormal")
                },
                hover =
                {
                    background = GetTexture("ButtonArrowBackHover")
                },
                active =
                {
                    background =
                        GetTexture("ButtonArrowBackPressed")
                },
            };

            ButtonSoundNextStyle = new GUIStyle()
            {
                fixedHeight = 29,
                fixedWidth = 29,
                normal =
                {
                    background = GetTexture("ButtonArrowForwardNormal")
                },
                hover =
                {
                    background = GetTexture("ButtonArrowForwardHover")
                },
                active =
                {
                    background =
                        GetTexture("ButtonArrowForwardPressed")
                },
            };

            ButtonAbortStyle = new GUIStyle()
            {
                stretchWidth = true,
                normal =
                {
                    background = GetTexture("ButtonAbortNormal")
                },
                hover =
                {
                    background = GetTexture("ButtonAbortHover")
                },
                active =
                {
                    background = GetTexture("ButtonAbortPressed")
                },
            };

            ButtonBackStyle = new GUIStyle()
            {
                normal =
                {
                    background = GetTexture("ButtonBackNormal")
                },
                hover = { background = GetTexture("ButtonBackHover") },
                active =
                {
                    background = GetTexture("ButtonBackPressed")
                },
                fixedHeight = 29,
                fixedWidth = 90
            };

            LabelStyle = new GUIStyle(HighLogic.Skin.label) { alignment = TextAnchor.MiddleCenter };

            ToggleStyle = new GUIStyle(HighLogic.Skin.toggle) { margin = new RectOffset(12, 12, 4, 4) };

            TextBoxStyle = new GUIStyle(HighLogic.Skin.textField) { fixedWidth = 50 };

            ErrorTextBoxStyle = new GUIStyle(HighLogic.Skin.textField)
            {
                normal = { textColor = Color.red },
                fixedWidth = 50
            };
        }


        public static GUIStyle ButtonLaunchStyle { get; private set; }

        public static GUIStyle ButtonSettingsStyle { get; private set; }

        public static GUIStyle ButtonSequenceStyle { get; private set; }

        public static GUIStyle ButtonSoundBackStyle { get; private set; }

        public static GUIStyle ButtonSoundNextStyle { get; private set; }

        public static GUIStyle ButtonAbortStyle { get; private set; }

        public static GUIStyle ButtonBackStyle { get; private set; }

        public static GUIStyle MainWindowStyle { get; private set; }

        public static GUIStyle LabelStyle { get; private set; }

        public static GUIStyle ToggleStyle { get; private set; }

        public static GUIStyle TextBoxStyle { get; private set; }

        public static GUIStyle ErrorTextBoxStyle { get; private set; }

        public static GUIStyle LaunchSequenceStyle { get; private set; }

        public static GUIStyle SettingsStyle { get; private set; }

        public static float Scale { get; set; }

        public static Texture2D GetTexture(string name)
        {
            return GameDatabase.Instance.GetTexture($"NASA_Countdown/Images/{name}", false);
        }
    }
}