using Scripts.Components.ColliderBased;
using Scripts.Components.GoBased;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Enemies
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private Cooldown _meleeAttackCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeAttackCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private Animator _animator;
        private static readonly int MeleeKey = Animator.StringToHash("melee");
        private static readonly int RangeKey = Animator.StringToHash("range");

        public bool IsControlledByModuleTotem { get; set; } = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update ()
        {
            if (IsControlledByModuleTotem) 
                return;

            if (_vision.isTouchingLayer)
            {
                if (_meleeAttack != null && _meleeCanAttack.isTouchingLayer)
                {
                    if (_meleeAttackCooldown.IsReady)
                        MeleeAttack();
                    return;
                }

                if (_rangeAttackCooldown.IsReady)
                    RangeAttack();
            }
        }

        public void RangeAttack()
        {
            _rangeAttackCooldown.Reset();
            _animator.SetTrigger(RangeKey);
        }

        private void MeleeAttack()
        {
            _meleeAttackCooldown.Reset();
            _animator.SetTrigger(MeleeKey);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
        public bool HasTargetInRange()
        {
            return _vision.isTouchingLayer;
        }
    }
}