using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    public class DoubleJump : MonoBehaviour, IPerk
    {
        private Cooldown _cooldown;
        public string Id { get; } = "double-jump";
        public bool AllowDoubleJump { get; private set; }

        public void InitializeCooldown()
        {
            _cooldown = DefsFacade.I.Perks.Get(Id).Cooldown;
        }

        public void UsePerk(GameObject go)
        {
            var player = go.GetComponent<Player>();
            if (!_cooldown.IsReady || !AllowDoubleJump)
                return;
            
            _cooldown.Reset();
            AllowDoubleJump = false;
            
            player.DoJumpVfx();
        }

        public void ResetDoubleJump() => AllowDoubleJump = true;
    }
}