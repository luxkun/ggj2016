﻿using System.Collections.Generic;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class EnemyInfo
    {
        public static Enemy bear;
        public static Enemy wolf;
        public static Enemy rhyno;

        private static readonly Dictionary<string, float> LevelUpModifiers = new Dictionary<string, float>
        {
            {"attack", 0.3f},
            {"maxHp", 0.2f},
            {"xpReward", 0.4f},
            {"speed", 0.1f},
            {"xpForNextLevel", 0.1f}
        };


        public static void Initialize(Engine engine)
        {
            bear = new Enemy("bear", 70, 40, LevelUpModifiers)
            {
                Stats =
                {
                    MaxHp = 200f,
                    Speed = 100f,
                    Attack = 120f,
                    XpReward = 30,
                    XpForNextLevel = 60,
                    AttackSpeed = 1.3f
                },
                RangeRadius = 1000f,
                Scale = new Vector2(1.5f, 1.5f)
            };

            wolf = new Enemy("wolf", 50, 20, LevelUpModifiers)
            {
                Stats =
                {
                    MaxHp = 100f,
                    Speed = 150f,
                    Attack = 100f,
                    XpReward = 20,
                    XpForNextLevel = 60,
                    AttackSpeed = 0.66f
                },
                RangeRadius = 1000f,
                Scale = new Vector2(1.75f, 1.75f)
            };

            rhyno = new Enemy("rhyno", 60, 30, LevelUpModifiers)
            {
                Stats =
                {
                    MaxHp = 300f,
                    Speed = 100f,
                    Attack = 140f,
                    XpReward = 30,
                    XpForNextLevel = 60,
                    AttackSpeed = 1f
                },
                RangeRadius = 1000f,
                Scale = new Vector2(2.5f, 2.5f)
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

            wolf.AddAnimation("idle", Utils.GetAssetName("wolf", 0, 0), 5, engine);
            wolf.AddAnimation("movingLeft", Utils.GetAssetName("wolf", 0, 1, 3), 5, engine);
            wolf.AddAnimation("movingRight", Utils.GetAssetName("wolf", 0, 2, 3), 5, engine);
            wolf.AddAnimation("movingUp", Utils.GetAssetName("wolf", 0, 3, 3), 5, engine);
            wolf.AddAnimation("movingDown", Utils.GetAssetName("wolf", 0, 0, 3), 5, engine);

            rhyno.AddAnimation("idle", Utils.GetAssetName("rhyno", 0, 0), 5, engine);
            rhyno.AddAnimation("movingLeft", Utils.GetAssetName("rhyno", 0, 1, 3), 5, engine);
            rhyno.AddAnimation("movingRight", Utils.GetAssetName("rhyno", 0, 2, 3), 5, engine);
            rhyno.AddAnimation("movingUp", Utils.GetAssetName("rhyno", 0, 3, 3), 5, engine);
            rhyno.AddAnimation("movingDown", Utils.GetAssetName("rhyno", 0, 0, 3), 5, engine);
        }
    }
}