using Scripts.Components.GoBased;
using Scripts.Creatures.Enemies.PatricTheBoss;
using UnityEngine;

public class BossNextStageState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var circularSpawner = animator.GetComponent<CircularProjectileSpawner>();
        var edgeSpawner = animator.GetComponent<EdgeColliderProjectileSpawner>();

        if (circularSpawner.Stage >= edgeSpawner.StageIndex)
            circularSpawner.StopProjectile();
        
        circularSpawner.Stage++;
        
        var changeLight = animator.GetComponent<ChangeLightsComponent>();
        changeLight.SetColor(circularSpawner.Stage);
    }
}
