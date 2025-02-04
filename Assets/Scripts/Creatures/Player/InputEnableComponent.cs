using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Creatures.Player
{
    public class InputEnableComponent : MonoBehaviour
    {
        private PlayerInput _input;
        private void Start()
        {
            _input = FindObjectOfType<PlayerInput>();
        }

        public void SetInput(bool isEnabled)
        {
            _input.enabled = isEnabled;
        }
    }
}