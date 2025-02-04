using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Animations
{
    [System.Serializable]
    public class State
    {
        [SerializeField] public UnityEvent _onComplete;

        public string Name;
        public bool Loop;
        public Sprite[] Sprites;
        public bool AllowNext;
    }
}
