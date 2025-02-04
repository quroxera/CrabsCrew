using UnityEngine;

namespace Scripts.Creatures.Weapons
{
    public class DirectionalProjectile : BaseProjectile
    {
        public void Launch(Vector2 direction)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
        }
    }
}