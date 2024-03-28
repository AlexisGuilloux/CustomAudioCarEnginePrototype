using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class CarEngineAssetData
    {
        public AudioClip audioClip;
        public int defaultPichValue;
        public float min;
        public float max;
        public int fadeInLength;
        public int fadeOutLength;
    }
}
