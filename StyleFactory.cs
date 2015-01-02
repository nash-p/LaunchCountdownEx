using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LaunchCountDown
{
    public static class StyleFactory
    {
        static StyleFactory()
        {
            MainWindowStyle = new GUIStyle()
            {
                fixedWidth = 459f,
                fixedHeight = 120f,
                normal = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/BGFull", false) },
                active = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/BGFull", false) },
                hover = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/BGFull", false) }
            };

            LaunchSequenceStyle = new GUIStyle()
            {
                fixedWidth = 160f,
                fixedHeight = 380f,
                normal = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/LaunchSeqBG", false) },
                active = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/LaunchSeqBG", false) },
                hover = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/LaunchSeqBG", false) },
            };

            SettingsStyle = new GUIStyle(LaunchSequenceStyle)
            {
                fixedWidth = 200f,
                fixedHeight = 170f,
            };

            ButtonLaunchStyle = new GUIStyle()
            {
                stretchWidth = true,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonLaunchNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonLaunchHover", false)
                },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonLaunchPressed", false)
                },
            };

            ButtonSettingsStyle = new GUIStyle()
            {
                stretchWidth = true,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonSettingNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonSettingHover", false)
                },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonSettingPressed", false)
                },
            };

            ButtonSequenceStyle = new GUIStyle()
            {
                stretchWidth = true,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonLaunchSeqNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonLaunchSeqHover", false)
                },
                active =
                {
                    background =
                        GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonLaunchSeqPressed", false)
                },
            };

            ButtonAbortStyle = new GUIStyle()
            {
                stretchWidth = true,
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonAbortNormal", false)
                },
                hover =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonAbortHover", false)
                },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonAbortPressed", false)
                },
            };

            ButtonBackStyle = new GUIStyle()
            {
                normal =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonBackNormal", false)
                },
                hover = { background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonBackHover", false) },
                active =
                {
                    background = GameDatabase.Instance.GetTexture("LaunchCountDown_Ex/Images/ButtonBackPressed", false)
                },
                fixedHeight = 29,
                fixedWidth = 90
            };

            LabelStyle = new GUIStyle(HighLogic.Skin.label) { alignment = TextAnchor.MiddleCenter };

            ToggleStyle = new GUIStyle(HighLogic.Skin.toggle) { margin = new RectOffset(12, 12, 4, 4) };

            TextBoxStyle = new GUIStyle(HighLogic.Skin.textField) { fixedWidth = 50 };
            
            ErrorTextBoxStyle = new GUIStyle(HighLogic.Skin.textField) { normal = { textColor = Color.red }, fixedWidth = 50 };
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

    }
}
