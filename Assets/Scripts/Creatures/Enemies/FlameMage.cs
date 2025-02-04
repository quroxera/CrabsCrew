using System.Collections;
using Scripts.Components.ColliderBased;
using Scripts.Components.GoBased;
using Scripts.Components.Movement;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Enemies
{
    public class FlameMage : MonoBehaviour
    {
        [Header("Checkers")]
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private float _castRange = 5f;
        [SerializeField] private LayerMask _avoidLayersForSummon;

        [Header("Skills")]
        [SerializeField] private Cooldown _fireballCooldown;
        [SerializeField] private Cooldown _blinkCooldown;
        [SerializeField] private Cooldown _summonCooldown;
        [SerializeField] private Cooldown _spikesCooldown;
        [SerializeField] private Transform _directBlinkDest;

        private BlinkComponent _blinkComponent;

        protected static readonly int SummonKey = Animator.StringToHash("summon");
        protected static readonly int SpikesKey = Animator.StringToHash("spikes");
        protected static readonly int FireballKey = Animator.StringToHash("fireball");
        protected static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        private SpawnListComponent _skills;
        private Animator _animator;
        private Creature _creature;
        private GameObject _target;
        private Coroutine _castCoroutine;

        private bool _isDead = false;

        private void Awake()
        {
            _skills = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _blinkComponent = GetComponent<BlinkComponent>();
        }

        private void Update()
        {
            if (_isDead)
                return;

            if (_target != null)
            {
                if (IsTargetInCastRange())
                {
                    StopMoving();

                    if (_castCoroutine == null)
                        _castCoroutine = StartCoroutine(CastSkillsWithDelay());
                }
                else
                {
                    if (_vision.isTouchingLayer && _blinkCooldown.IsReady)
                    {
                        BlinkToTarget(_target.transform);
                    }
                    else
                    {
                        MoveTowardsTarget();
                    }
                }
            }
        }

        private bool IsTargetInCastRange()
        {
            if (_target == null)
                return false;

            float direction = Mathf.Sign(_target.transform.position.x - transform.position.x);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, _castRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, Vector2.right * direction * _castRange, hit.collider != null ? Color.green : Color.red);

            return hit.collider != null && hit.collider.gameObject == _target;
        }

        private void MoveTowardsTarget()
        {
            var direction = (_target.transform.position - transform.position).normalized;

            float distance = 1f;

            Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(direction.x * 0.5f, 0);

            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, distance, LayerMask.GetMask("Ground"));
            Debug.DrawRay(raycastOrigin, Vector2.down * distance, hit.collider != null ? Color.green : Color.red);

            if (hit.collider != null)
            {
                _creature.SetDirection(new Vector2(direction.x, 0));
            }
            else
            {
                StopMoving();
            }
        }

        private void StopMoving()
        {
            _creature.SetDirection(Vector2.zero);
        }

        private IEnumerator CastSkillsWithDelay()
        {
            if (_fireballCooldown.IsReady)
            {
                CastFireball();
                yield return new WaitForSeconds(1f);
            }

            if (_spikesCooldown.IsReady)
            {
                CastSpikes();
                yield return new WaitForSeconds(1f);
            }

            if (_summonCooldown.IsReady)
            {
                CastSummon();
                yield return new WaitForSeconds(3f);
            }

            _castCoroutine = null;
        }

        private void CastSummon()
        {
            _summonCooldown.Reset();
            _animator.SetTrigger(SummonKey);

            Vector3 spawnPosition = _skills.GetSpawnPosition("Summon");
            float spawnRadius = 0.5f;

            Vector3 availablePosition = FindNearestAvailablePosition(spawnPosition, spawnRadius);

            _skills.SetSpawnPosition("Summon", availablePosition);
            _skills.Spawn("Summon");
        }

        private void CastSpikes()
        {
            _spikesCooldown.Reset();
            _animator.SetTrigger(SpikesKey);
            _skills.Spawn("Spikes");
        }

        private void CastFireball()
        {
            _fireballCooldown.Reset();
            _animator.SetTrigger(FireballKey);
            _skills.Spawn("Fireball");
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            ResetState();
        }

        public void OnPlayerInVision(GameObject go)
        {
            if (_isDead)
                return;

            _target = go;
        }

        public void OnPlayerOutOfVision()
        {
            if (_isDead)
                return;

            ResetState();
            _skills.Spawn("Blink");
            _blinkComponent.DirectBlink(_directBlinkDest);
        }

        private void ResetState()
        {
            _target = null;
            _creature.SetDirection(Vector2.zero);

            if (_castCoroutine != null)
            {
                StopCoroutine(_castCoroutine);
                _castCoroutine = null;
            }
        }

        private void BlinkToTarget(Transform targetTransform)
        {
            if (_blinkCooldown.IsReady)
            {
                float xOffset = Random.Range(0, 2) == 0
                    ? Random.Range(-_castRange, -(_castRange / 2))
                    : Random.Range((_castRange / 2), _castRange);

                Vector3 blinkPosition = targetTransform.position + new Vector3(xOffset, 0, 0);

                blinkPosition.x = Mathf.Clamp(blinkPosition.x, transform.position.x - _castRange, transform.position.x + _castRange);

                GameObject tempBlinkPosition = new GameObject("TempBlinkPosition");
                tempBlinkPosition.transform.position = blinkPosition;

                _blinkCooldown.Reset();
                _skills.Spawn("Blink");
                _blinkComponent.DirectBlink(tempBlinkPosition.transform);

                Destroy(tempBlinkPosition);
            }
        }

        private Vector3 FindNearestAvailablePosition(Vector3 position, float radius)
        {
            if (IsSpawnPositionClear(position, radius))
            {
                return position;
            }

            for (float offset = radius; offset <= radius * 5; offset += radius)
            {
                for (float angle = 0; angle < 360; angle += 30)
                {
                    Vector3 offsetPosition = position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * offset;
                    if (IsSpawnPositionClear(offsetPosition, radius))
                    {
                        return offsetPosition;
                    }
                }
            }

            return position;
        }

        private bool IsSpawnPositionClear(Vector3 position, float radius)
        {
            Collider2D overlap = Physics2D.OverlapCircle(position, radius, _avoidLayersForSummon);
            return overlap == null;
        }
    }
}