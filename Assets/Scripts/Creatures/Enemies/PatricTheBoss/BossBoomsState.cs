using Scripts.Components.GoBased;
using UnityEngine;

public class BossBoomsState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<EdgeColliderProjectileSpawner>();
        spawner.LaunchProjectile();
    }
}
