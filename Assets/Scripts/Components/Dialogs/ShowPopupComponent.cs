using System.Collections.Generic;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.UI.HUD.Dialogs;
using Scripts.UI.Localization;
using UnityEngine;

namespace Scripts.Components.Dialogs
{
    [RequireComponent(typeof(LocalizePopups))]
    public class ShowPopupComponent : MonoBehaviour
    {
        private PopupsDef _popups;
        [SerializeField] private string[] _popupKeys;

        private DialogBoxController _dialogBox;

        private void Awake()
        {
            if (_popups == null)
                _popups = Resources.Load<PopupsDef>("Locales/Popups/Popups");
        }

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<DialogBoxController>();

            UpdateLocalizePopups(_popups);

            List<string> popupsContent = new List<string>();
            foreach (var popupKey in _popupKeys)
            {
                popupsContent.Add(_popups.GetPopupText(popupKey));
                _dialogBox.ShowDialog(popupsContent, usePopup: true);
            }
        }

        private void UpdateLocalizePopups(PopupsDef popupsDef)
        {
            var localizePopup = GetComponent<LocalizePopups>();

            if (localizePopup != null)
                localizePopup.SetPopupsData(popupsDef);
        }
    }
}