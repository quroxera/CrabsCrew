using Scripts.Components.GoBased;
using Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.LevelManagement
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private SpawnComponent _playerSpawner;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        private GameSession _session;
        public string Id => _id;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            
            if (_session.IsChecked(_id))
                _setChecked?.Invoke();
            else
                _setUnchecked?.Invoke();
        }

        public void Check()
        {
            if (_session.IsChecked(_id))
                return;
            
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnPlayer()
        {
            _playerSpawner.Spawn();
        }
    }
}