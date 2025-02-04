using Scripts.Model.Definitions.ItemsDef.Localization;
using UnityEngine;

namespace Scripts.UI.Localization
{
    public abstract class AbstractLocalizeItem : MonoBehaviour
    {
        protected abstract void Localize();
        
        protected virtual void Awake()
        {
            LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
            Localize();
        }
        
        private void OnLocaleChanged()
        {
            Localize();
        }
        
        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= OnLocaleChanged;
        }
    }
}