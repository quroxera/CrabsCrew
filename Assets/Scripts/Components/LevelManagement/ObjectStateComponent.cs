using Scripts.Model;
using UnityEngine;

namespace Scripts.Component.LevelManagement
{
    public class ObjectStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id; 
        private GameSession _session;
        public string Id => _id;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            if (_session.RestoreState(Id))
                Destroy(gameObject);

            if (_session.TryGetObjectPosition(Id, out var savedPosition))
                transform.position = savedPosition;
        }

        private void Update()
        {
            SaveObjectPosition();
        }

        public void SaveObjectPosition()
        {
            if (_session == null) 
                return;
            
            _session.StoreObjectPosition(Id, transform.position);
        }
    }
}