using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Data
{
    [Serializable]
    public class DialogSentence
    {
        [TextArea] public string Sentence;
        public bool IsHeroSpeaking; 
    }

    [Serializable]
    public class DialogData
    {
        [SerializeField] private string _npcName;
        [SerializeField] private Sprite _npcPortrait;
        [SerializeField] private DialogSentence[] _sentences;
        
        public string NpcName => _npcName;
        public Sprite NpcPortrait => _npcPortrait;
        public DialogSentence[] Sentences => _sentences;
        
        public void SetSentences(List<DialogSentence> sentences)
        {
            _sentences = sentences.ToArray();
        }
    }
}