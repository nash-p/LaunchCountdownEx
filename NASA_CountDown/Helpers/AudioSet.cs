using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NASA_CountDown.Helpers
{
    public class AudioSet
    {
        public AudioSet(string name)
        {
            Name = name;

            var sounds = GameDatabase.Instance.databaseAudio.Where(x => x.name.StartsWith("NASA_CountDown", StringComparison.OrdinalIgnoreCase) && x.name.Contains(name)).ToList();

            TimerSounds = sounds.Where(x => x.name.Contains("/Timer")).ToList();
            Abort = sounds.FirstOrDefault(x => x.name.EndsWith("Aborted", StringComparison.OrdinalIgnoreCase));
            Hold = sounds.FirstOrDefault(x => x.name.EndsWith("Hold", StringComparison.OrdinalIgnoreCase));
            LiftOff = sounds.FirstOrDefault(x => x.name.EndsWith("LiftOff", StringComparison.OrdinalIgnoreCase));
            AllEngineRunnig = sounds.FirstOrDefault(x => x.name.EndsWith("AllEngineRuning", StringComparison.OrdinalIgnoreCase));
            TowerCleared = sounds.FirstOrDefault(x => x.name.EndsWith("TowerCleared", StringComparison.OrdinalIgnoreCase));
        }

        public List<AudioClip> TimerSounds { get; private set; }

        public string Name { get; private set; }

        public AudioClip Abort { get; private set; }

        public AudioClip Hold { get; private set; }

        public AudioClip LiftOff { get; private set; }

        public AudioClip TowerCleared { get; private set; }

        public AudioClip AllEngineRunnig { get; private set; }
    }
}
