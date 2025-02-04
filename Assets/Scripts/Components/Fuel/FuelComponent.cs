using System;
using Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.Fuel
{
    public class FuelComponent : MonoBehaviour
    {
        [SerializeField] public FuelChangeEvent onRefill;
        [NonSerialized] private float _maxFuel;
        
        private GameSession _session;
        

        public float Capacity => _session?.Data.Fuel.Capacity ?? 0;
        public float MaxFuel => _maxFuel;

        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            if (_session == null) 
                return;
            
            _maxFuel = Mathf.Clamp(_session.Data.Fuel.Value, 0, Capacity);
            InvokeFuelChange();
        }

        public void RefillFuel(float fuelChangeValue)
        {
            if (_session == null) 
                return;

            _maxFuel = Mathf.Clamp(_maxFuel + fuelChangeValue, 0, Capacity);
            _session.Data.Fuel.Value = _maxFuel;

            InvokeFuelChange();
        }

        private void InvokeFuelChange()
        {
            onRefill?.Invoke(_maxFuel);
        }

        public void SetFuel(float fuel)
        {
            _maxFuel = fuel;
        }

        [Serializable]
        public class FuelChangeEvent : UnityEvent<float> { }
    }
}