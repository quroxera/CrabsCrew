using System;
using Scripts.Model.Definitions.ItemsDef;
using UnityEngine;

namespace Scripts.Model.Definitions.Player
{
    [Serializable]
    public struct StatDef
    {
        [SerializeField] private string _name;
        [SerializeField] private StatId _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private StatLevelDef[] _levels;
        
        public string Name => _name;
        public StatId Id => _id;
        public Sprite Icon => _icon;
        public StatLevelDef[] Levels => _levels;
    }

    [Serializable]
    public struct StatLevelDef
    {
        [SerializeField] private float _value;
        [SerializeField] private ItemWithCount _price;
        
        public float Value => _value;
        public ItemWithCount Price => _price;
    }

    public enum StatId
    {
        Hp,
        Speed,
        RangeDamage,
        CritChance
    }
}