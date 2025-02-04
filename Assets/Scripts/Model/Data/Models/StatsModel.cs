using System;
using Scripts.Model.Data.Properties;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Model.Definitions.Player;
using Scripts.Utils.Disposables;

namespace Scripts.Model.Data.Models
{
    public class StatsModel : IDisposable
    {
        private readonly PlayerData _data;
        public event Action OnChanged;
        public event Action<StatId> OnUpgraded;
        public readonly ObservableProperty<StatId> InterfaceSelectedStat = new ObservableProperty<StatId>();
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        public StatsModel(PlayerData data)
        {
            _data = data;
            _trash.Retain(InterfaceSelectedStat.Subscribe((x, y ) => OnChanged?.Invoke()));
        }
        
        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void LevelUp(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;

            if (def.Levels.Length <= nextLevel) 
                return;
            
            var price = def.Levels[nextLevel].Price;
            if (!_data.Inventory.IsEnough(price))
                return;
            
            _data.Inventory.Remove(price.ItemId, price.Count);
            _data.Levels.LevelUp(id);
            
            OnChanged?.Invoke();
            OnUpgraded?.Invoke(id);    
        }

        public float GetValue(StatId id, int level = -1)
        {
            return GetLevelDef(id, level).Value;
        }

        public StatLevelDef GetLevelDef(StatId id, int level = -1)
        {
            if (level == -1)
                level = GetCurrentLevel(id);
            
            var def = DefsFacade.I.Player.GetStat(id);
            
            return def.Levels.Length > level ? def.Levels[level] : default;
        }

        public int GetCurrentLevel(StatId id) => _data.Levels.GetLevel(id);
        
        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}