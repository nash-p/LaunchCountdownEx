using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LaunchCountDown.Config;
using UnityEngine;

namespace LaunchCountDown.Common
{
    internal class LaunchControl : MonoBehaviour
    {
        private AudioSource _audioSource;
        private int _tick;
        private float _defaultAltitude;

        internal List<AudioClip> CountDownClips { get; private set; }
        internal List<AudioClip> EventClips { get; private set; }

        protected void Awake()
        {
            CountDownClips = new List<AudioClip>();
            EventClips = new List<AudioClip>();

            LaunchCountdownConfig.Instance.Info.OnChanged += ConfigChanged;

            Load();

            _audioSource = this.gameObject.AddComponent<AudioSource>();
            _audioSource.panLevel = 0;
            _audioSource.volume = GameSettings.VOICE_VOLUME;
        }

        private void ConfigChanged(object sender, ConfigEventArgs eventArgs)
        {
            if (eventArgs.Data == ConfigProperties.IsSoundEnabled || eventArgs.Data == ConfigProperties.SoundSet)
            {
                Load();
            }

            if (eventArgs.Data == ConfigProperties.EngineControl)
            {
                CheckEngine();
            }
        }

        private void Load()
        {
            if (!LaunchCountdownConfig.Instance.Info.IsSoundEnabled)
            {
                _tick = 15;
                return;
            }

            List<AudioClip> audios =
                GameDatabase.Instance.databaseAudio.Where(x => x.name.Contains(LaunchCountdownConfig.Instance.Info.SoundSet))
                    .ToList();

            CountDownClips.Clear();
            EventClips.Clear();

            CountDownClips = audios.Where(x => x.name.Contains("/Timer")).ToList();
            EventClips = audios.Where(x => x.name.Contains("/Events")).ToList();

            if (!CountDownClips.Any())
            {
                _tick = 15;
                LaunchCountdownConfig.Instance.Info.IsSoundEnabled = false;
                return;
            }

            _tick = CountDownClips.Count;

            DebugHelper.WriteMessage("tick reload {0}", _tick);
        }

