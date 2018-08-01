using System;
using System.Collections.Generic;
using System.Linq;
using NASA_CountDown.Helpers;
using UnityEngine;

namespace NASA_CountDown.Config
{
    class ConfigInfo //: IConfigNode
    {
        private readonly RectWrapper _wrapper = new RectWrapper();

        private static ConfigInfo _config;

        private ConfigInfo() { }

        internal bool IsSoundEnabled { get; set; }

        internal string SoundSet { get; set; }

        internal Dictionary<string, int[]> Sequences { get; set; } = new Dictionary<string, int[]>();

        internal Rect WindowPosition { get; set; } = GUIUtil.ScreenCenteredRect(459, 120);

        internal bool EngineControl { get; set; } = true;

        internal bool AbortExecuted { get; set; } = true;

        internal float Scale { get; set; } = 1;

        internal bool IsLoaded { get; private set; } = false;

        internal bool useGravityTurn { get; set; } = false;

        internal static ConfigInfo Instance => _config ?? (_config = new ConfigInfo());

        string SETTINGSNAME = "NASACountdown";
        string PLUGINDATA = KSPUtil.ApplicationRootPath + "GameData/NASA_CountDown/PluginData/NASACountdown.cfg";

        public void Save()
        {
            Log.Info("ConfigInfo.Save");
            ConfigNode settingsFile = new ConfigNode();
            ConfigNode settings = new ConfigNode();

            settingsFile.SetNode(SETTINGSNAME, settings, true);
            if (SaveLoadWinPos.Instance.sequenceWindow.width > 0)
                SaveLoadWinPos.Instance.SaveWinPos(settings, "sequenceWindow", SaveLoadWinPos.Instance.sequenceWindow);
            if (SaveLoadWinPos.Instance.initialWindow.width > 0)
                SaveLoadWinPos.Instance.SaveWinPos(settings, "initialWindow", SaveLoadWinPos.Instance.initialWindow);
            if (SaveLoadWinPos.Instance.settingsWindow.width > 0)
                SaveLoadWinPos.Instance.SaveWinPos(settings, "settingsWindow", SaveLoadWinPos.Instance.settingsWindow);

            settings.AddValue("soundEnabled", IsSoundEnabled);
            settings.AddValue("engineControl", EngineControl);
            settings.AddValue("scale", Scale);
            settings.AddValue("soundSet", SoundSet);
            settings.AddValue("useGravityTurn", useGravityTurn);

            foreach (var sequence in Sequences)
            {
                var seqNode = new ConfigNode("sequence");
                seqNode.AddValue("id", sequence.Key);
                string stages = "";
                for (int i = 0; i < sequence.Value.Length - 1; i++)
                    //if (sequence.Value[i] >= 0)
                    {
                        if (stages != "")
                            stages += ",";
                        stages += sequence.Value[i].ToString();
                    }

                if (stages != "")
                {
                    seqNode.AddValue("stages", stages);

                    settings.AddNode(seqNode);
                }
            }
            
            settingsFile.Save(PLUGINDATA);
        }

