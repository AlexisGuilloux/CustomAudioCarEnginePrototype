using System.Collections.Generic;

namespace Audio
{
    public class BlendTrack
    {
        private readonly List<CarEngineAsset> _carEngineAssets = new();

        private float _rpm;
        private bool _initialized;

        public void Init(CarEngine carEngine, bool isOnLoadBlendTrack)
        {
            var carEngineAssetdatas = (isOnLoadBlendTrack ?
                carEngine.SelectedEnginePreset.onLoadBlendTrackData.carEngineAssetDatas :
                carEngine.SelectedEnginePreset.offLoadBlendTrackData.carEngineAssetDatas);

            for (int i = 0; i < carEngineAssetdatas.Length; i++)
            {
                CarEngineAsset carEngineAsset = new CarEngineAsset(carEngineAssetdatas[i]);
                carEngineAsset.Init(carEngine.transform,
                    isOnLoadBlendTrack ? carEngine.onLoadAudioMixerGroup : carEngine.offLoadAudioMixerGroup,
                    carEngine.AudioSourceModelPrefab);

                _carEngineAssets.Add(carEngineAsset);
            }
            _initialized = true;
        }

        public void Unload()
        {
            for (int i = 0; i < _carEngineAssets.Count; i++)
            {
                _carEngineAssets[i].Stop();

            }
            _carEngineAssets.Clear();
            _initialized = false;
        }

        public void UdpateRpm(float rpm)
        {
            if (!_initialized) return;

            _rpm = rpm;

            for (int i = 0; i < _carEngineAssets.Count; i++)
            {
                _carEngineAssets[i].UpdateRpm(_rpm);
            }
        }
    }
}

