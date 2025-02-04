using UnityEngine;

namespace Scripts.Components.Health
{
    public class HealthChangeComponent : MonoBehaviour
    {
        [SerializeField] private int _healthChangeValue;
        public int HealthChangeValue
        {
            get => _healthChangeValue;
            set => _healthChangeValue = value;
        }

        public void ApplyHealthChange(GameObject target)
        {
            if (target == null)
                target = GameObject.FindWithTag("Player");

            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null )
            {
                healthComponent.ApplyHealthChange(_healthChangeValue);
            }
        }
    }
}