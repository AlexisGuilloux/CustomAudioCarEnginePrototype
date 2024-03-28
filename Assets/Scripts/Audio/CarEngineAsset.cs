using System;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Audio
{
    [Serializable]
    public class CarEngineAsset
    {
        private AudioSource _audioSource;
        private float _rpm;

        private AudioClip _audioClip;
        private int _defaultPitchValue;
        private float _min;
        private float _max;
        private int _fadeInLength;
        private int _fadeOutLength;

        public CarEngineAsset(CarEngineAssetData data)
        {
            _audioClip = data.audioClip;
            _defaultPitchValue = data.defaultPichValue;
            _min = data.min;
            _max = data.max;
            _fadeInLength = data.fadeInLength;
            _fadeOutLength = data.fadeOutLength;
        }

        public void Init(Transform parentTransform, UnityEngine.Audio.AudioMixerGroup audioMixerGroup, AudioSource audioSourceModelPrefab)
        {
            CreateAudioSource(parentTransform, audioSourceModelPrefab);
            _audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        public void Play()
        {
            _audioSource.Play();
        }

        public void Stop()
        {
            if(_audioSource == null) return;
            _audioSource.Stop();
            Object.Destroy(_audioSource.gameObject);
        }

        public void UpdateRpm(float rpm)
        {
            if (!_audioSource.isPlaying) Play();

            _rpm = rpm;
            UpdatePitch();

            //if not in fade zone, volume at 1
            if (_rpm > (_min + _fadeInLength) && _rpm < (_max - _fadeOutLength))
            {
                _audioSource.volume = 1f;
            }
            else if( _rpm < _min || _rpm > _max)
            {
                _audioSource.volume = 0f;
            }
            else
            {
                if(_rpm > _min && _rpm < _min + _fadeInLength)
                {
                    UpdateFadeIn();
                }
                else
                {
                    UpdateFadeOut();
                }
            }
        }

        public void UpdateFadeOut()
        {
            var relativeValue = Mathf.InverseLerp((_max - _fadeOutLength), _max, _rpm) * 2f - 1f;
            var volumeAtRpm = Mathf.Sqrt(0.5f * (1f - relativeValue));
            _audioSource.volume = volumeAtRpm;
        }

        public void UpdateFadeIn()
        {
            var relativeValue = Mathf.InverseLerp(_min, (_min + _fadeInLength), _rpm) * 2f - 1f;
            var volumeAtRpm = Mathf.Sqrt(0.5f * (1f + relativeValue));
            _audioSource.volume = volumeAtRpm;
        }

        public void UpdatePitch()
        {
            var pitch = Math.Min(1f + math.log2(_rpm / _defaultPitchValue), 10);
            _audioSource.pitch = pitch;
        }

        public void CreateAudioSource(Transform parentTransform, AudioSource audioSourceModelPrefab)
        {
            var audioSource = Object.Instantiate(audioSourceModelPrefab, parentTransform);
            audioSource.enabled = true;
            audioSource.name = $"EngineAudioSource_{_defaultPitchValue}RPM";
            audioSource.playOnAwake = false;
            audioSource.Stop();
            audioSource.clip = _audioClip;
            audioSource.spatialize = false;
            audioSource.spatialBlend = 0f;
            audioSource.loop = true;
            _audioSource = audioSource;
        }
    }
}

