using System;
using UnityEngine;

namespace Scripts.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private State[] _states;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private float _nextFrameTime;
        private State _currentState;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (_states.Length > 0)
            {
                SetClip(_states[0].Name);
            }
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time;
            _currentSpriteIndex = 0;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time)
                return;

            if (_currentState.Sprites == null || _currentState.Sprites.Length == 0)
            {
                enabled = false;
                return;
            }

            if (_currentSpriteIndex >= _currentState.Sprites.Length)
            {
                if (_currentState.Loop)
                {
                    _currentSpriteIndex = 0;
                }
                else if (_currentState.AllowNext)
                {
                    int currentStateIndex = System.Array.IndexOf(_states, _currentState);
                    if (currentStateIndex >= _states.Length-1)
                    {
                        enabled = false;
                        return;
                    }
                    _currentState._onComplete?.Invoke();
                    _currentState = _states[currentStateIndex + 1];
                    SetClip(_currentState.Name);
                }
                else
                {
                    enabled = false;
                    _currentState._onComplete?.Invoke();
                    return;
                }
            }

            _renderer.sprite = _currentState.Sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public void SetClip(string name)
        {
            foreach (var state in _states)
            {
                if (state.Name == name)
                {
                    _currentState = state;
                    _currentSpriteIndex = 0;
                    enabled = true;
                    return;
                }
            }
        }
    }
}