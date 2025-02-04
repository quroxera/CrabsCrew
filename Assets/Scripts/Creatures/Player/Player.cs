using System.Collections;
using Scripts.Components.ColliderBased;
using Scripts.Components.Fuel;
using Scripts.Components.GoBased;
using Scripts.Components.Health;
using Scripts.Creatures.Player.Items;
using Scripts.Creatures.Player.Perks;
using Scripts.Effects.CameraRelated;
using Scripts.Model;
using Scripts.Model.Data;
using Scripts.Model.Definitions.Player;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player
{
    public class Player : Creature, ICanAddInInventory
    {
        [SerializeField] private float _fallVelocity;
        [SerializeField] private float _damageVelocity;

        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _disarmed;

        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private Cooldown _attackCooldown;

        [SerializeField] private FlashlightComponent _flashlight;

        private bool _isOnWall;
        private float _defaultGravityScale;
        public HealthComponent Health;
        private CameraShakeEffect _cameraShake;

        private ItemsController _itemsController;
        private PerksController _perks;
        private GameSession _session;

        public InventoryService InventoryService { get; private set; }
        public SpawnComponent ThrowSpawner => _throwSpawner;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = FindObjectOfType<GameSession>();
            Health = GetComponent<HealthComponent>();
            _perks = GetComponentInChildren<PerksController>();
            _itemsController = GetComponentInChildren<ItemsController>();
            InventoryService = new InventoryService(_session);
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnPlayerUpgraded;

            Health.SetHealth(_session.Data.Hp.Value);
            
            UpdatePlayerWeapon();
        }
        
        private void OnPlayerUpgraded(StatId statId)
        {
            if (statId != StatId.Hp) 
                return;
            
            var health = (int)_session.StatsModel.GetValue(statId);
            _session.Data.Hp.Value = health;
            Health.SetHealth(health);
        }

        internal int CalculateDamageWithCrit(int damage)
        {
            const int critModifier = 2;
            var critChance = _session.StatsModel.GetValue(StatId.CritChance);

            if (Random.value * 100 <= critChance)
                return damage * critModifier;

            return damage;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == InventoryService.SwordId)
                UpdatePlayerWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        public void OnFuelChanged(float currentFuel)
        {
            _session.Data.Fuel.Value = currentFuel;
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;

            if (_wallCheck.isTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(PlayerAnimatorKeys.IsOnWallKey, _isOnWall);
        }

        internal override float CalculateXVelocity()
        {
            Dash dash = null;
            if (_perks.GetCurrentPerk() is Dash)
                dash = (Dash)_perks.GetCurrentPerk();

            if (!dash || !dash.IsDashing)
                return base.CalculateXVelocity();

            var dashDirection = Direction;
            if (dashDirection.x == 0)
                dashDirection = new Vector2(transform.localScale.x, 0);

            return dashDirection.normalized.x * dash.Velocity;
        }

        protected override float CalculateYVelocity()
        {
            switch (_perks.GetCurrentPerk())
            {
                case Dash dash when dash.IsDashing:
                    return Direction.normalized.y;
                case DoubleJump doubleJump when IsGrounded || _isOnWall:
                    doubleJump.ResetDoubleJump();
                    break;
            }

            var isJumpPressing = Direction.y > 0;
            if (!isJumpPressing && _isOnWall)
                return 0f;

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded || !_session.PerksModel.IsDoubleJumpSupported || !(_perks.GetCurrentPerk() is DoubleJump doubleJump))
                return base.CalculateJumpVelocity(yVelocity);

            doubleJump.UsePerk(gameObject);
            return JumpSpeed;
        }

        protected override float CalculateSpeed()
        {
            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _itemsController.AdditionalSpeed;
        }

        public override void Attack()
        {
            if (InventoryService.SwordsCount <= 0)
                return;

            if (!_attackCooldown.IsReady)
                return;

            var healthChangeComponent = GetComponentInChildren<HealthChangeComponent>();
            var baseDamage = healthChangeComponent.HealthChangeValue;
            var damage = CalculateDamageWithCrit(baseDamage);
            healthChangeComponent.HealthChangeValue = damage;

            _attackCooldown.Reset();
            StartCoroutine(WaitForAttackToComplete(healthChangeComponent, baseDamage));

            base.Attack();
        }

        private IEnumerator WaitForAttackToComplete(HealthChangeComponent healthChangeComponent, int baseDamage)
        {
            while (!_attackCooldown.IsReady)
                yield return null;

            healthChangeComponent.HealthChangeValue = baseDamage;
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            _cameraShake?.Shake();

            if (InventoryService.CoinsCount > 0)
                SpawnCoins();
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        private void UpdatePlayerWeapon()
        {
            Animator.runtimeAnimatorController = InventoryService.SwordsCount > 0 ? _armed : _disarmed;
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(InventoryService.CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.IsInLayer(GroundLayer))
                return;

            var contact = collision.contacts[0];

            if (contact.relativeVelocity.y >= _fallVelocity)
                Particles.Spawn("Fall");

            if (contact.relativeVelocity.y >= _damageVelocity)
                GetComponent<HealthComponent>().ApplyHealthChange(-1);
        }

        public void SwitchFlashlight()
        {
            _flashlight.Switch();
        }
        
        public void UseSkill()
        {
            _perks.GetCurrentPerk()?.UsePerk(gameObject);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        public void UseItem()
        {
            _itemsController.UseItem();
        }
    }
}