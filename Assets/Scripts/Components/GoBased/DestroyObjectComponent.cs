using Scripts.Component.LevelManagement;
using Scripts.Model;
using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private ObjectStateComponent _state;

        public void DestroyObject()
        {
            Destroy(_objectToDestroy);

            if (_state != null)
                FindObjectOfType<GameSession>().StoreState(_state.Id);
        }
    }
}