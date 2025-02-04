using System;
using System.Linq;
using Scripts.Model.Definitions.ItemsDef.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Localization
{
    public class LocalizeImage : AbstractLocalizeItem
    {
        [SerializeField] private IconId[] _icons;
        [SerializeField] private Image _icon;

        protected override void Localize()
        {
            var iconData = _icons.FirstOrDefault(x => x.Id == LocalizationManager.I.LocaleKey);
            if (iconData != null)
                _icon.sprite = iconData.Icon;
        }
    }

    [Serializable]
    public class IconId
    {
        public string Id;
        public Sprite Icon;
    }
}