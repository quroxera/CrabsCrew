using System.Collections;
using Scripts.Model;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Scripts.Components.Fuel
{
    public class FlashlightComponent : MonoBehaviour
    {
        [SerializeField] private float _flashlightIntensity;
        [SerializeField] private float fuelConsumptionRate = 0.1f;
        [SerializeField] private float switchSpeed = 1f;
        
        private Light2D _light;
        private Animator _animator;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _animator = GetComponent<Animator>();
            _light = GetComponentInChildren<Light2D>();
        }

        private IEnumerator SpendingFuel()
        {
            while (_animator.GetBool(FlashlightAnimatorKeys.IsActive))
            {
                var currentFuel = _session.Data.Fuel.Value;

                currentFuel -= fuelConsumptionRate * Time.deltaTime;
                currentFuel = Mathf.Max(currentFuel, 0);

                _session.Data.Fuel.Value = currentFuel;

                if (currentFuel <= 0)
                {
                    OutOfEnergy();
                    yield break;
                }

                var dimThreshold = _session.Data.Fuel.Capacity / 10;

                if (currentFuel <= dimThreshold)
                {
                    var fuelRatio = currentFuel / dimThreshold;
                    _light.intensity = Mathf.Lerp(0, _flashlightIntensity, fuelRatio);
                }
                
                else
                    _light.intensity = _flashlightIntensity;
                
                yield return null;
            }
        }

        public void Switch()
        {
            if (_animator.GetBool(FlashlightAnimatorKeys.IsActive))
            {
                _animator.SetBool(FlashlightAnimatorKeys.IsActive, false);
                StopAllCoroutines();
                StartCoroutine(DecreaseIntensity());
            }
            else
            {
                if (_session.Data.Fuel.Value <= 0)
                {
                    OutOfEnergy();
                    return;
                }

                _animator.SetBool(FlashlightAnimatorKeys.IsActive, true);
                StopAllCoroutines();
                StartCoroutine(IncreaseIntensity());
            }
        }

        private void OutOfEnergy()
        {
            if (!_animator.GetBool(FlashlightAnimatorKeys.IsActive))
                return;

            _animator.SetBool(FlashlightAnimatorKeys.IsActive, false);
            _animator.SetTrigger(FlashlightAnimatorKeys.OutOfFuel);
            StartCoroutine(DecreaseIntensity());
        }

        private IEnumerator IncreaseIntensity()
        {
            while (_light.intensity < _flashlightIntensity)
            {
                _light.intensity += switchSpeed * Time.deltaTime;
                _light.intensity = Mathf.Min(_light.intensity, _flashlightIntensity);
                yield return null;
            }
            StartCoroutine(SpendingFuel());
        }

        private IEnumerator DecreaseIntensity()
        {
            while (_light.intensity > 0f)
            {
                _light.intensity -= switchSpeed * Time.deltaTime;
                _light.intensity = Mathf.Max(_light.intensity, 0f);
                yield return null;
            }
        }
        
        public void OnTurnOn()
        {
            StartCoroutine(IncreaseIntensity());
        }

        public void OnTurnOff()
        {
            StartCoroutine(DecreaseIntensity());
        }
    }
}
