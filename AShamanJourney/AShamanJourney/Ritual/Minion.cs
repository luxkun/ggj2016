﻿using System.Collections.Generic;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class Minion : Character
    {
        private MinionType minionType;
        private float healModifier = 0.66f;
        private float attackModifier = 0.66f;
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
            Opacity = 0.33f;
            Order = 7;

            bulletHitMask = (Collision collision) => !(collision.Other is Player);
            minimumDiff = (float) ((0.66 + GameManager.Random.NextDouble())*minimumDiff);
        }

        public override void Start()
        {
            base.Start();
            Player = (Player) Engine.Objects["player"];
            Stats = Player.Stats.Clone();
            Stats.Owner = this;
            Stats.Attack *= attackModifier;
            Stats.AttackSpeed *= 1 + (1 - attackModifier);

            switch (minionType)
            { // TODO: different for each type
                case MinionType.Demoniac:
                    bulletAsset = (SpriteAsset)Engine.GetAsset("genericBullet");
                    AddAnimation("idle", Utils.GetAssetName("demoniacMinion", 0, 0), 1);
                    AddAnimation("movingDown", Utils.GetAssetName("demoniacMinion", 0, 0, 4), 8);
                    AddAnimation("movingLeft", Utils.GetAssetName("demoniacMinion", 0, 1, 4), 8);
                    AddAnimation("movingRight", Utils.GetAssetName("demoniacMinion", 0, 2, 4), 8);
                    AddAnimation("movingUp", Utils.GetAssetName("demoniacMinion", 0, 3, 4), 8);
                    break;
                case MinionType.Earth:
                    bulletAsset = (SpriteAsset)Engine.GetAsset("genericBullet");
                    AddAnimation("idle", Utils.GetAssetName("earthMinion", 0, 0), 1);
                    AddAnimation("movingDown", Utils.GetAssetName("earthMinion", 0, 0, 4), 8);
                    AddAnimation("movingLeft", Utils.GetAssetName("earthMinion", 0, 1, 4), 8);
                    AddAnimation("movingRight", Utils.GetAssetName("earthMinion", 0, 2, 4), 8);
                    AddAnimation("movingUp", Utils.GetAssetName("earthMinion", 0, 3, 4), 8);
                    break;
                case MinionType.Life:
                    bulletAsset = (SpriteAsset)Engine.GetAsset("genericBullet");
                    AddAnimation("idle", Utils.GetAssetName("lifeMinion", 0, 0), 1);
                    AddAnimation("movingDown", Utils.GetAssetName("lifeMinion", 0, 0, 4), 8);
                    AddAnimation("movingLeft", Utils.GetAssetName("lifeMinion", 0, 1, 4), 8);
                    AddAnimation("movingRight", Utils.GetAssetName("lifeMinion", 0, 2, 4), 8);
                    AddAnimation("movingUp", Utils.GetAssetName("lifeMinion", 0, 3, 4), 8);
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