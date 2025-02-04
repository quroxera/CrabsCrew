using Scripts.Components.ColliderBased;
using UnityEngine;

namespace Scripts.Creatures.Enemies.FroggyTheBoss
{
    public class FroggyTheBoss : MonoBehaviour
    {
        private static readonly int Health = Animator.StringToHash("Health");
        [SerializeField] private CheckCapsuleOverlap _attackRange;
        [SerializeField] private Animator _animator;
        private GameObject _target;

        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (_animator.GetInteger(Health) <= 0 )
                return;
            
            EnsureFacingPlayer();
        }

        private void EnsureFacingPlayer()
        {
            if (_target == null) return;

            var direction = _target.transform.position - transform.position;
            if (direction.x == 0) return;

            var isFacingRight = transform.localScale.x > 0;
            var shouldFaceRight = direction.x > 0;

            if (isFacingRight != shouldFaceRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        public void SetAttack()
        {
            _attackRange.Check();
        }
    }
}