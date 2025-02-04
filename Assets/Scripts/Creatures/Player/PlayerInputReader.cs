using Scripts.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Creatures.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            var direction = context.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            if (context.performed)
                _player.Attack();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            if (context.performed)
                _player.Interact();
        }

        public void OnCast(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            if (context.performed)
                _player.UseSkill();
        }


        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            if (context.performed)
                _player.UseItem();
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            if (context.performed)
                _player.NextItem();
        }
        
        public void OnSwitchFlashlight(InputAction.CallbackContext context)
        {
            if (GameSession.IsPaused)
                return;
            
            if (context.performed)
                _player.SwitchFlashlight();
        }
    }
}