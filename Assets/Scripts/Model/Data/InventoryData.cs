using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Model.Definitions.ItemsDef;
using UnityEngine;

namespace Scripts.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public InventoryItemData[] AllItems => _inventory.ToArray();

        public void Add(string id, int value)
        {
            if (value <= 0)
                return;

            var itemDef = DefsFacade.I.Items.Get(id);

            if (itemDef.IsVoid)
                return;

            var isFull = _inventory.Count >= DefsFacade.I.Player.InventorySize;

            if (isFull)
                return;

            if (!itemDef.HasTag(ItemTag.Stackable))
            {
                var item = new InventoryItemData(id);
                item.Value = 1;
                _inventory.Add(item);
            }

            else
            {
                var item = GetItem(id);

                if (item == null)
                {
                    item = new InventoryItemData(id);
                    _inventory.Add(item);
                }

                if (item.Value + value <= itemDef.Capacity)
                    item.Value += value;
                
                else if (item.Value + value > itemDef.Capacity)
                    item.Value = itemDef.Capacity;
            }

            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);

            if (itemDef.IsVoid)
                return;

            var item = GetItem(id);

            if (item == null)
                return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);

            OnChanged?.Invoke(id, Count(id));
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }

            return null;
        }

        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                    count += item.Value;
            }
            return count;
        }
        
        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();
            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));

                if (isAllRequirementsMet)
                    retValue.Add(item);
            }
            return retValue.ToArray();
        }

        public bool IsEnough(params ItemWithCount[] items)
        {
            var joined = new Dictionary<string, int>();
            foreach (var item in items)
            {
                if(joined.ContainsKey(item.ItemId))
                    joined[item.ItemId] += item.Count;
                else 
                    joined.Add(item.ItemId, item.Count);
            }

            foreach (var kvp in joined)
            {
                var count = Count(kvp.Key);
                if (count < kvp.Value)
                    return false;
            }
            
            return true;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}