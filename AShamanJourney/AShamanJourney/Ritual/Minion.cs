using System.Collections.Generic;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class Minion : Character
    {
        private MinionType minionType;
        private float healModifier = 0.66f;
        private SpriteAsset bulletAsset;
        private int bulletCounter;
        private float minimumDiff = 200f;
        private float speedModifier = 0.66f;

        public enum MinionType
        {
            Demoniac, Earth, Life
        }
        public Minion(int width, int height, MinionType minionType) : base("minion", width, height, Player.LevelUpModifiers)
        {
            this.minionType = minionType;
            AutomaticHitBox = false;
            Opacity = 0.66f;
            Order = 7;

            minimumDiff = (float) ((0.66 + GameManager.Random.NextDouble())*minimumDiff);
        }

        public override void Start()
        {
            base.Start();
            Player = (Player) Engine.Objects["player"];
            Stats = Player.Stats.Clone();
            Stats.Owner = this;

            switch (minionType)
            { // TODO: different for each type
                case MinionType.Demoniac:
                case MinionType.Earth:
                case MinionType.Life:
                    bulletAsset = (SpriteAsset) Engine.GetAsset("earthMinionBullet");
                    AddAnimation("idle", Utils.GetAssetName("earthMinion", 0, 0), 1);
                    AddAnimation("movingDown", Utils.GetAssetName("earthMinion", 0, 0, 4), 8);
                    AddAnimation("movingLeft", Utils.GetAssetName("earthMinion", 0, 1, 4), 8);
                    AddAnimation("movingRight", Utils.GetAssetName("earthMinion", 0, 2, 4), 8);
                    AddAnimation("movingUp", Utils.GetAssetName("earthMinion", 0, 3, 4), 8);
                    break;
            }
            CurrentAnimation = "idle";
        }

        public Player Player { get; set; }

        public override void Update()
        {
            base.Update();

            switch (minionType)
            {
                case MinionType.Demoniac:
                case MinionType.Earth:
                    AggressiveWorker();
                    break;
                case MinionType.Life:
                    HealingWorker();
                    break;
            }

            FollowPlayer();
        }

        private void FollowPlayer()
        {
            var direction = Player.GetHitCenter() - new Vector2(X, Y);
            if (direction.LengthFast < minimumDiff) { 
                movingState = MovingState.Idle;
                return;
            }
            direction.Normalize();
            X += direction.X * DeltaTime * Stats.Speed * speedModifier;
            Y += direction.Y * DeltaTime * Stats.Speed * speedModifier;
            CalculateMovingState(direction);
        }

        private void AggressiveWorker()
        {
            if (Timer.Get("shotTimer") <= 0) { 
                var myPos = new Vector2(X, Y);
                foreach (var enemy in World.SpawnedObjects[2])
                {
                    var diff = new Vector2(enemy.X, enemy.Y) - myPos;
                    if (diff.LengthFast < RangeRadius) { 
                        Shot(diff.Normalized());
                        break;
                    }
                }
            }
        }

        private void Shot(Vector2 direction)
        {
            Timer.Set("shotTimer", Stats.AttackSpeed);
            var bullet = new Bullet(
                this, bulletAsset, direction, collision => !(collision.Other is Player))
            {
                X = X, Y = Y, Scale = new Vector2(0.33f, 0.33f)
            };
            Engine.SpawnObject($"{Name}_bullet{bulletCounter}", bullet);
        }

        public float RangeRadius { get; set; } = 200f;

        private void HealingWorker()
        {
            if (Timer.Get("healTimer") <= 0)
            {
                if (Player.Stats.Hp < Player.Stats.MaxHp)
                {
                    Timer.Set("healTimer", Stats.AttackSpeed);
                    Player.Stats.Hp += Stats.Attack * healModifier;
                }
            }
        }
    }
}