using System;
using System.Collections;
using Scripts.Components.GoBased;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    [Serializable]
    public class Dash : MonoBehaviour, IPerk
    {
        [SerializeField] private float _velocity;
        private Cooldown _cooldown;
        public string Id { get; } = "dash";
        
        public bool IsDashing { get; private set; }
        public float Velocity => _velocity;

        public void InitializeCooldown()
        {
            _cooldown = DefsFacade.I.Perks.Get(Id).Cooldown;
        }
        
        public void UsePerk(GameObject go)
        {
            var player = go.GetComponent<Player>();
            if (!_cooldown.IsReady || IsDashing)
                return;
            
            StartCoroutine(DoDash(player.Direction, player.CalculateXVelocity(), player.Particles, player.Rigidbody));
        }

        private IEnumerator DoDash(Vector2 direction, float xVelocity, SpawnListComponent particles, Rigidbody2D rb)
        {
            IsDashing = true;
            _cooldown.Reset();

            const float duration = 0.2f;

            var calculateXVelocity = direction.x * xVelocity;
            rb.AddForce(new Vector2(calculateXVelocity, direction.y), ForceMode2D.Impulse);
            particles.Spawn("Dash");

            yield return new WaitForSeconds(duration);
            IsDashing = false;
        }
    }
}