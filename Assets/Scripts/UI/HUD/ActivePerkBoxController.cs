using Scripts.Model;
using Scripts.Model.Data.Models;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.HUD
{
    public class ActivePerkBoxController : MonoBehaviour
    {
        [SerializeField] private Image _activePerk;
        [SerializeField] private Image _locked;
        [SerializeField] private Image _cooldown;
        [SerializeField] private Image _hotkey;

        private PerksModel _perksModel;
        private PerkDef _currentPerk;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private void Start()
        {
            _perksModel = FindObjectOfType<GameSession>()?.PerksModel;

            if (_perksModel == null)
                return;
            
            _trash.Retain(_perksModel.Subscribe(UpdateActivePerk));
            UpdateActivePerk();
        }
        
        private void UpdateActivePerk()
        {
            _hotkey.enabled = false;
            var perkId = _perksModel.Used;

            _currentPerk = DefsFacade.I.Perks.Get(perkId);

            if (_currentPerk.IsValid)
            {
                _activePerk.sprite = _currentPerk.Icon;
                _locked.enabled = false;

                if (_currentPerk.HotKey)
                {
                    _hotkey.sprite = _currentPerk.HotKey;
                    _hotkey.enabled = true;
                }
                
                _activePerk.enabled = true;
            }
            else
            {
                _locked.enabled = true;
                _activePerk.enabled = false;
            }
        }
        
        private void Update()
        {
            if (!_currentPerk.IsValid) 
                return;
            
            var remainingTime = _currentPerk.Cooldown.RemainingTime;
            var duration = _currentPerk.Cooldown.Value;
                
            _cooldown.fillAmount = remainingTime > 0 ? remainingTime / duration : 0;
        }
        
        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}