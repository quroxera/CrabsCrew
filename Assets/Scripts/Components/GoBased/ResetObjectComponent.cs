using UnityEngine;

namespace Scripts.Components.GoBased
{
    public class ResetObjectComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _defaultCoordinates;
        [SerializeField] private GameObject _objectToReset;

        public void ResetCoordinates()
        {
            _objectToReset.transform.position = _defaultCoordinates;
        }
    }
}