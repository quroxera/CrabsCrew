using System;
using System.Collections.Generic;
using System.IO;
using Scripts.Model.Definitions.ItemsDef.Localization;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef
{
    [CreateAssetMenu(menuName = "Defs/PopupDef", fileName = "PopupDef")]
    public class PopupsDef : ScriptableObject
    {
        [SerializeField] private List<PopupData> _popups = new List<PopupData>();

        public string GetPopupText(string key)
        {
            foreach (var popup in _popups)
            {
                if (popup.Key == key)
                {
                    return popup.Value;
                }
            }
            return string.Empty;
        }

        [ContextMenu("Update Popups")]
        public void OnLocaleChanged()
        {
            LoadLocalizedPopups();
        }

        public void LoadLocalizedPopups()
        {
            var localeKey = LocalizationManager.I.LocaleKey;
            var path = $"Assets/Resources/Locales/Popups/popups_{localeKey}.tsv";
            
            try
            {
                string fileContent = File.ReadAllText(path);
                var localizedPopups = PopupLocalizationParser.Parse(fileContent);

                _popups.Clear();
                foreach (var pair in localizedPopups)
                {
                    _popups.Add(new PopupData { Key = pair.Key, Value = pair.Value });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load file: {path}. {ex}");
            }
        }
    }
    
    [Serializable]
    public class PopupData
    {
        public string Key;
        [TextArea] public string Value;
    }
}