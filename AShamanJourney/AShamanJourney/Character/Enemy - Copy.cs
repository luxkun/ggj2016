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
        public float RangeRadius { get; set; }
        private bool goingToPlayer;

        public Enemy(string name, int width, int height, Dictionary<string, float> levelUpModifiers) : base(name, width, height, levelUpModifiers)
        {
            goingToPlayer = true;
        }

        public override void Start()
        {
            base.Start();
            var player = (Player)Engine.Objects["player"];
            Vector2 playerIndex = new Vector2(player.X, player.Y);
            target = playerIndex;
            CurrentAnimation = "idle";
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game") return;

            var player = (Player)Engine.Objects["player"];
            if (player.HitBoxes == null) return;
            Vector2 playerIndex = player.GetHitCenter();
            Vector2 enemyIndex = new Vector2(X, Y);
            
            if (goingToPlayer) // check if the enemy is going to player, after refresh the current player position
                target = playerIndex;

            Vector2 diff = target - enemyIndex;

            if (diff.Length < RangeRadius * 2)
            {
                diff.Normalize();
                diff = diff * Stats.Speed * DeltaTime;
                enemyIndex.X += diff.X;
                enemyIndex.Y += diff.Y;
            }
            if (diff.Length < 15)  //if the enemy is near target
            {
                X = target.X;
                Y = target.Y;
                target = playerIndex;
            }

            ManageCollisions(player);
        }

        public override GameObject Clone()
        {
            var go = new Enemy(Name, (int) Width, (int) Height, Stats.LevelUpModifiers);
            if (Animations != null)
            {
                go.Animations = new Dictionary<string, Animation>();
                foreach (var animKey in Animations.Keys)
                {
                    go.Animations[animKey] = Animations[animKey].Clone();
                    go.Animations[animKey].Owner = go;
                }
            }
            go.RangeRadius = RangeRadius;
            go.Stats = Stats.Clone();
            return go;
        }

        private void ManageCollisions(Player player)
        {
            foreach (var collision in CheckCollisions())
            {
                if (collision.Other is Player) { 
                    if (Timer.Get("lastHit") <= 0) { 
                        DoDamage(player);
                        Timer.Set("lastHit", Stats.AttackSpeed);
                    }
                } else
                {
                    Vector2 diff = new Vector2(X, Y) - new Vector2(collision.Other.X, collision.Other.Y);
                    if ((diff.X > 0 && diff.Y > 0)) 
                        if (X + Width <= collision.Other.X ) //check right collision
                            target = new Vector2(target.X + collision.Other.HitBoxes[collision.OtherHitBox].Width, target.Y);
                        else //under collision
                            target = new Vector2(target.X, target.Y + collision.Other.HitBoxes[collision.OtherHitBox].Height);
                    if((diff.X > 0 && diff.Y < 0))
                        if (X + Width <= collision.Other.X) //check right collision
                            target = new Vector2(target.X + collision.Other.HitBoxes[collision.OtherHitBox].Width, target.Y);
                        else //under collision
                            target = new Vector2(target.X, target.Y - collision.Other.HitBoxes[collision.OtherHitBox].Height);


                    if ((diff.X < 0 && diff.Y > 0))
                        if(X >= collision.Other.X + collision.Other.HitBoxes[collision.OtherHitBox].Width)
                            target = new Vector2(target.X - collision.Other.HitBoxes[collision.OtherHitBox].Width, target.Y);
                        else
                            target = new Vector2(target.X, target.Y - collision.Other.HitBoxes[collision.OtherHitBox].Height);
                    if ((diff.X < 0 && diff.Y < 0))
                        if (X >= collision.Other.X + collision.Other.HitBoxes[collision.OtherHitBox].Width)
                            target = new Vector2(target.X - collision.Other.HitBoxes[collision.OtherHitBox].Width, target.Y);
                        else
                            target = new Vector2(target.X, target.Y + collision.Other.HitBoxes[collision.OtherHitBox].Height);
                }
            }
            
        }
    }
}
