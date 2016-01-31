using System;
using System.Collections.Generic;

namespace AShamanJourney
{
    public class Stats
    {
        private float hp;
        private float attack;
        private long xpReward;
        private float speed;
        private long xpForNextLevel;
        private long xp;

        public Stats(Character owner, Dictionary<string, float> levelUpModifiers)
        {
            Owner = owner;
            LevelUpModifiers = levelUpModifiers;
        }

        public Dictionary<string, float> LevelUpModifiers { get; set; } 

        public Character Owner { get; set; }

        public float Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                Owner.HpChanged();
            }
        }
        public float MaxHp { get; set; }

        public float Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public long XpReward
        {
            get { return xpReward; }
            set { xpReward = value; }
        }

        public long Xp
        {
            get { return xp; }
            set
            {
                var delta = value - xp;
                xp = value;
                Owner.XpChanged(delta);
                LevelCheck();
            }
        }

        public int Level { get; set; }

        private void LevelCheck()
        {
            if (xp >= XpForNextLevel)
            {
                xp -= XpForNextLevel;
                Level++;
                foreach (var pair in LevelUpModifiers)
                {
                    var s = pair.Key;
                    var m = pair.Value;
                    switch (s)
                    {
                        case "maxHp":
                            MaxHp *= m;
                            break;
                        case "attack":
                            Attack *= m;
                            break;
                        case "xpReward":
                            XpReward = (int) (XpReward * m);
                            break;
                        case "speed":
                            Speed *= m;
                            break;
                        case "xpForNextLevel":
                            XpForNextLevel = (int) (XpForNextLevel * m);
                            break;
                    }

                }
            }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public long XpForNextLevel
        {
            get { return xpForNextLevel; }
            set
            {
                xpForNextLevel = value;
                Owner.XpChanged(0);
            }
        }

        public float AttackSpeed { get; set; }
        public float RangedSpeed { get; set; }

        public Stats Clone()
        {
            return (Stats) MemberwiseClone();
        }
    }
}