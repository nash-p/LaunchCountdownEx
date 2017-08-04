using System;
using System.Collections;
using NASA_CountDown.Config;
using NASA_CountDown.Helpers;
using NASA_CountDown.StateMachine;
using UnityEngine;

namespace NASA_CountDown.States
{
    public class LaunchedState : BaseGuiState
    {
        private AudioSource _audioSource;
        private GameObject _obj;
        protected DummyComponent _dummy;

        public LaunchedState(string name, KerbalFsmEx machine) : base(name, machine)
        {
            OnEnter = EnterState;
            OnLeave = OnLeaveFromState;
            updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
        }

        private void OnLeaveFromState(KFSMState kfsmState)
        {
            _obj.DestroyGameObjectImmediate();
            _dummy.StopAllCoroutines();
        }

        private void EnterState(KFSMState kfsmState)
        {
            _obj = new GameObject();
            _audioSource = _obj.AddComponent<AudioSource>();
            _audioSource.spatialBlend = 0;
            _audioSource.volume = GameSettings.VOICE_VOLUME;

            if (ConfigInfo.Instance.CurrentAudio == null) return;
            
            //_obj.AddComponent<MonoBehaviour>().StartCoroutine(LaunchedSuccess());
            _dummy = _obj.AddComponent<DummyComponent>();
            _dummy.StartCoroutine(LaunchedSuccess());
        }

        private IEnumerator LaunchedSuccess()
        {
            var clip = ConfigInfo.Instance.CurrentAudio.LiftOff;

            if (clip != null)
            {
                _audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }

            clip = ConfigInfo.Instance.CurrentAudio.AllEngineRunnig;

            if (clip != null)
            {
                _audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }

            clip = ConfigInfo.Instance.CurrentAudio.TowerCleared;

            if (clip != null)
            {
                _audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }

            Machine.RunEvent("Finish");
        }

        public override void Draw()
        {
        }
    }
}
