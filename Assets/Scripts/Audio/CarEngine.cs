using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class CarEngine : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSourceModelPrefab;
        [SerializeField] private AudioMixerGroup _offLoadAudioMixerGroup;
        [SerializeField] private AudioMixerGroup _onLoadAudioMixerGroup;

        private BlendTrack _offLoadBlendTrack;
        private BlendTrack _onLoadBlendTrack;
        private EnginePreset _selectedEnginePreset;
        private EngineSimulator _engineSimulator;
        private float _engineLoad;

        public AudioSource AudioSourceModelPrefab => _audioSourceModelPrefab;
        public EnginePreset SelectedEnginePreset => _selectedEnginePreset;
        public AudioMixerGroup offLoadAudioMixerGroup => _offLoadAudioMixerGroup;
        public AudioMixerGroup onLoadAudioMixerGroup => _onLoadAudioMixerGroup;

        #region Mono

        private void FixedUpdate()
        {
            if (_selectedEnginePreset == null) return;

            UpdateRpm();
            UpdateThrottlePosition();
        }

        private void OnDestroy()
        {
            UnloadCarEngine();
        }

        #endregion




        public void InitCarEngine(EnginePreset enginePreset, EngineSimulator engineSimulator)
        {
            _selectedEnginePreset = enginePreset;
            _engineSimulator = engineSimulator;

            LoadCarEngine();
        }

        private void LoadCarEngine()
        {
            _offLoadBlendTrack = new BlendTrack();
            _offLoadBlendTrack.Init(this, false);
            _onLoadBlendTrack = new BlendTrack();
            _onLoadBlendTrack.Init(this, true);

        }

        public void UnloadCarEngine()
        {
            if(_selectedEnginePreset == null) return;

            _offLoadBlendTrack.Unload();
            _onLoadBlendTrack.Unload();
            _selectedEnginePreset = null;
        }

        private void UpdateEngineLoadVolumes(float offLoadVolume, float onLoadVolume)
        {
            _offLoadAudioMixerGroup.audioMixer.SetFloat("OffLoadVolume", offLoadVolume);
            _onLoadAudioMixerGroup.audioMixer.SetFloat("OnLoadVolume", onLoadVolume);
        }

        private void UpdateRpm()
        {
            var rpm = _engineSimulator.Rpm;
            _offLoadBlendTrack.UdpateRpm(rpm);
            _onLoadBlendTrack.UdpateRpm(rpm);
        }

        private void UpdateThrottlePosition()
        {
            _engineLoad = _engineSimulator.ThrottlePosition;
            UpdateEngineLoadVolumes(UpdateOffLoadVolume(), UpdateOnLoadVolume());
        }

        private float UpdateOffLoadVolume()
        {
            var relativeValue = Mathf.Sqrt(0.5f * (1 - (_engineLoad * 2 - 1)));
            return Mathf.Lerp(-20, 0, relativeValue);
        }

        private float UpdateOnLoadVolume()
        {
            var relativeValue = Mathf.Sqrt(0.5f * (1 + (_engineLoad * 2 - 1)));
            return Mathf.Lerp(-30, 0, relativeValue);
        }
    }
}

