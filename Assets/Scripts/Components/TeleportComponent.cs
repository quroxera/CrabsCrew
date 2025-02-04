using UnityEngine;

namespace Scripts.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        private void OnEnable() { }
        public void Teleport(GameObject target)
        {
            if (!enabled)
                return;

            target.transform.position = _destTransform.position;
        }
    }
}