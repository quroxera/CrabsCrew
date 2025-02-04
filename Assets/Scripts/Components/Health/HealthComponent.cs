using System;
using Scripts.Model;
using Scripts.Model.Definitions.Player;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent onDamage;
        [SerializeField] public UnityEvent onDie;
        [SerializeField] public HealthChangeEvent onChange;
        
        private Lock _immune = new Lock();
        private int _maxHealth;
        
        public int Health => _health;
        public Lock Immune => _immune;

        private void Start()
        {
            _maxHealth = _health;
        }

        public void ApplyHealthChange(int healthChangeValue)
        {
            if (_health <= 0) 
                return;
            
            if (Immune.IsLocked && healthChangeValue < 0)
                return;
            
            if (gameObject.CompareTag("Player"))
                _maxHealth = (int)FindObjectOfType<GameSession>().StatsModel.GetValue(StatId.Hp);
            
            if (_health + healthChangeValue > _maxHealth)
            {
                _health = _maxHealth;
                onChange?.Invoke(_health);
            }

            if (_health + healthChangeValue <= _maxHealth)
            {
                _health += healthChangeValue;
                onChange?.Invoke(_health);
            }
            
            if (healthChangeValue < 0)
                onDamage?.Invoke();
            
            if (_health <= 0)
                onDie?.Invoke();
        }
        
        internal void SetHealth(int health)
        {
            _health = health;
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int> { }
    }
}