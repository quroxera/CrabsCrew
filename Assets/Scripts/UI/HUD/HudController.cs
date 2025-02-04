using Scripts.Model;
using Scripts.Model.Definitions.Player;
using Scripts.UI.Widgets;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private ProgressBarWidget _fuelBar;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;
            _session.Data.Fuel.OnChanged += OnFuelChanged;
            
            OnHealthChanged(_session.Data.Hp.Value, 0);
            OnFuelChanged(_session.Data.Fuel.Value, 0);
            
            if (_session.Data.Fuel.Value > 0)
                _fuelBar.gameObject.SetActive(true);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var valueHp = (float) newValue / maxHealth;
            _healthBar.SetProgress(valueHp);
        }

        private void OnFuelChanged(float newValue, float oldValue)
        {
            if (_session.Data.Fuel.Value > 0)
                _fuelBar.gameObject.SetActive(true);
            
            var maxFuel = _session.Data.Fuel.Capacity;
            var valueFuel = (float) newValue / maxFuel;
            _fuelBar.SetProgress(valueFuel);
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
            _session.Data.Fuel.OnChanged -= OnFuelChanged;
        }

        public void OnPause()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }
        
        public void OnDebug()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow");
        }

        public void OnInventoryOpened()
        {
            WindowUtils.CreateWindow("UI/InventoryWindow");
        }
    }
}