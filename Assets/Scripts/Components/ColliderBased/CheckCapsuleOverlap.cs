using System;
using System.Linq;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.ColliderBased
{
    public class CheckCapsuleOverlap : MonoBehaviour
    {
        [SerializeField] private Vector2 _point1;
        [SerializeField] internal Vector2 _point2;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private CapsuleDirection2D _direction = CapsuleDirection2D.Vertical;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            var start = GetWorldPosition(_point1);
            var end = GetWorldPosition(_point2);

            GizmosUtils.DrawCapsule(start, end, _radius, _direction);
        }
#endif

        [ContextMenu("Check Overlap")]
        public void Check()
        {
            var worldPoint1 = GetWorldPosition(_point1);
            var worldPoint2 = GetWorldPosition(_point2);
            
            var size = Physics2D.OverlapCapsuleNonAlloc(
                (worldPoint1 + worldPoint2) / 2f,
                new Vector2(Vector2.Distance(worldPoint1, worldPoint2), _radius * 2f), // Размер капсулы
                _direction,
                0f,
                _interactionResult,
                _mask
            );

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(x => overlapResult.CompareTag(tag));

                if (isInTags)
                    _onOverlap?.Invoke(overlapResult.gameObject);
            }
        }
        
        private Vector2 GetWorldPosition(Vector2 localPoint)
        {
            var worldScale = transform.lossyScale;
            
            return (Vector2)transform.position + new Vector2
            (
                localPoint.x * worldScale.x,
                localPoint.y * worldScale.y
            );
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject> { }
    }
}