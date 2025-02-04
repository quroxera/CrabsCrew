using Scripts.Components.Health;
using Scripts.Utils.Disposables;
using UnityEngine;

namespace Scripts.UI.Widgets
{
    public class HealthBarWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        private HealthComponent _health;
        private int _maxHealth;

        private Transform _parentTransform;
        private Vector3 _initialScale;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _health = GetComponentInParent<HealthComponent>();
            _maxHealth = _health.Health;

            _parentTransform = _health.transform;
            _initialScale = transform.localScale;

            _trash.Retain(_health.onChange.Subscribe(OnHealthChanged));
            _trash.Retain(_health.onDie.Subscribe(OnDie));
        }

        private void LateUpdate()
        {
            var parentScale = _parentTransform.localScale;
            transform.localScale = new Vector3(
                Mathf.Sign(parentScale.x) * Mathf.Abs(_initialScale.x),
                _initialScale.y,
                _initialScale.z
            );
        }

        private void OnHealthChanged(int health)
        {
            var value = (float) health / _maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}