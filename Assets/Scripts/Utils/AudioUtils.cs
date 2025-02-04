using UnityEngine;

namespace Scripts.Utils
{
    public static class AudioUtils
    {
        private const string SfxSourceTag = "SfxAudioSource";
        public static AudioSource FindSfxSource()
        {
            return GameObject.FindWithTag(SfxSourceTag).GetComponent<AudioSource>();
        }
    }
}