        public void Load()
        {
            LoadSounds();
            Log.Info("ConfigInfo.Load");
            ConfigNode settingsFile =  ConfigNode.Load(PLUGINDATA);
            if (settingsFile != null)
            {
                ConfigNode node = settingsFile.GetNode(SETTINGSNAME);
                if (node != null)
                {
                    if (node.HasValue("soundEnabled"))
                    {
                        IsSoundEnabled = bool.Parse(node.GetValue("soundEnabled"));
                    }

                    if (node.HasValue("useGravityTurn"))
                    {
                        useGravityTurn = bool.Parse(node.GetValue("useGravityTurn"));
                    }

                    if (node.HasValue("engineControl"))
                    {
                        EngineControl = bool.Parse(node.GetValue("engineControl"));
                    }

                    if (node.HasValue("abort"))
                    {
                        AbortExecuted = bool.Parse(node.GetValue("abort"));
                    }

                    if (node.HasValue("scale"))
                    {
                        Scale = float.Parse(node.GetValue("scale"));
                    }

                    if (node.HasValue("soundSet"))
                    {
                        SoundSet = node.GetValue("soundSet");
                    }

                    if (node.HasValue("position"))
                    {
                        WindowPosition = _wrapper.ToRect(node.GetValue("position"));
                        Debug.LogWarning("Position is" + WindowPosition);
                    }
#if true
                if (node.HasNode("sequence"))
                {
                    var sequences = node.GetNodes("sequence");

                    Sequences.Clear();

                    foreach (var sequence in sequences)
                    {
                            //Sequences.Add(new Guid(sequence.GetValue("id")), sequence.GetValue("stages").Split(',').Select(int.Parse).ToArray()); Sequences.Add(new Guid(sequence.GetValue("id")), sequence.GetValue("stages").Split(',').Select(int.Parse).ToArray());
                            int[] ar = Enumerable.Repeat(-1, 10).ToArray();
                            var d = sequence.GetValue("stages").Split(',').Select(int.Parse).ToArray();
                            for (int i = 0; i < d.Length; i++)
                            {
                                //Log.Info("Load, i: " + i + ", " + d[i]);
                                ar[i] = d[i];
                            }
                            Sequences.Add(sequence.GetValue("id"), ar);
                        }
                    }
                else
#endif
                    {
                        Sequences.Clear();
                        if (FlightGlobals.ActiveVessel != null && !ConfigInfo.Instance.Sequences.ContainsKey(ModuleNASACountdown.CraftName(FlightGlobals.ActiveVessel)))
                        {
                            Sequences.Add(ModuleNASACountdown.CraftName(FlightGlobals.ActiveVessel), Enumerable.Repeat(-1, 10).ToArray());
                        }
                    }

                    IsLoaded = true;
                }
            }


        }
#if false
        public void Load(ConfigNode node)
        {
            try
            {
                LoadSounds();

                if (node == null) throw new NullReferenceException("Node not exist");

                if (node.HasValue("soundEnabled"))
                {
                    IsSoundEnabled = bool.Parse(node.GetValue("soundEnabled"));
                }

                if (node.HasValue("engineControl"))
                {
                    EngineControl = bool.Parse(node.GetValue("engineControl"));
                }

                if (node.HasValue("abort"))
                {
                    AbortExecuted = bool.Parse(node.GetValue("abort"));
                }

                if (node.HasValue("scale"))
                {
                    Scale = float.Parse(node.GetValue("scale"));
                }

                if (node.HasValue("soundSet"))
                {
                    SoundSet = node.GetValue("soundSet");
                }

                if (node.HasValue("position"))
                {
                    WindowPosition = _wrapper.ToRect(node.GetValue("position"));
                    Debug.LogWarning("Position is" + WindowPosition);
                }
#if false
                if (node.HasNode("sequence"))
                {
                    var sequences = node.GetNodes("sequence");

                    Sequences.Clear();

                    foreach (var sequence in sequences)
                    {
                        Sequences.Add(new Guid(sequence.GetValue("id")), sequence.GetValue("stages").Split(',').Select(int.Parse).ToArray());
                    }
                }
                else
#endif
                {
                    if (FlightGlobals.ActiveVessel != null && !ConfigInfo.Instance.Sequences.ContainsKey(ModuleNASACountdown.CraftName(FlightGlobals.ActiveVessel)))
                    {
                        Sequences.Add(ModuleNASACountdown.CraftName(FlightGlobals.ActiveVessel), Enumerable.Repeat(-1, 10).ToArray());
                    }
                }

                IsLoaded = true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Cannot load config");
                Debug.LogException(ex);
                IsLoaded = false;
            }
        }
#endif

        private void LoadSounds()
        {
            var soundsList =
                GameDatabase.Instance.databaseAudio.Where(x => x.name.StartsWith("NASA_CountDown", StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.name)
                    .ToList();

            foreach (var name in soundsList.Select(x => x.Split('/')).Select(m => m[2]).Distinct())
            {
                if (!AudioSets.ContainsKey(name))
                    AudioSets.Add(name, new AudioSet(name));
            }
        }

        public void Save(ConfigNode node)
        {
            node.AddValue("soundEnabled", IsSoundEnabled);
            node.AddValue("soundSet", SoundSet);

            _wrapper.FromRect(WindowPosition);

            node.AddValue("position", _wrapper);
            node.AddValue("engineControl", EngineControl);
            node.AddValue("abort", AbortExecuted);
            node.AddValue("scale", Scale);

            foreach (var sequence in Sequences)
            {
                var seqNode = node.AddNode("sequence");

                seqNode.AddValue("id", sequence.Key);
                
                string value = sequence.Value.Select(x => x.ToString()).Aggregate((x, y) => $"{x},{y}");

                seqNode.AddValue("stages", value);
            }
        }

        public Dictionary<string, AudioSet> AudioSets { get; } = new Dictionary<string, AudioSet>();

        public AudioSet CurrentAudio  {
            get
            {
                if (!ConfigInfo.Instance.IsSoundEnabled)
                    return null;
                return AudioSets.ContainsKey(SoundSet) ? AudioSets[SoundSet] : null;
            }
        }
    }
}