using Scripts.Model;

namespace Scripts.Creatures.Player
{
    public class InventoryService
    {
        private readonly GameSession _session;

        public InventoryService(GameSession session)
        {
            _session = session;
        }

        private int GetItemCount(string itemId) => _session.Data.Inventory.Count(itemId);

        public readonly string SwordId = "Sword";
        public int CoinsCount => GetItemCount("Coin");

        public int SwordsCount => GetItemCount("Sword");

        public string SelectedItemId()
        {
            return _session.QuickInventory.SelectedItem == null
                ? string.Empty : _session.QuickInventory.SelectedItem.Id;
        }

        public int SelectedItemCount => GetItemCount(SelectedItemId());
    }
}