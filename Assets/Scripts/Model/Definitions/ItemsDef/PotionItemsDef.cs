using System;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef
{
    [CreateAssetMenu(menuName = "Defs/PotionItemsDef", fileName = "PotionItemsDef")]
    public class PotionItemsDef : ScriptableObject
    {
        [SerializeField] private PotionDef[] _items;

        public PotionDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
    }

    [Serializable]
    public struct PotionDef : IHaveId
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private GameObject _potion;
        [SerializeField] private Effect _effect;
        [SerializeField] private float _value;
        [SerializeField] private float _time;

        public string Id => _id;
        public GameObject Potion => _potion;
        public Effect Effect => _effect;
        public float Value => _value;
        public float Time => _time;
    }
    
    public enum Effect
    {
        AddHp,
        SpeedUp
    }
}