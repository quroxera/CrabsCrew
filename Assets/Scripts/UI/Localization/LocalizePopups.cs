using Scripts.Model.Definitions.ItemsDef;

namespace Scripts.UI.Localization
{
    public class LocalizePopups : AbstractLocalizeItem
    {
        private PopupsDef _popupsDef;

        protected override void Localize()
        {
            if (_popupsDef == null)
                return;

            _popupsDef.LoadLocalizedPopups();
        }

        public void SetPopupsData(PopupsDef popupsDef)
        {
            _popupsDef = popupsDef;
            Localize();
        }
    }
}