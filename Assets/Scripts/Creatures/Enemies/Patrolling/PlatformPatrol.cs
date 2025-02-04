using System.Collections;
using UnityEngine;

namespace Scripts.Creatures.Enemies.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _obstacles;

        [SerializeField] private float _checkDistance = 0.5f;

        private readonly Color _rayColor = Color.red;

        private Creature _creature;
        private bool _movingRight = true;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                var direction = _movingRight ? Vector2.right : Vector2.left;
                _creature.SetDirection(direction);

                if (IsNearEdge())
                    _movingRight = !_movingRight;

                yield return null;
            }
        }

        private bool IsNearEdge()
        {
            var position = transform.position;
            var groundCheckPosition = _movingRight
                ? new Vector2(position.x + _checkDistance, position.y)
                : new Vector2(position.x - _checkDistance, position.y);

            Debug.DrawRay(groundCheckPosition, Vector2.down * _checkDistance, _rayColor);
            var groundHit = Physics2D.Raycast(groundCheckPosition, Vector2.down, _checkDistance, _groundLayer);
            
            Debug.DrawRay(groundCheckPosition, Vector2.down * _checkDistance, _rayColor);
            var obstaclesHit = Physics2D.Raycast(groundCheckPosition, Vector2.down, _checkDistance, _obstacles);

            return groundHit.collider == null || obstaclesHit.collider != null;
        }
    }
}