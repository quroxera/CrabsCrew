using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.ColliderBased
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;
        
        private void OnEnable() { }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled)
                return;

            if (!other.gameObject.IsInLayer(_layer))
                return;

            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag))
                return;

            _action?.Invoke(other.gameObject);
        }

        [System.Serializable]
        public class EnterEvent : UnityEvent<GameObject> { }
    }
}