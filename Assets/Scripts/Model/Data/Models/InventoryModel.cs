namespace Scripts.Model.Data.Models
{
    public class InventoryModel : BaseInventoryModel
    {
        public InventoryModel(PlayerData data) : base(data) { }

        protected override InventoryItemData[] LoadInventory()
        {
            return _data.Inventory.AllItems;
        }
    }
}