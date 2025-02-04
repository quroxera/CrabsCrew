using System;
using UnityEngine;

namespace Scripts.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;
        private float _timesUp;
        public float RemainingTime => Mathf.Max(0, _timesUp - Time.time);
        public bool IsReady => _timesUp <= Time.time;
        public float Value
        {
            get => _value;
            set => _value = value;
        }
        
        public void Reset()
        {
            _timesUp = Time.time + _value;
        }
    }
}