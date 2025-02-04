using Scripts.Model.Definitions.ItemsDef;
using Scripts.UI.HUD.Dialogs;
using Scripts.UI.Localization;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.Dialogs
{
    [RequireComponent(typeof(LocalizeDialog))]
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private DialogDef _dialog;
        [SerializeField] private UnityEvent _onComplete;
        
        private DialogBoxController _dialogBox;

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<DialogBoxController>();

            UpdateLocalizeDialog(_dialog);
            _dialogBox.ShowDialog(_dialog.Data, usePopup: false, _onComplete);
        }

        private void UpdateLocalizeDialog(DialogDef dialogDef)
        {
            var localizeDialog = GetComponent<LocalizeDialog>();

            if (localizeDialog != null)
                localizeDialog.SetDialogData(dialogDef);
        }

        public void Show(DialogDef def)
        {
            _dialog = def;
            Show();
        }
    }
}