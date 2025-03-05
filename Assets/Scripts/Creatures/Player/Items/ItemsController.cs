using Scripts.Components.Fuel;
using Scripts.Components.Health;
using Scripts.Model;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Model.Definitions.Player;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player.Items
{
    public class ItemsController: MonoBehaviour
    {
        [SerializeField] private Cooldown _itemCooldown;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private Cooldown _speedUpCooldown;
        
        private InventoryService _inventoryService;
        private FuelComponent _fuel;
        private GameSession _session;
        private Player _player;
        private float _additionalSpeed;
        public float AdditionalSpeed => _speedUpCooldown.IsReady ? 0f : _additionalSpeed;
        
        private void Start()
        {
            _player = GetComponentInParent<Player>();
            _session = FindObjectOfType<GameSession>();
            _fuel = GetComponentInParent<FuelComponent>();
            _inventoryService = _player.InventoryService;
        }
        
        public void UseItem()
        {
            if (!_itemCooldown.IsReady || _session.QuickInventory.SelectedItem == null)
                return;

            switch (true)
            {
                case var _ when IsSelectedItem(ItemTag.Fuel):
                    _itemCooldown.Reset();
                    UseFuel();
                    break;
                case var _ when IsSelectedItem(ItemTag.Throwable):
                    _itemCooldown.Reset();
                    Throw();
                    break;
                case var _ when IsSelectedItem(ItemTag.Potion):
                    _itemCooldown.Reset();
                    UsePotion();
                    break;
                default:
                    Debug.Log("You can't use this item.");
                    break;
            }
        }
        
        private void UseFuel()
        {
            var newCapacity = _session.Data.Fuel.Capacity += 10;
            _session.Data.Inventory.Remove(_inventoryService.SelectedItemId, 1);
            _session.Data.Fuel.InvokeChangedEvent(_session.Data.Fuel.Value, newCapacity);
            _fuel.SetFuel(newCapacity);
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(_inventoryService.SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    Heal();
                    break;
                case Effect.SpeedUp:
                    SpeedUp(potion);
                    break;
            }
        }

        private void Heal()
        {
            var usableId = _session.QuickInventory.SelectedItem.Id;
            var potionsDef = DefsFacade.I.Potions.Get(usableId);
            var healthChangeComponent = potionsDef.Potion.GetComponent<HealthChangeComponent>();
            healthChangeComponent.HealthChangeValue = (int) potionsDef.Value;
            healthChangeComponent.ApplyHealthChange(GetComponentInParent<Player>().gameObject);

            _session.Data.Inventory.Remove(_inventoryService.SelectedItemId, 1);
        }
        
        private void SpeedUp(PotionDef potion)
        {
            if (!_speedUpCooldown.IsReady)
                return;

            _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
            _speedUpCooldown.Reset();

            _session.Data.Inventory.Remove(_inventoryService.SelectedItemId, 1);
        }
        
        private void Throw()
        {
            if (IsSelectedItem(ItemTag.Potion) || _inventoryService.SelectedItemId == _inventoryService.SwordId 
                && _inventoryService.SelectedItemCount <= 1)
                return;

            if (!_throwCooldown.IsReady)
                return;

            _player.Sounds.Play("Range");

            var throwableId = _inventoryService.SelectedItemId;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);

            var baseDamage = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
            var damage = _player.CalculateDamageWithCrit(baseDamage);

            var prefab = throwableDef.Projectile;
            prefab.GetComponent<HealthChangeComponent>().HealthChangeValue = -damage;
            _player.ThrowSpawner.SetPrefab(prefab);
            _player.ThrowSpawner.Spawn();

            _player.Animator.SetTrigger(PlayerAnimatorKeys.ThrowKey);
            _throwCooldown.Reset();

            _session.Data.Inventory.Remove(throwableId, 1);
        }
        
        private bool IsSelectedItem(ItemTag itemTag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(itemTag);
        }
    }
}