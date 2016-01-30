using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class Enemy : Character
    {
        private Vector2 lastPosition;
        private Vector2 target;
        public float radius;

        public Enemy(string name, int width, int height, Dictionary<string, float> levelUpModifiers) : base(name, width, height, levelUpModifiers)
        {

        }

        public override void Start()
        {
            base.Start();
            LoadAnimations();
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game") return;

            var player = (Player)Engine.Objects["player"];
            Vector2 playerIndex = new Vector2(player.X, player.Y);
            Vector2 enemyIndex = new Vector2(X, Y);

            Vector2 diff = playerIndex - enemyIndex;

            if (diff.Length < radius * 2)
            {
                diff.Normalize();
                diff = diff * Stats.Speed * DeltaTime;
                enemyIndex.X += diff.X;
                enemyIndex.Y += diff.Y;
            }

            ManageCollisions(player);
        }

        private void LoadAnimations()
        {
            // TODO: add animations
            CurrentSprite = (SpriteAsset)Engine.GetAsset("enemy");
        }

        private void ManageCollisions(Player player)
        {
            foreach (var collision in CheckCollisions())
            {
                if (collision.Other.Name == "player")
                    DoDamage(player);
                //else
                //{
                //    Vector2 diff = Vector2.Subtract(new Vector2(X, Y), new Vector2(collision.Other.X, collision.Other.Y));
                //    if (diff.X > 0 && diff.Y > 0)
                //    {
                //        target = new Vector2(target.X - 20, target.Y);
                //    }
                //}
            }
        }
    }
}
