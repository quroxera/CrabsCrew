using Scripts.Utils;
using UnityEngine;

namespace Scripts.Components.Audio
{
    public class PlaySfxSound : MonoBehaviour
    {
        private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        public void Play()
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSource();

            _source.PlayOneShot(_clip);
        }
    }
}