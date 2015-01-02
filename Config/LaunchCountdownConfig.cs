using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LaunchCountDown.Common;
using PluginFramework;
using UnityEngine;

namespace LaunchCountDown.Config
{
    class LaunchCountdownConfig : ConfigNodeStorage
    {
        private static LaunchCountdownConfig _config;

        [Persistent]
        private bool _isDebug;

        [Persistent]
        private bool _isSoundEnabled;

        [Persistent]
        private string _soundSet;

        [Persistent]
        private bool _engineControl;

        [Persistent]
        private bool _abortExecuted;
        
        [Persistent]
        private float _scale = 1f;

        private Dictionary<string, string[]> _sequences;

        [Persistent]
        private List<SequenceStorage> _storages = new List<SequenceStorage>();

        protected LaunchCountdownConfig()
            : base(@"config\lcdex.cfg")
        {
            var current_dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!Directory.Exists(Path.Combine(current_dir, "config")))
            {
                Directory.CreateDirectory(Path.Combine(current_dir, "config"));
            }

            _sequences = new Dictionary<string, string[]>
            {
                {"default", new[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty }}
            };
        }

        internal static LaunchCountdownConfig Instance
        {
            get { return _config ?? (_config = new LaunchCountdownConfig()); }
        }

        internal bool IsDebug
        {
            get { return _isDebug; }
            set
            {
                if (_isDebug == value) return;
                _isDebug = value;
                OnChangedExecute(ConfigProperties.IsDebug);
            }
        }

        internal bool IsSoundEnabled
        {
            get { return _isSoundEnabled; }
            set
            {
                if (_isSoundEnabled == value) return;
                _isSoundEnabled = value;
                OnChangedExecute(ConfigProperties.IsSoundEnabled);
            }
        }

        internal string SoundSet
        {
            get { return _soundSet; }
            set
            {
                if (_soundSet == value) return;
                _soundSet = value;
                OnChangedExecute(ConfigProperties.SoundSet);
            }
        }

        internal Dictionary<string, string[]> Sequences
        {
            get { return _sequences; }
            set
            {
                if (_sequences == value) return;
                _sequences = value;
                OnChangedExecute(ConfigProperties.Sequences);
            }
        }

        internal Rect WindowPosition;

        internal bool EngineControl
        {
            get { return _engineControl; }
            set
            {
                if (_engineControl == value) return;
                _engineControl = value;
                OnChangedExecute(ConfigProperties.EngineControl);
            }
        }

        public bool AbortExecuted
        {
            get { return _abortExecuted; }
            set
            {
                if (_abortExecuted == value) return;

                _abortExecuted = value;
                OnChangedExecute(ConfigProperties.AbortExecuted);
            }
        }
        public float Scale
        {
            get { return _scale; }
            set
            {
                if (_scale == value) return;

                _scale = (float) Math.Round(value, 1);
                OnChangedExecute(ConfigProperties.Scale);
            }
        }

        [Persistent]
        private RectStorage _storage = new RectStorage();


        public override void OnDecodeFromConfigNode()
        {
            WindowPosition = _storage.ToRect();

            _sequences.Clear();

            foreach (var sequenceStorage in _storages)
            {
                _sequences.Add(sequenceStorage.Key, sequenceStorage.Values);
            }
        }

        public override void OnEncodeToConfigNode()
        {
            _storage = _storage.FromRect(WindowPosition);

            _storages.Clear();

            foreach (var sequence in _sequences)
            {
                var item = new SequenceStorage();
                item.FromDictionaryItem(sequence);
                _storages.Add(item);
            }
        }

        internal event EventHandler<ConfigEventArgs> OnChanged;

        private void OnChangedExecute(ConfigProperties data)
        {
            if (OnChanged != null)
            {
                OnChanged(this, new ConfigEventArgs(data));
            }
        }
    }

    class RectStorage
    {
        [Persistent]
        internal float X;
        [Persistent]
        internal float Y;
        [Persistent]
        internal float Width;
        [Persistent]
        internal float Height;

        internal RectStorage FromRect(Rect obj)
        {
            X = obj.xMin;
            Y = obj.yMin;
            Width = obj.width;
            Height = obj.height;
            return this;
        }

        internal Rect ToRect()
        {
            return new Rect(X, Y, Width, Height);
        }
    }

    class SequenceStorage
    {
        [Persistent]
        internal string Key;

        [Persistent]
        internal string[] Values;

        internal void FromDictionaryItem(KeyValuePair<string, string[]> item)
        {
            Key = item.Key;
            Values = item.Value;
        }

        internal KeyValuePair<string, string[]> ToDictionaryItem()
        {
            return new KeyValuePair<string, string[]>(Key, Values);
        }
    }
}


