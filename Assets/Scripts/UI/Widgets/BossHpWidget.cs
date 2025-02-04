using Scripts.Components.Health;
using Scripts.Utils;
using Scripts.Utils.Disposables;
using UnityEngine;

namespace Scripts.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private ProgressBarWidget _hpBar;
        [SerializeField] private CanvasGroup _canvas;

        private float _maxHealth;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _maxHealth = _health.Health;
            _trash.Retain(_health.onChange.Subscribe(OnHpChanged));
            _trash.Retain(_health.onDie.Subscribe(HideUI));
        }

        [ContextMenu("Show UI")]
        public void ShowUI()
        {
            this.LerpAnimated(0, 1, 1, SetAlpha);
        }

        private void SetAlpha(float alpha)
        {
           _canvas.alpha = alpha;
        }
        
        [ContextMenu("Hide UI")]
        public void HideUI()
        {
            this.LerpAnimated(1, 0, 1, SetAlpha);
        }

        private void OnHpChanged(int hp)
        {
            _hpBar.SetProgress(hp / _maxHealth);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}