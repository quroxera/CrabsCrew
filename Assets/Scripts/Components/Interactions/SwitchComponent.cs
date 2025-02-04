﻿using UnityEngine;

namespace Scripts.Components.Interactions
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private bool _updateOnStart;
        [SerializeField] private string _animationKey;

        private void Start()
        {
            if(_updateOnStart)
                _animator.SetBool(_animationKey, _state);
        }

        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animationKey, _state);
        }

        [ContextMenu(itemName: "Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}