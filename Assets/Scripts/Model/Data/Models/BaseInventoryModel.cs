using System;
using Scripts.Utils.Disposables;

namespace Scripts.Model.Data.Models
{
    public abstract class BaseInventoryModel :IDisposable
    {
        protected PlayerData _data;
        public InventoryItemData[] Inventory { get; protected set; }

        public event Action OnChanged;

        protected BaseInventoryModel(PlayerData data)
        {
            _data = data;
            Inventory = LoadInventory();
            _data.Inventory.OnChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        protected abstract InventoryItemData[] LoadInventory();

        protected virtual void OnChangedInventory(string id, int value)
        {
            var newInventory = LoadInventory();

            if (newInventory.Length != Inventory.Length || Array.FindIndex(newInventory, x => x.Id == id) != -1)
            {
                Inventory = newInventory;
                InvokeOnChanged();
            }
        }

        protected void InvokeOnChanged()
        {
            if (OnChanged != null)
            {
                OnChanged.Invoke();
            }
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}