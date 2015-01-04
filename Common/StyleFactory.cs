using System;
using LaunchCountDown.Config;
using UnityEngine;

namespace LaunchCountDown.Common
{
    public static class StyleFactory
    {
        static StyleFactory()
        {
            Scale = LaunchCountdownConfig.Instance.Info.IsLoaded ? LaunchCountdownConfig.Instance.Info.Scale : 1f;

            Reload();
        }

        public static void Reload()
        {
            MainWindowStyle = new GUIStyle()
            {
                fixedWidth = Mathf.RoundToInt(459f*Scale),
                fixedHeight = Mathf.RoundToInt(120f*Scale),
                normal = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/BGFull", false)},
                active = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/BGFull", false)},
                hover = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/BGFull", false)}
            };

            LaunchSequenceStyle = new GUIStyle()
            {
                fixedWidth = 160f,
                fixedHeight = 430f,
                normal = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/LaunchSeqBG", false)},
                active = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/LaunchSeqBG", false)},
                hover = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/LaunchSeqBG", false)},
            };

            SettingsStyle = new GUIStyle(LaunchSequenceStyle)
            {
                fixedWidth = 200f,
                fixedHeight = 290f,
            };

            ButtonLaunchStyle = new GUIStyle()
            {
                fixedWidth = Mathf.RoundToInt(153f*Scale),
                fixedHeight = Mathf.RoundToInt(29f*Scale),
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonLaunchNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonLaunchHover", false)
                },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonLaunchPressed", false)
                },
            };

            ButtonSettingsStyle = new GUIStyle(ButtonLaunchStyle)
            {
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonSettingNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonSettingHover", false)
                },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonSettingPressed", false)
                },
            };

            ButtonSequenceStyle = new GUIStyle(ButtonLaunchStyle)
            {
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonLaunchSeqNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonLaunchSeqHover", false)
                },
                active =
                {
                    background =
                        GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonLaunchSeqPressed", false)
                },
            };

            ButtonSoundBackStyle = new GUIStyle()
            {
                fixedHeight = 29,
                fixedWidth = 29,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonArrowBackNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonArrowBackHover", false)
                },
                active =
                {
                    background =
                        GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonArrowBackPressed", false)
                },
            };

            ButtonSoundNextStyle = new GUIStyle()
            {
                fixedHeight = 29,
                fixedWidth = 29,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonArrowForwardNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonArrowForwardHover", false)
                },
                active =
                {
                    background =
                        GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonArrowForwardPressed", false)
                },
            };

            ButtonAbortStyle = new GUIStyle()
            {
                stretchWidth = true,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonAbortNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonAbortHover", false)
                },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonAbortPressed", false)
                },
            };

            ButtonBackStyle = new GUIStyle()
            {
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonBackNormal", false)
                },
                hover = {background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonBackHover", false)},
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDownEx/Images/ButtonBackPressed", false)
                },
                fixedHeight = 29,
                fixedWidth = 90
            };

            LabelStyle = new GUIStyle(HighLogic.Skin.label) {alignment = TextAnchor.MiddleCenter};

            ToggleStyle = new GUIStyle(HighLogic.Skin.toggle) {margin = new RectOffset(12, 12, 4, 4)};

            TextBoxStyle = new GUIStyle(HighLogic.Skin.textField) {fixedWidth = 50};

            ErrorTextBoxStyle = new GUIStyle(HighLogic.Skin.textField) {normal = {textColor = Color.red}, fixedWidth = 50};
        }


        public static GUIStyle ButtonLaunchStyle
        {
            get;
            private set;
        }

        public static GUIStyle ButtonSettingsStyle
        {
            get;
            private set;
        }

        public static GUIStyle ButtonSequenceStyle
        {
            get;
            private set;
        }

        public static GUIStyle ButtonSoundBackStyle
        {
            get;
            private set;
        }
        public static GUIStyle ButtonSoundNextStyle
        {
            get;
            private set;
        }

        public static GUIStyle ButtonAbortStyle
        {
            get;
            private set;
        }

        public static GUIStyle ButtonBackStyle
        {
            get;
            private set;
        }

        public static GUIStyle MainWindowStyle
        {
            get;
            private set;
        }

        public static GUIStyle LabelStyle
        {
            get;
            private set;
        }

        public static GUIStyle ToggleStyle
        {
            get;
            private set;
        }

        public static GUIStyle TextBoxStyle
        {
            get;
            private set;
        }
        public static GUIStyle ErrorTextBoxStyle
        {
            get;
            private set;
        }

        public static GUIStyle LaunchSequenceStyle
        {
            get;
            private set;
        }

        public static GUIStyle SettingsStyle
        {
            get;
            private set;
        }

        public static float Scale { get; set; }

    }
}
