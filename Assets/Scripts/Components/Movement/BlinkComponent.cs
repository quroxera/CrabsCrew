using UnityEngine;

namespace Scripts.Components.Movement
{
    public class BlinkComponent : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        public void DirectBlink(Transform blinkDirection)
        {
            Vector3 targetPosition = blinkDirection.position;

            if (!_collider.bounds.Contains(targetPosition))
            {
                targetPosition = _collider.ClosestPoint(targetPosition);
            }

            transform.position = targetPosition;
        }
    }
}
