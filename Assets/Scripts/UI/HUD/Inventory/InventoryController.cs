using System.Collections.Generic;
using Scripts.Model;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils.Disposables;
using UnityEngine;

namespace Scripts.UI.HUD.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItem _prefab;

        private GameSession _session;
        private List<InventoryItem> _createdItems = new List<InventoryItem>();

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Inventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            CleanupDestroyedItems();

            var instance = DefsFacade.I;
            var inventorySize = instance.MaxItems;
            var inventory = _session.Inventory.Inventory;

            int itemsToProcess = Mathf.Min(inventory.Length, inventorySize);

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