using UnityEngine;

namespace Scripts.Creatures.Player.Perks
{
    public interface IPerk
    {
        string Id { get; }
        void InitializeCooldown();
        void UsePerk(GameObject go);
    }
}