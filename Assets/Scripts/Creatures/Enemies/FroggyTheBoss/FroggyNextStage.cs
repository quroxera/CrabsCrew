using Scripts.Creatures.Enemies.PatricTheBoss;
using UnityEngine;
public class FroggyNextStage : StateMachineBehaviour
{
    [SerializeField] private int _stageIndex;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stageIndex++;

        if (_stageIndex == 1)
        {
            var jump = animator.GetComponent<PlatformJumpComponent>();
            jump.JumpToCentralPlatform();
        }

        animator.GetComponent<ChangeLightsComponent>().SetColor(_stageIndex);
    }
}
