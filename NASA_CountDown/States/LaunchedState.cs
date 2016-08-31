using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NASA_CountDown.Config;
using NASA_CountDown.StateMachine;
using UnityEngine;

namespace NASA_CountDown.States
{
    public class LaunchedState: BaseGuiState
    {
        private List<AudioClip> _events = new List<AudioClip>();
        private const string LiftOffSoundName = "LiftOff";
        private const string TowerClearedSoundName = "TowerCleared";
        private const string AllEngineRunnigSoundName = "AllEngineRuning";

        private AudioSource _audioSource;
        private GameObject _obj;

        public LaunchedState(string name, KerbalFsmEx machine) : base(name, machine)
        {
            OnEnter = EnterState;
            OnLeave = OnLeaveFromState;
            updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
        }

        private void OnLeaveFromState(KFSMState kfsmState)
        {
            _obj.DestroyGameObjectImmediate();
        }

        private void EnterState(KFSMState kfsmState)
        {
            _obj = new GameObject();
            _audioSource = _obj.AddComponent<AudioSource>();
            _audioSource.panLevel = 0;
            _audioSource.volume = GameSettings.VOICE_VOLUME;


            if (string.IsNullOrEmpty(ConfigInfo.Instance.SoundSet)) return;

            var clips =
                GameDatabase.Instance.databaseAudio.Where(
                    x => x.name.StartsWith("NASA_Countdown") && x.name.Contains(ConfigInfo.Instance.SoundSet)).ToList();

            _events = clips.Where(x => x.name.Contains("/Events")).ToList();

            _obj.AddComponent<MonoBehaviour>().StartCoroutine(LaunchedSuccess());
        }

        private IEnumerator LaunchedSuccess()
        {
            var clip = _events.FirstOrDefault(x => x.name.EndsWith(LiftOffSoundName));

            if (clip != null)
            {
                _audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }

            clip = _events.FirstOrDefault(x => x.name.EndsWith(AllEngineRunnigSoundName));

            if (clip != null)
            {
                _audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }

            clip = _events.FirstOrDefault(x => x.name.EndsWith(TowerClearedSoundName));

            if (clip != null)
            {
                _audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }

            Machine.RunEvent("Finish");
        }
    }
}
