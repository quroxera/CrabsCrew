using System;
using System.Globalization;
using Scripts.Model;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Model.Definitions.ItemsDef.Localization;
using Scripts.Model.Definitions.Player;
using Scripts.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows.PlayerStats
{
    public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _currentValue;
        [SerializeField] private Text _increaseValue;
        [SerializeField] private ProgressBarWidget _progressBar;
        [SerializeField] private GameObject _selector;

        private GameSession _session;
        private StatDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }

        private void UpdateView()
        {
            var statsModel = _session.StatsModel;

            _icon.sprite = _data.Icon;
            _name.text = LocalizationManager.I.Localize(_data.Name);
            _currentValue.text = statsModel.GetValue(_data.Id).ToString(CultureInfo.InvariantCulture);

            var currentLevel = statsModel.GetCurrentLevel(_data.Id);
            var nextLevel = currentLevel + 1;
            var increaseValue = statsModel.GetValue(_data.Id, nextLevel);
            _increaseValue.text = $"=>{increaseValue}";
            _increaseValue.gameObject.SetActive(increaseValue > 0);

            var maxLevel = DefsFacade.I.Player.GetStat(_data.Id).Levels.Length - 1;
            _progressBar.SetProgress(currentLevel / (float) maxLevel);

            _selector.SetActive(statsModel.InterfaceSelectedStat.Value == _data.Id);
        }

        public void SetData(StatDef data, int index)
        {
            _data = data;
            
            if (_session != null)
                UpdateView();
        }

        public void OnSelect()
        {
            _session.StatsModel.InterfaceSelectedStat.Value = _data.Id;
        }
    }
}