using System;
using System.Collections;
using Scripts.Components.GoBased;
using Scripts.Components.Health;
using Scripts.Model;
using Scripts.Model.Definitions.ItemsDef;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    [Serializable]
    public class MagicShield : MonoBehaviour, IPerk
    {
        [SerializeField] private float _duration;
        private Cooldown _cooldown;
        public string Id { get; } = "magic-shield";
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void InitializeCooldown()
        {
            _cooldown = DefsFacade.I.Perks.Get(Id).Cooldown;
        }
        
        public void UsePerk(GameObject go)
        {
            var player = go.GetComponent<Player>();
            if (_session.PerksModel.IsMagicShieldSupported && _cooldown.IsReady)
                StartCoroutine(ActivateMagicShield(player.Health, player.Particles, go));
        }
        
        private IEnumerator ActivateMagicShield(HealthComponent health, SpawnListComponent particles, GameObject go)
        {
            _cooldown.Reset();
            health.Immune.Retain(go);
            particles.Spawn("MagicShield", _duration);

            yield return new WaitForSeconds(_duration);
            health.Immune.Release(go);
        }
    }
}