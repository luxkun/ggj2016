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
        private float minimumDiff = 100f;
        private float speedModifier = 0.66f;

        public enum MinionType
        {
            Demoniac, Earth, Life
        }
        public Minion(int width, int height, MinionType minionType) : base("minion", width, height, Player.LevelUpModifiers)
        {
            this.minionType = minionType;
            AutomaticHitBox = false;

            minimumDiff = (float) (0.5f + GameManager.Random.NextDouble()*minimumDiff);
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
                    AddAnimation("movingDown", Utils.GetAssetName("earthMinion", 0, 0, 4), 4);
                    AddAnimation("movingLeft", Utils.GetAssetName("earthMinion", 0, 1, 4), 4);
                    AddAnimation("movingRight", Utils.GetAssetName("earthMinion", 0, 2, 4), 4);
                    AddAnimation("movingUp", Utils.GetAssetName("earthMinion", 0, 3, 4), 4);
                    break;
            }
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
            var direction = new Vector2(Player.X, Player.Y) - new Vector2(X, Y);
            if (direction.LengthFast < minimumDiff)
                return;
            direction.Normalize();
            X += direction.X * DeltaTime * Stats.Speed * speedModifier;
            Y += direction.Y * DeltaTime * Stats.Speed * speedModifier;
        }

        private void AggressiveWorker()
        {
            if (Timer.Get("shotTimer") <= 0) { 
                var myPos = new Vector2(X, Y);
                foreach (var enemy in World.SpawnedObjects[2])
                {
                    var pos = new Vector2(enemy.X, enemy.Y);
                    if ((pos - myPos).LengthFast < RangeRadius) { 
                        Shot(pos.Normalized());
                        break;
                    }
                }
            }
        }

        private void Shot(Vector2 direction)
        {
            Timer.Set("shotTimer", Stats.AttackSpeed);
            var bullet = new Bullet(
                this, bulletAsset, direction, (Collision collision) => !(collision.Other is Player));
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