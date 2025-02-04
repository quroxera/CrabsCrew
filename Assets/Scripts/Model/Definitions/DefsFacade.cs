using Scripts.Model.Definitions.Player;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private ThrowableItemsDef _throwableItems;
        [SerializeField] private PotionItemsDef _potions;
        [SerializeField] private PerkItemsDef _perks;
        [SerializeField] private PlayerDef _player;

        public InventoryItemsDef Items => _items;
        public ThrowableItemsDef Throwable => _throwableItems;
        public PotionItemsDef Potions => _potions;
        public PerkItemsDef Perks => _perks;
        public PlayerDef Player => _player;
        public int MaxItems => _player.InventorySize;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs () : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}