using System;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class Bullet : SpriteObject
    {
        private Vector2 direction;
        private Func<GameObject.Collision, bool> hitMask;
        private Character owner;

        public Bullet(Character owner, SpriteAsset bulletAsset, Vector2 direction, Func<GameObject.Collision, bool> hitMask) :
            base(bulletAsset.Width, bulletAsset.Height, true)
        {
            this.owner = owner;
            CurrentSprite = bulletAsset;
            this.direction = direction;
            this.hitMask = hitMask;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();

            ManageMovement();
            ManageCollisions();
        }

        private void ManageCollisions()
        {
            foreach (var collision in CheckCollisions())
            {
                if (!hitMask(collision)) continue;
                var enemy = collision.Other as Enemy;
                if (enemy != null)
                {
                    owner.DoDamage(enemy);
                }
                Destroy();
            }
        }

        private void ManageMovement()
        {
            X += direction.X*DeltaTime*owner.Stats.RangedSpeed;
            Y += direction.Y*DeltaTime*owner.Stats.RangedSpeed;
        }
    }
}