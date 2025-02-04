using System.Collections.Generic;
using System.IO;
using Scripts.Model.Data;
using Scripts.Model.Definitions.ItemsDef.Localization;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Model.Definitions.ItemsDef
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
    public class DialogDef : ScriptableObject
    {
        [SerializeField] private string _dialogKey;
        [SerializeField] private DialogData _data;
        public DialogData Data => _data;
        
        [ContextMenu("Update Dialog Data")]
        public void OnLocaleChanged()
        {
            LoadLocalizedDialog();
        }
        
        public void LoadLocalizedDialog()
        {
            var localeKey = LocalizationManager.I.LocaleKey;
            var path = $"Assets/Resources/Locales/Dialogs/{localeKey}/{_dialogKey}.tsv";
            
            try
            {
                string fileContent = File.ReadAllText(path);

                var sentences = new List<DialogSentence>();
                DialogLocalizationParser.Parse(fileContent, sentences);

                _data.SetSentences(sentences);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load file: {path}. {ex}");
            }
        }
    }
}