using Scripts.Creatures.Player;
using Scripts.Model.Data;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;
        
        public void Add(GameObject go)
        {
            var player = go.GetInterface<ICanAddInInventory>();
            player?.AddInInventory(_id, _count);
        }
        
        public void Add()
        {
            var player = FindObjectOfType<Player>().gameObject.GetInterface<ICanAddInInventory>();
            player?.AddInInventory(_id, _count);
        }
    }
}