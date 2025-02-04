using Scripts.Model;
using UnityEngine;
using UnityEngine.Analytics;

namespace Scripts.UI.Windows
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int ShowKey = Animator.StringToHash("show");
        private static readonly int HideKey = Animator.StringToHash("hide");
        private float _timeScale;

        protected virtual void Start()
        {
            AnalyticsEvent.ScreenVisit(gameObject.name);
            
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(ShowKey);
            
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            GameSession.IsPaused = true;
        }

        public void Close()
        {
            _animator.SetTrigger(HideKey);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            Time.timeScale = _timeScale;
            GameSession.IsPaused = false;
        }
    }
}