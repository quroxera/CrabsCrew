﻿using UnityEngine;

namespace Scripts.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
            var force = new Vector2(Direction * Speed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse); 
        }

        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += Direction * Speed;
            Rigidbody.MovePosition(position);
        }
    }
}