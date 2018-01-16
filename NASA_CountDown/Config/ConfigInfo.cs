using System;
using System.Collections.Generic;
using System.Linq;
using NASA_CountDown.Helpers;
using UnityEngine;

namespace NASA_CountDown.Config
{
    class ConfigInfo: IConfigNode
    {
        private readonly RectWrapper _wrapper = new RectWrapper();

        private static ConfigInfo _config;

        private ConfigInfo(){}

        internal bool IsSoundEnabled { get; set; }

        internal string SoundSet { get; set; }

        internal Dictionary<Guid, int[]> Sequences { get; set; } = new Dictionary<Guid, int[]>();

        internal Rect WindowPosition { get; set; } = GUIUtil.ScreenCenteredRect(459, 120);

        internal bool EngineControl { get; set; } = true;

        internal bool AbortExecuted { get; set; } = true;

        internal float Scale { get; set; } = 1;

        internal bool IsLoaded { get; private set; }

        internal bool useGravityTurn { get; set; }  = false;

        internal static ConfigInfo Instance => _config ?? (_config = new ConfigInfo());

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
                {
                    if (FlightGlobals.ActiveVessel != null && !ConfigInfo.Instance.Sequences.ContainsKey(FlightGlobals.ActiveVessel.id))
                    {
                        Sequences.Add(FlightGlobals.ActiveVessel.id, Enumerable.Repeat(-1, 10).ToArray());
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

                var value = sequence.Value.Select(x => x.ToString()).Aggregate((x, y) => $"{x},{y}");

                seqNode.AddValue("stages", value);
            }
        }

        public Dictionary<string, AudioSet> AudioSets { get; } = new Dictionary<string, AudioSet>();

        public AudioSet CurrentAudio => AudioSets.ContainsKey(SoundSet) ? AudioSets[SoundSet] : null;
    }
}