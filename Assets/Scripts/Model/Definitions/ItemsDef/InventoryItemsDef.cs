using System;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private int _capacity;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;

        public string Id => _id;
        public Sprite Icon => _icon;

        public bool HasTag(ItemTag tag)
        {
            return _tags != null && _tags.Contains(tag);
        }

        public int Capacity
        {
            get
            {
                if (!HasTag(ItemTag.Stackable))
                {
                    return 1;
                }

                return _capacity;
            }
        }

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}