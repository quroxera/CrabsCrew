using System.Collections.Generic;
using Scripts.Model;
using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    public class PerksController : MonoBehaviour
    {
        private readonly List<IPerk> _perks = new List<IPerk>();
        private GameSession _session;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            
            foreach (var perk in GetComponents<IPerk>())
            {
                _perks.Add(perk);
                perk.InitializeCooldown();
            }
        }

        public IPerk GetCurrentPerk()
        {
            var currentPerkId = _session.PerksModel.Used;
            foreach (var perk in _perks)
            {
                if (perk.Id.Equals(currentPerkId))
                    return perk;
            }
            
            return null;
        }
    }
}