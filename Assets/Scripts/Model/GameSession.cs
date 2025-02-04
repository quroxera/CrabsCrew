using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Scripts.Components.LevelManagement;
using Scripts.Model.Data;
using Scripts.Model.Data.Models;
using Scripts.Model.Definitions.Player;
using Scripts.Utils.Disposables;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Scripts.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;

        private PlayerData _savedData;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private readonly List<string> _checkPoints = new List<string>();
        private readonly List<string> _removedItems = new List<string>();
        private readonly Dictionary<string, Vector3> _movedObjects = new Dictionary<string, Vector3>();
        public static bool IsPaused { get; set; }
        public int LevelIndex { get; private set; }
        public PlayerData Data => _data;
        public InventoryModel Inventory { get; private set; }
        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }
        
        private void Awake()
        {
            var existSession = GetExistsSession();

            if (existSession != null)
            {
                existSession.StartSession(_defaultCheckPoint, _levelIndex);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                SaveSessionData();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckPoint, _levelIndex);
            }
        }

        private void StartSession(string defaultCheckPoint, int levelIndex)
        {
            SetChecked(defaultCheckPoint);
            LevelIndex = levelIndex;
            TrackSessionStart(levelIndex);

            LoadUIs();
            SpawnPlayer();
        }

        private void TrackSessionStart(int levelIndex)
        {
            var eventParams = new Dictionary<string, object>()
            {
                {"level_index", levelIndex}
            };
            
            AnalyticsEvent.Custom("level_start", eventParams);
        }

        private void SpawnPlayer()
        {
            var checkPoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkPoints.Last();

            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.Id != lastCheckPoint) 
                    continue;
                
                checkPoint.SpawnPlayer();
                break;
            }
        }

        private void InitModels()
        {
            Inventory = new InventoryModel(_data);
            QuickInventory = new QuickInventoryModel(_data);
            PerksModel = new PerksModel(_data);
            StatsModel = new StatsModel(_data);
            
            _trash.Retain(Inventory);
            _trash.Retain(QuickInventory);
            _trash.Retain(PerksModel);
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int)StatsModel.GetValue(StatId.Hp);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            LoadOnScreenControls();
        }
        
        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadOnScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        }
        
        public void SaveSessionData()
        {
            _savedData = _data.Clone();
        }

        public void LoadSessionData()
        {
            _data = _savedData.Clone();

            _trash.Dispose();
            InitModels();
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                    return session;
            }

            return null;
        }

        public bool IsChecked(string id)
        {
            return _checkPoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (_checkPoints.Contains(id)) 
                return;
            
            _checkPoints.Add(id);
            SaveSessionData();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public void StoreState(string id)
        {
            if (!_removedItems.Contains(id))
                _removedItems.Add(id);
        }

        public bool RestoreState(string id)
        {
            return _removedItems.Contains(id);
        }
        
        public void StoreObjectPosition(string id, Vector3 position)
        {
            _movedObjects[id] = position;
        }

        public bool TryGetObjectPosition(string id, out Vector3 position)
        {
            return _movedObjects.TryGetValue(id, out position);
        }

        internal void OnGameComplete()
        {
            Data.Hp.Value = (int)StatsModel.GetValue(StatId.Hp);
            
            _checkPoints.Clear();
            _removedItems.Clear();
            _movedObjects.Clear();
        }

        internal void DestroyProgress()
        {
            Destroy(gameObject);
        }
    }
}