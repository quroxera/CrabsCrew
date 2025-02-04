using System.Collections;
using Scripts.Components.ColliderBased;
using Scripts.Components.GoBased;
using Scripts.Creatures.Enemies.Patrolling;
using UnityEngine;

namespace Scripts.Creatures.Enemies
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missPlayerCooldown = 0.5f;

        private Coroutine _current;
        private GameObject _target;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private bool _isDead;
        private Patrol _patrol;
        private bool _isAttacking;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnPlayerInVision(GameObject go)
        {
            if (_isDead)
                return;
            
            var directionToTarget = (go.transform.position - transform.position).normalized;
            var distanceToTarget = Vector2.Distance(transform.position, go.transform.position);
            var hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, LayerMask.GetMask("Ground", "Wall"));

            if (hit.collider != null) 
                return;
            
            _target = go;
            StartState(AgroToPlayer());
        }

        private IEnumerator AgroToPlayer()
        {
            LookAtPlayer();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToPlayer());
        }

        private void LookAtPlayer()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        private IEnumerator GoToPlayer()
        {
            while (_vision.isTouchingLayer && !_isDead)
            {
                if (_canAttack.isTouchingLayer && !_isAttacking && !_isDead)
                {
                    _isAttacking = true;
                    StartState(Attack());
                }
                else
                    SetDirectionToTarget();

                yield return null;
            }

            if (_isDead) 
                yield break;
            
            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missPlayerCooldown);

            StartState(_patrol.DoPatrol());
        }

        private IEnumerator Attack()
        {
            while (_canAttack.isTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            _isAttacking = false;
            StartState(GoToPlayer());
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);
        }
    }
}