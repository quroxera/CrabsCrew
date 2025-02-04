using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scripts.UI.HUD
{
    public class DialoguePostEffectsComponent : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private float _switchDuration;
        
        [Header("Post Effects Settings")]
        [SerializeField] private float _targetIntensity;
        [SerializeField] private float _targetSaturation;
        [SerializeField] private float _targetFocalLength;

        private Vignette _vignette;
        private ColorAdjustments _colorAdjustments;
        private DepthOfField _depthOfField;
        
        private Coroutine _fadeCoroutine;
        
        private void Start()
        {
            _volume.profile.TryGet(out _vignette);
            _volume.profile.TryGet(out _colorAdjustments);
            _volume.profile.TryGet(out _depthOfField);
        }

        public void StartFadeToZero()
        {
            StartTransition(FadeValuesToZero);
        }

        public void StartIncreaseToValue()
        {
            StartTransition((x) => IncreaseValuesTo(_targetIntensity, _targetSaturation, _targetFocalLength));
        }

        private void StartTransition(System.Func<float, IEnumerator> transitionMethod)
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(transitionMethod(_switchDuration));
        }

        private IEnumerator FadeValuesToZero(float duration)
        {
            var startValues = GetEffectValues();
            yield return TransitionEffects(startValues, (0, 0, 0), duration);
        }

        private IEnumerator IncreaseValuesTo(float targetIntensity, float targetSaturation, float targetFocalLength)
        {
            var startValues = GetEffectValues();
            yield return TransitionEffects(startValues, (targetIntensity, targetSaturation, targetFocalLength), _switchDuration);
        }

        private IEnumerator TransitionEffects((float intensity, float saturation, float focalLength) startValues,
            (float intensity, float saturation, float focalLength) targetValues, float duration)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                var progress = t / duration;
                
                SetEffectValues(
                    Mathf.Lerp(startValues.intensity, targetValues.intensity, progress),
                    Mathf.Lerp(startValues.saturation, targetValues.saturation, progress),
                    Mathf.Lerp(startValues.focalLength, targetValues.focalLength, progress)
                );
                
                yield return null;
            }
            
            SetEffectValues(targetValues.intensity, targetValues.saturation, targetValues.focalLength);
        }

        private (float intensity, float saturation, float focalLength) GetEffectValues()
        {
            return (_vignette.intensity.value, _colorAdjustments.saturation.value, _depthOfField.focalLength.value);
        }

        private void SetEffectValues(float intensity, float saturation, float focalLength)
        {
            _vignette.intensity.value = intensity;
            _colorAdjustments.saturation.value = saturation;
            _depthOfField.focalLength.value = focalLength;
        }
    }
}