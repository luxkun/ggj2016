using System.Collections.Generic;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class Enemy : Character
    {
        private bool goingToPlayer;
        private Vector2 lastPosition;
        private readonly float minDiff = 20f;
        private Vector2 target;

        public Enemy(string name, int width, int height, Dictionary<string, float> levelUpModifiers)
            : base(name, width, height, levelUpModifiers)
        {
            goingToPlayer = true;
            Opacity = 0.5f;
        }

        public float RangeRadius { get; set; }

        public override void Start()
        {
            base.Start();
            var player = (Player) Engine.Objects["player"];
            var playerIndex = new Vector2(player.X, player.Y);
            target = playerIndex;
            CurrentAnimation = "idle";
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game") return;
            if (HitBoxes == null) return;

            var player = (Player) Engine.Objects["player"];
            if (player.HitBoxes == null) return;

            // sorry god of programmers, I'm really sorry.
            if (Stats.Hp <= 0)
                Destroy();

            NextMove();
            ManageCollisions();
        }

        private void NextMove()
        {
            lastPosition = new Vector2(X, Y);
            var playerPos = ((Player) Engine.Objects["player"]).GetHitCenter();
            var pos = GetHitCenter();
            var diffPlayer = pos - playerPos;
            if (diffPlayer.LengthFast < RangeRadius*2)
            {
                target = playerPos;
            }
            var direction = target - pos;
            if (direction.LengthFast > minDiff)
            {
                direction.Normalize();
                direction *= Stats.Speed*DeltaTime;
                X += direction.X;
                Y += direction.Y;
                CalculateMovingState(direction);
            }
            else
            {
                movingState = MovingState.Idle;
            }
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
            go.Stats.Owner = go;
            return go;
        }

        private void ManageCollisions()
        {
            foreach (var collision in CheckCollisions())
            {
                var player = collision.Other as Player;
                if (player != null)
                {
                    X = lastPosition.X;
                    Y = lastPosition.Y;
                    if (Timer.Get("lastHit") <= 0)
                    {
                        DoDamage(player);
                        Timer.Set("lastHit", Stats.AttackSpeed);
                    }
                }
                else
                {
                    return;
                    var diff = new Vector2(X, Y) - new Vector2(collision.Other.X, collision.Other.Y);
                    if (diff.X > 0 && diff.Y > 0)
                        if (X + Width <= collision.Other.X) //check right collision
                            target = new Vector2(target.X + collision.Other.HitBoxes[collision.OtherHitBox].Width,
                                target.Y);
                        else //under collision
                            target = new Vector2(target.X,
                                target.Y + collision.Other.HitBoxes[collision.OtherHitBox].Height);
                    if (diff.X > 0 && diff.Y < 0)
                        if (X + Width <= collision.Other.X) //check right collision
                            target = new Vector2(target.X + collision.Other.HitBoxes[collision.OtherHitBox].Width,
                                target.Y);
                        else //under collision
                            target = new Vector2(target.X,
                                target.Y - collision.Other.HitBoxes[collision.OtherHitBox].Height);


                    if (diff.X < 0 && diff.Y > 0)
                        if (X >= collision.Other.X + collision.Other.HitBoxes[collision.OtherHitBox].Width)
                            target = new Vector2(target.X - collision.Other.HitBoxes[collision.OtherHitBox].Width,
                                target.Y);
                        else
                            target = new Vector2(target.X,
                                target.Y - collision.Other.HitBoxes[collision.OtherHitBox].Height);
                    if (diff.X < 0 && diff.Y < 0)
                        if (X >= collision.Other.X + collision.Other.HitBoxes[collision.OtherHitBox].Width)
                            target = new Vector2(target.X - collision.Other.HitBoxes[collision.OtherHitBox].Width,
                                target.Y);
                        else
                            target = new Vector2(target.X,
                                target.Y + collision.Other.HitBoxes[collision.OtherHitBox].Height);
                }
            }
        }
    }
}