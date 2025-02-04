using System.Collections;
using UnityEngine;

namespace Scripts.Creatures.Enemies
{
    public class ModuleTotemAI : MonoBehaviour
    {
        [SerializeField] private float _attackInterval = 1.0f;
        private ShootingTrapAI[] _traps;

        private void Start()
        {
            _traps = GetComponentsInChildren<ShootingTrapAI>();

            foreach (var trap in _traps)
            {
                trap.IsControlledByModuleTotem = true;
            }

            StartCoroutine(FireTrapsInSequence());
        }

        private IEnumerator FireTrapsInSequence()
        {
            while (true)
            {
                foreach (var trap in _traps)
                {
                    if (trap.HasTargetInRange())
                    {
                        trap.RangeAttack();
                        yield return new WaitForSeconds(_attackInterval);
                    }
                }
                yield return null;
            }
        }
    }
}