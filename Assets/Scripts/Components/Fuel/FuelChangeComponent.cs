using Scripts.Creatures.Player;
using UnityEngine;

namespace Scripts.Components.Fuel
{
    public class FuelChangeComponent : MonoBehaviour
    {
        [SerializeField] private float _fuelChangeValue;

        public void ApplyFuelChange(GameObject target)
        {
            if (target == null)
                target = FindObjectOfType<Player>().gameObject;
            
            var fuelComponent = target.GetComponent<FuelComponent>();

            if (fuelComponent != null)
                fuelComponent.RefillFuel(_fuelChangeValue != 0 ? _fuelChangeValue : fuelComponent.Capacity);
        }
    }
}