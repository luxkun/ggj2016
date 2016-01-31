using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;

namespace AShamanJourney
{
    public class EnemyInfo
    {
        public static Enemy bear;
        public static Enemy wolf;
        public static Enemy boar;

        private static readonly Dictionary<string, float> LevelUpModifiers = new Dictionary<string, float>()
        {
            { "attack", 0.3f }, { "maxHp", 0.2f }, { "xpReward", 0.4f },
            { "speed", 0.1f }, { "xpForNextLevel", 0.1f }
        };


        public static void Initialize(Engine engine)
        {
            bear = new Enemy("bear", 70, 40, LevelUpModifiers)
            {
                Stats =
                {
                    Hp = 150f,
                    MaxHp = 200f,
                    Speed = 100f,
                    Attack = 90f,
                    XpForNextLevel = 105,
                    AttackSpeed = 1.3f
                },
                RangeRadius = 1000f
            };

            wolf = new Enemy("wolf", 50, 20, LevelUpModifiers)
            {
                Stats =
                {
                    Hp = 90f,
                    MaxHp = 180f,
                    Speed = 150f,
                    Attack = 100f,
                    XpForNextLevel = 60,
                    AttackSpeed = 1f
                },
                RangeRadius = 1000f
            };

            boar = new Enemy("boar", 60, 30, LevelUpModifiers)
            {
                Stats =
                {
                    Hp = 200f,
                    MaxHp = 150f,
                    Speed = 100f,
                    Attack = 140f,
                    XpForNextLevel = 90,
                    AttackSpeed = 1.3f
                },
                RangeRadius = 1000f
            };

            AddAnimations(engine);
        }

        private static void AddAnimations(Engine engine)
        {
            bear.AddAnimation("idle", Utils.GetAssetName("bear", 0, 0), 5, engine);
            bear.AddAnimation("movingLeft", Utils.GetAssetName("bear", 0, 1, 3), 5, engine);
            bear.AddAnimation("movingRight", Utils.GetAssetName("bear", 0, 2, 3), 5, engine);
            bear.AddAnimation("movingUp", Utils.GetAssetName("bear", 0, 3, 3), 5, engine);
            bear.AddAnimation("movingDown", Utils.GetAssetName("bear", 0, 0, 3), 5, engine);
        }
    }
}
