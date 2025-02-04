using UnityEngine;

namespace Scripts.Utils
{
    public static class AnimatorKeys
    {
        public static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        public static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        public static readonly int IsRunningKey = Animator.StringToHash("is-running");
        public static readonly int HitKey = Animator.StringToHash("hit");
        public static readonly int AttackKey = Animator.StringToHash("attack");
    }

    public static class PlayerAnimatorKeys
    {
        public static readonly int ThrowKey = Animator.StringToHash("throw");
        public static readonly int IsOnWallKey = Animator.StringToHash("is-on-wall");
    }

    public static class FlashlightAnimatorKeys
    {
        public static readonly int IsActive = Animator.StringToHash("isActive");
        public static readonly int OutOfFuel = Animator.StringToHash("outOfFuel");
    }
}