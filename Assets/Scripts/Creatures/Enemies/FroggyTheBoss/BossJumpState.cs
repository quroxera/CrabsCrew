using UnityEngine;

public class BossJumpState : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var platformJumpComponent = animator.GetComponent<PlatformJumpComponent>();
        platformJumpComponent.StartJump();
    }
}
