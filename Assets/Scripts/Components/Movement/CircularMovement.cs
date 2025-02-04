using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components.Movement
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;

        private List<Transform> _objects = new List<Transform>();
        private float _angleOffset;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                _objects.Add(child);
            }
        }

        private void Update()
        {
            _angleOffset += _speed * Time.deltaTime;

            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                if (_objects[i] == null)
                {
                    _objects.RemoveAt(i);
                }
            }

            float angleStep = (2 * Mathf.PI) / _objects.Count;

            for (int i = 0; i < _objects.Count; i++)
            {
                float targetAngle = _angleOffset + angleStep * i;

                Vector2 targetPos = new Vector2(Mathf.Cos(targetAngle) * _radius,
                    Mathf.Sin(targetAngle) * _radius);

                _objects[i].localPosition = Vector2.Lerp(_objects[i].localPosition, targetPos, Time.deltaTime * _speed);
            }
        }
    }
}