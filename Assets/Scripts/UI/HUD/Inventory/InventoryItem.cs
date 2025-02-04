using Scripts.Model.Data;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.HUD.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _index;

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }
    }
}