        public void RepeatingWorker()
        {
            DebugHelper.WriteMessage("tick {0}", _tick);

            if (_tick <= 0)
            {
                //StopRepeatingWorker();
                return;
            }

            _tick--;

            if (OnTick != null)
            {
                OnTick(this, new LaunchEvenArgs(_tick));
            }

            TryStagingExecute();

            if (!LaunchCountdownConfig.Instance.Info.IsSoundEnabled) return;

            var clip = CountDownClips.FirstOrDefault(x => x.name.EndsWith(string.Format("/{0}", _tick)));

            if (clip == null)
            {
                return;
            }


            DebugHelper.WriteMessage("Sound played {0}", clip.name);

            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        internal void Run()
        {
            if (FlightGlobals.ActiveVessel != null)
            {
                _defaultAltitude = (float) FlightGlobals.ActiveVessel.altitude;
            }

            CheckEngine();
           // StartRepeatingWorker(1);
        }

        internal void Abort()
        {
            //StopRepeatingWorker();

            DisableEngineControl();

            DebugHelper.WriteMessage("Vessel aborted");

            _tick = CountDownClips.Any() ? CountDownClips.Count : 15;

            if (OnVesselAborted != null)
            {
                OnVesselAborted(this, EventArgs.Empty);
            }

            if (!LaunchCountdownConfig.Instance.Info.IsSoundEnabled) return;

            _audioSource.Stop();
            _audioSource.clip = EventClips.Single(x => x.name.EndsWith("/Aborted"));
            _audioSource.Play();
        }

        internal void Launched()
        {
            LaunchCountdownConfig.Instance.Info.OnChanged -= ConfigChanged;

            DebugHelper.WriteMessage("Vessel launched");

            DisableEngineControl();

            if (LaunchCountdownConfig.Instance.Info.IsSoundEnabled)
            {
              StartCoroutine(PlayLaunchedSound());  
            }

            if (OnVesselLaunched != null)
            {
                OnVesselLaunched(this, EventArgs.Empty);
            }
        }

        private IEnumerator PlayLaunchedSound()
        {
            List<AudioClip> clips = EventClips.Where(x => !x.name.Contains("/Aborted")).OrderBy(x => x.name).ToList();

            foreach (AudioClip clip in clips)
            {
                _audioSource.Stop();
                _audioSource.clip = clip;
                _audioSource.Play();
                yield return new WaitForSeconds(3);
            }
        }

        internal event EventHandler<LaunchEvenArgs> OnTick;

        internal event EventHandler OnVesselLaunched;

        internal event EventHandler OnVesselAborted;

        protected void OnDestroy()
        {
            EventClips.Clear();
            CountDownClips.Clear();

            LaunchCountdownConfig.Instance.Info.OnChanged -= ConfigChanged;
        }

        private void CheckEngine()
        {
            if (LaunchCountdownConfig.Instance.Info.EngineControl)
            {
                EnableEngineControl();
            }
            else
            {
                DisableEngineControl();
            }
        }

        private void EnableEngineControl()
        {
            if (FlightGlobals.ActiveVessel == null) return;

            // ReSharper disable once DelegateSubtraction
            FlightGlobals.ActiveVessel.OnFlyByWire -= OnFlyByWire;
            FlightGlobals.ActiveVessel.OnFlyByWire += OnFlyByWire;
            DebugHelper.WriteMessage("Vessel control enabled");
        }

        private void OnFlyByWire(FlightCtrlState st)
        {
            switch (_tick)
            {
                case 7:
                case 6:
                case 5:
                case 4:
                case 3:
                case 2:
                case 1:
                    st.mainThrottle = 0.01f;
                    break;
                case 0:
                    st.mainThrottle = 1f;
                    break;
                default:
                    st.mainThrottle = 0f;
                    break;
            }
        }

        private void TryStagingExecute()
        {
            switch (_tick)
            {
                case 9:
                case 8:
                case 7:
                case 6:
                case 5:
                case 4:
                case 3:
                case 2:
                case 1:
                    RunStaging(_tick);
                    break;

                case 0:
                    if (!RunStaging(_tick))
                    {
                        Staging.ActivateNextStage();
                    }
                    StartCoroutine(CheckVesselState());
                    break;
            }
        }

        private IEnumerator CheckVesselState()
        {
            yield return new WaitForSeconds(1);

            if (FlightGlobals.ActiveVessel.verticalSpeed <= 0)
            {
                DebugHelper.WriteMessage("Check Vessel aborted: {0} {1}", FlightGlobals.ActiveVessel.altitude, _defaultAltitude);

                if (LaunchCountdownConfig.Instance.Info.AbortExecuted)
                {
                    var vesselParts = FlightGlobals.ActiveVessel.parts;
                    BaseAction.FireAction(vesselParts, KSPActionGroup.Abort, KSPActionType.Activate);
                }

                Abort();
            }
            else
            {
                DebugHelper.WriteMessage("Check Vessel launched: {0} {1}", FlightGlobals.ActiveVessel.altitude, _defaultAltitude);

                try
                {
                    Launched();
                }
                catch (Exception ex)
                {
                    DebugHelper.WriteMessage(ex.StackTrace);
                }
            }
        }

        private void DisableEngineControl()
        {
            if (FlightGlobals.ActiveVessel == null) return;

            // ReSharper disable once DelegateSubtraction
            FlightGlobals.ActiveVessel.OnFlyByWire -= OnFlyByWire;
            DebugHelper.WriteMessage("Vessel control disabled");
        }

        private bool RunStaging(int count)
        {
            string[] items;

            var result = LaunchCountdownConfig.Instance.Info.Sequences.TryGetValue(FlightGlobals.ActiveVessel.id,
                out items);

            if (!result)
            {
                DebugHelper.WriteMessage("Current id is {0} with stage {1}", FlightGlobals.ActiveVessel.id, count);

                LaunchCountdownConfig.Instance.Info.Sequences.Keys.ToList().ForEach(x => Debug.LogWarning(x));

                return false;
            }

            
            if (!Regex.IsMatch(items[count], "^[0-9]+"))
            {
                DebugHelper.WriteMessage("Stage {0} not activated", count);
                return false;
            }

            try
            {
                Staging.ActivateStage(
                    int.Parse(LaunchCountdownConfig.Instance.Info.Sequences[FlightGlobals.ActiveVessel.id][count]));
                DebugHelper.WriteMessage("Stage {0} activated", count);
                return true;
            }
            catch (Exception ex)
            {
                DebugHelper.WriteMessage("Staging sequence error {0}", ex.Message);
                return false;
            }
        }
    }
}