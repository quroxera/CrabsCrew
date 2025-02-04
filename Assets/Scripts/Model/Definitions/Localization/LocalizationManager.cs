using System;
using System.Collections.Generic;
using Scripts.Model.Data.Properties;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef.Localization
{
    public class LocalizationManager
    {
        public static readonly LocalizationManager I;

        private StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;
        public string LocaleKey => _localeKey.Value;

        public event Action OnLocaleChanged;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        private LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }

        private void LoadLocale(string localeToLoad)
        {
            var localeDef = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = localeDef.GetData();
            _localeKey.Value = localeToLoad;
            OnLocaleChanged?.Invoke(); 
        }

        public string Localize(string key)
        {
            return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
        }

        public void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }
    }
}