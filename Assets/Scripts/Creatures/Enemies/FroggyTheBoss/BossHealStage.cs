using Scripts.Components.Health;
using UnityEngine;

public class BossHealStage : StateMachineBehaviour
{
    [SerializeField] private int _healValue;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<HealthComponent>().ApplyHealthChange(_healValue);
    }
}
