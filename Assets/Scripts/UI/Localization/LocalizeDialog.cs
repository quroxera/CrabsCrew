using Scripts.Model.Definitions.ItemsDef;

namespace Scripts.UI.Localization
{
    public class LocalizeDialog : AbstractLocalizeItem
    {
        private DialogDef _dialogDef;

        protected override void Localize()
        {
            if (_dialogDef == null)
                return;
            
            _dialogDef.LoadLocalizedDialog();
        }

        public void SetDialogData(DialogDef dialogDef)
        {
            _dialogDef = dialogDef;
            Localize();
        }
    }
}