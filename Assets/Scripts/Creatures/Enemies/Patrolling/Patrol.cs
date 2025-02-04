using System.Collections;
using UnityEngine;

namespace Scripts.Creatures.Enemies.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}