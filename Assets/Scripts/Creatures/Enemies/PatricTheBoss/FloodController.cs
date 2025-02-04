using System.Collections;
using UnityEngine;

namespace Scripts.Creatures.Enemies.PatricTheBoss
{
    public class FloodController : MonoBehaviour
    {
        [SerializeField] private Animator _floodAnimator;
        [SerializeField] private float _floodTime;

        private Coroutine _floodCoroutine;
        private static readonly int IsFlooding = Animator.StringToHash("isFlooding");

        public void StartFlooding()
        {
            if (_floodCoroutine != null)
                return;
            
            _floodCoroutine = StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _floodAnimator.SetBool(IsFlooding, true);
            yield return new WaitForSeconds(_floodTime);
            _floodAnimator.SetBool(IsFlooding, false);
            _floodCoroutine = null;
        }
    }
}