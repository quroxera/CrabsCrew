using UnityEngine;

namespace Scripts.Creatures.Enemies.PatricTheBoss
{
    public class BossFloodState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<FloodController>();
            spawner.StartFlooding();
        }
    }
}