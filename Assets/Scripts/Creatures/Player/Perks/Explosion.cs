using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    public class Explosion : MonoBehaviour, IPerk
    {
        private Cooldown _cooldown;
        public string Id { get; } = "explosion";

        public void InitializeCooldown()
        {
            _cooldown = DefsFacade.I.Perks.Get(Id).Cooldown;
        }
        
        public void UsePerk(GameObject go)
        {
            var player = go.GetComponent<Player>();
            
            if (!_cooldown.IsReady) 
                return;
            
            _cooldown.Reset();
            player.Particles.Spawn("Explosion");
        }
    }
}