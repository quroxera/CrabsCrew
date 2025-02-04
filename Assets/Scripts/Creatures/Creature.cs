using Scripts.Components.Audio;
using Scripts.Components.ColliderBased;
using Scripts.Components.GoBased;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] public bool invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float JumpSpeed;
        [SerializeField] private float _damageJumpSpeed;

        [Header("Checkers")]
        [SerializeField] protected LayerMask GroundLayer;
        [SerializeField] public SpawnListComponent Particles;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] private LayerCheck _groundCheck;

        public Rigidbody2D Rigidbody;
        public Vector2 Direction;
        public Animator Animator;
        public PlaySoundsComponent Sounds;

        protected bool IsGrounded;
        private bool IsJumping;
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.isTouchingLayer;
        }
        private void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();

            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(AnimatorKeys.IsGroundKey, IsGrounded);
            Animator.SetBool(AnimatorKeys.IsRunningKey, Direction.x != 0);
            Animator.SetFloat(AnimatorKeys.VerticalVelocityKey, Rigidbody.velocity.y);

            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;
            
            if (IsGrounded)
                IsJumping = false;
            
            if (isJumpPressing)
            {
                IsJumping = true;

                var isFalling = Rigidbody.velocity.y <= 0.001f;
                
                if (!isFalling) 
                    return yVelocity;

                yVelocity = CalculateJumpVelocity(yVelocity);
            }

            else if (Rigidbody.velocity.y > 0 && IsJumping)
                yVelocity *= 0.5f;

            return yVelocity;
        }
        
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded) 
                return yVelocity;
            
            yVelocity += JumpSpeed;
            DoJumpVfx();
            return yVelocity;
        }
        
        internal virtual float CalculateXVelocity()
        {
            return Direction.x * CalculateSpeed();
        }
        
        protected virtual float CalculateSpeed()
        {
            return _speed;
        }

        internal void DoJumpVfx()
        {
            Particles.Spawn("Jump");
            Sounds.Play("Jump");
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage()
        {
            IsJumping = false;
            Animator.SetTrigger(AnimatorKeys.HitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpSpeed);
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AnimatorKeys.AttackKey);
        }

        public void SetAttack()
        {
            _attackRange.Check();
            Sounds.Play("Melee");
            Particles.Spawn("Attack");
        }
    }
}