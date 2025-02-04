using System.Collections.Generic;
using Scripts.Model;
using Scripts.Utils.Disposables;
using UnityEngine;

namespace Scripts.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private int _itemsCount = 5;
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;

        private GameSession _session;
        private List<InventoryItemWidget> _createdItems = new List<InventoryItemWidget>();

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            CleanupDestroyedItems();

            var maxItems = _itemsCount;
            var inventory = _session.QuickInventory.Inventory;

            int itemsToProcess = Mathf.Min(inventory.Length, maxItems);

            for (var i = _createdItems.Count; i < itemsToProcess; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createdItems.Add(item);
            }

            for (var i = 0; i < itemsToProcess; i++)
            {
                _createdItems[i].SetData(inventory[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            for (var i = itemsToProcess; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }

        private void CleanupDestroyedItems()
        {
            _createdItems.RemoveAll(item => item == null || item.gameObject == null);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}