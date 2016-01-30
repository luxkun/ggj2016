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
        public Enemy bear;
        public Enemy wolf;
        public Enemy boar;

        private static readonly Dictionary<string, float> LevelUpModifiers = new Dictionary<string, float>()
        {
            { "attack", 0.3f }, { "maxHp", 0.2f }, { "xpReward", 0.4f },
            { "speed", 0.1f }, { "xpForNextLevel", 0.1f }
        };


        public void Enemies(Engine engine)
        {
            bear = new Enemy("bear", 70, 40, LevelUpModifiers);
            bear.Stats.Hp = 150f;
            bear.Stats.MaxHp = 200f;
            bear.Stats.Speed = 600f;
            bear.Stats.Attack = 90f;
            bear.Stats.XpForNextLevel = 105;
            bear.radius = 100f;

            wolf = new Enemy("wolf", 50, 20, LevelUpModifiers);
            wolf.Stats.Hp = 90f;
            wolf.Stats.MaxHp = 180f;
            wolf.Stats.Speed = 400f;
            wolf.Stats.Attack = 100f;
            wolf.Stats.XpForNextLevel = 60;
            wolf.radius = 100f;

            boar = new Enemy("boar", 60, 30, LevelUpModifiers);
            boar.Stats.Hp = 200f;
            boar.Stats.MaxHp = 150f;
            boar.Stats.Speed = 450f;
            boar.Stats.Attack = 140f;
            boar.Stats.XpForNextLevel = 90;
            boar.radius = 100f;
        }
    }
}
