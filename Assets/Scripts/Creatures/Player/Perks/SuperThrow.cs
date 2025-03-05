using System.Collections;
using Scripts.Model;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    public class SuperThrow : MonoBehaviour, IPerk
    {
        [SerializeField] private int _throwsCount = 3;
        public string Id { get; } = "super-throw";
        private Cooldown _cooldown;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void InitializeCooldown()
        {
            _cooldown = DefsFacade.I.Perks.Get(Id).Cooldown;
        }
        
        public void UsePerk(GameObject go)
        {
            var player = go.GetComponent<Player>();
            var inventoryService = player.InventoryService;
            if (_session.QuickInventory.SelectedDef.HasTag(ItemTag.Potion) 
                || inventoryService.SelectedItemId() == inventoryService.SwordId 
                && inventoryService.SelectedItemCount <= 1)
                return;
            
            var possibleCount = inventoryService.SelectedItemId() == inventoryService.SwordId 
                ? inventoryService.SelectedItemCount - 1 
                : inventoryService.SelectedItemCount;

            if (possibleCount >= _throwsCount && _cooldown.IsReady &&
                _session.PerksModel.IsSuperThrowSupported)
                StartCoroutine(DoSuperThrow(_throwsCount, inventoryService, player));
        }

        private IEnumerator DoSuperThrow(int throwsCount, InventoryService inventoryService, Player player)
        {
            _cooldown.Reset();

            for (var i = 0; i < throwsCount; i++)
            {
                if (_session.Data.Inventory.Count(inventoryService.SelectedItemId()) > 0)
                {
                    player.Sounds.Play("Range");

                    var throwableId = _session.QuickInventory.SelectedItem.Id;
                    var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
                    player.ThrowSpawner.SetPrefab(throwableDef.Projectile);
                    player.ThrowSpawner.Spawn();

                    player.Animator.SetTrigger(PlayerAnimatorKeys.ThrowKey);
                    _session.Data.Inventory.Remove(throwableId, 1);

                    yield return new WaitForSeconds(0.2f);
                }

                else
                    break;
            }
        }

    }
}