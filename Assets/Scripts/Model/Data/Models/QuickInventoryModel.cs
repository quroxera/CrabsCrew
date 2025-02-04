using Scripts.Model.Data.Properties;
using Scripts.Model.Definitions.ItemsDef;
using UnityEngine;

namespace Scripts.Model.Data.Models
{
    public class QuickInventoryModel : BaseInventoryModel
    {
        public readonly IntProperty SelectedIndex = new IntProperty();

        public InventoryItemData SelectedItem => Inventory.Length > 0 ? Inventory[SelectedIndex.Value] : null;
        public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);
        public QuickInventoryModel(PlayerData data) : base(data)
        {
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
        }

        protected override InventoryItemData[] LoadInventory()
        {
            return _data.Inventory.GetAll(ItemTag.Usable);
        }

        protected override void OnChangedInventory(string id, int value)
        {
            var newInventory = _data.Inventory.GetAll(ItemTag.Usable);
            
            Inventory = newInventory;
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            InvokeOnChanged();
        }

        internal void SetNextItem()
        {
            SelectedIndex.Value = (int)Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }
    }
}