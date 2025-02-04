using Scripts.Model;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Model.Definitions.Player;
using Scripts.UI.Widgets;
using Scripts.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows.PlayerStats
{
    public class PlayerStatsWindow: AnimatedWindow
    {
        [SerializeField] private Transform _statsContainer;
        [SerializeField] private StatWidget _prefab;
        
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private ItemWidget _price;

        private DataGroup<StatDef, StatWidget> _dataGroup;
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        protected override void Start()
        {
            base.Start();
            
            _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _statsContainer);
            
            _session = FindObjectOfType<GameSession>();
            
            _session.StatsModel.InterfaceSelectedStat.Value = DefsFacade.I.Player.Stats[0].Id;
            _trash.Retain(_session.StatsModel.Subscribe(OnStatsChanged));
            _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgrade));

            OnStatsChanged();
        }

        private void OnUpgrade()
        {
            var selected = _session.StatsModel.InterfaceSelectedStat.Value;
            _session.StatsModel.LevelUp(selected);
        }

        private void OnStatsChanged()
        {
            var stats = DefsFacade.I.Player.Stats;
            _dataGroup.SetData(stats);
            
            var selected = _session.StatsModel.InterfaceSelectedStat.Value;
            var nextLevel = _session.StatsModel.GetCurrentLevel(selected) + 1;
            var def = _session.StatsModel.GetLevelDef(selected, nextLevel);
            _price.SetData(def.Price);
            
            _price.gameObject.SetActive(def.Price.Count != 0);
            _upgradeButton.gameObject.SetActive(def.Price.Count != 0);
        }

        protected override void OnDestroy()
        {
            _trash.Dispose();
            base.OnDestroy();
        }
    }
}