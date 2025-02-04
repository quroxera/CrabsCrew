using System.Collections;
using Scripts.Utils.Disposables;
using UnityEngine;

namespace Scripts.Components.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class ImmuneAfterHit : MonoBehaviour
    {
        [SerializeField] private float _immuneTime;
        private HealthComponent _health;
        private Coroutine _coroutine;
        private CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _trash.Retain(_health.onDamage.Subscribe(OnDamage));
        }

        private void OnDamage()
        {
            TryStop();
            
            if (_immuneTime > 0)
                _coroutine = StartCoroutine(MakeImmune());
        }

        private void TryStop()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _coroutine = null;
        }

        private IEnumerator MakeImmune()
        {
            _health.Immune.Retain(this);
            yield return new WaitForSeconds(_immuneTime);
            _health.Immune.Release(this);
        }

        private void OnDestroy()
        {
            TryStop();
            _trash.Dispose();
        }
    }
}