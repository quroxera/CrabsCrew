using System;
using Scripts.Model.Data.Properties;
using UnityEngine;

namespace Scripts.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public IntProperty Hp = new IntProperty();
        public FloatProperty Fuel = new FloatProperty();
        public PerksData Perks = new PerksData();
        public LevelData Levels = new LevelData();
        
        public InventoryData Inventory => _inventory;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}