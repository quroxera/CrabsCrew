using System;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef
{
    [CreateAssetMenu(menuName = "Defs/PerkItems", fileName = "PerkItems")]
    public class PerkItemsDef : DefRepository<PerkDef> { }

    [Serializable]
    public struct PerkDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _info;
        [SerializeField] private ItemWithCount _price;
        [SerializeField] private Cooldown _cooldown;
        [SerializeField] private Sprite _hotKey;
        public string Id => _id;
        public Sprite Icon => _icon;
        public string Info => _info;
        public ItemWithCount Price => _price;
        public Cooldown Cooldown => _cooldown;
        public Sprite HotKey => _hotKey;

        public bool IsValid => !string.IsNullOrEmpty(_id);
    }
    
    [Serializable]
    public struct ItemWithCount
    {
        [InventoryId] [SerializeField] private string _itemId;
        [SerializeField] private int _count;

        public string ItemId => _itemId;
        public int Count => _count;
    }
}