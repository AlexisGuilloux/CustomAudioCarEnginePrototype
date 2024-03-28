using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "Audio car engine preset", menuName = "ScriptableObjects/Audio/EnginePreset", order = 0)]
    public class EnginePreset : ScriptableObject
    {
        public EngineType engineType;
        public BlendTrackData offLoadBlendTrackData;
        public BlendTrackData onLoadBlendTrackData;
    }
}

