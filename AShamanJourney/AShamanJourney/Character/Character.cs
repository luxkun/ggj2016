using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public class Character : SpriteObject
    {
        public delegate void DamageTakenEventHandler(object sender, float delta);

        public delegate void HpChangedEventHandler(object sender);

        public delegate void LevelupEventHandler(object sender);

        public delegate void XpChangedEventHandler(object sender, long delta);

        public enum MovingState
        {
            Inactive = 0,
            Idle = 1,
            MovingLeft = 2,
            MovingRight = 3,
            MovingDown = 4,
            MovingUp = 5
        }

        public Character(string name, int width, int height, Dictionary<string, float> levelUpModifiers)
            : base(width, height, true)
        {
            Order = 8;

            Name = name;

            DropManager = new DropManager(this);

            Stats = new Stats(this, levelUpModifiers);
            Stats.Hp = 1f;

            OnDestroy += DestroyEvent;
        }

        public bool IsAlive => Stats.Hp > 0;

        public DropManager DropManager { get; }

        public Vector2 MovingDirection { get; set; }

        public Stats Stats { get; set; }

        public MovingState movingState { get; set; }

        public Character LastHitCharacter { get; private set; }

        public event HpChangedEventHandler OnHpChanged;

        public event LevelupEventHandler OnLevelup;

        public event XpChangedEventHandler OnXpChanged;

        public void XpChanged(long delta)
        {
            OnXpChanged?.Invoke(this, delta);
        }

        public event DamageTakenEventHandler OnDamageTaken;

        private void TookDamage(float delta)
        {
            OnDamageTaken?.Invoke(this, delta);
        }

        public void LevelCheck()
        {
            if (false) // TODO: levelup
            {
                OnLevelup?.Invoke(this);
            }
        }

        public override void Start()
        {
            base.Start();

            LevelCheck();
            movingState = MovingState.Inactive;
            if (Animations != null)
                CurrentAnimation = GetMovingStateString(MovingState.Idle);
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game") return;
            if (movingState == MovingState.Inactive)
            {
                // because we could have inactive minions
                movingState = MovingState.Idle;
            }
            else if (Animations != null)
            {
                CurrentAnimation = GetMovingStateString();
            }
        }

        public Vector2 GetHitCenter()
        {
            return new Vector2(
                X + HitBoxes["auto"].X + HitBoxes["auto"].Width / 2,
                Y + HitBoxes["auto"].Y + HitBoxes["auto"].Height / 2
                );
        }

        private void DestroyEvent(object sender)
        {
        }

        internal void HpChanged()
        {
            OnHpChanged?.Invoke(this);
            Debug.WriteLine($"{Name} hp changed to {Stats.Hp}");
            if (Stats.Hp <= 0)
            {
                DropManager.DropAndSpawn(LastHitCharacter);
                Destroy();
            }
        }

        public virtual bool DoDamage(Character enemy, Damage damage = null)
        {
            if (damage == null)
            {
                // simple (closecombat usually) damage
                damage = new Damage(this, enemy) { DamageFunc = (ch0, ch1) => ch1.Stats.Attack };
            }

            enemy.GetDamage(this, damage);
            if (!enemy.IsAlive)
                Stats.Xp += enemy.Stats.XpReward;
            return enemy.IsAlive;
        }

        protected virtual float GetDamage(Character enemy, Damage damage)
        {
            LevelCheck(); // could happen that the player kills the enemy before he fully spawn (before Start "starts")
            enemy.LevelCheck();
            var dmg = damage.Caculate(this, enemy);

            LastHitCharacter = enemy;
            Stats.Hp -= dmg;

            var floatingText = new FloatingText(this, "-" + (int)dmg, Color.Orange, 0.6f + dmg / 300f);
            Engine.SpawnObject(
                floatingText.Name, floatingText
                );

            // bounce back only if the damage is from a ranged enemy
            if (damage.KnockBack > 0f)
                BounceBack(damage);

            TookDamage(dmg);
            return Stats.Hp;
        }

        private void BounceBack(Damage damage)
        {
            //var force = new Force
            //{
            //    Owner = this,
            //    Direction = damage.Direction,
            //    Step = BounceSpeed / BounceTime,
            //    DestroyTimer = BounceTime * damage.Spell.KnockBack
            //};
            //forces.Add(force);
            //Engine.SpawnObject($"{Name}_bouncebackforce_{forceCount++}", force);
        }

        protected void CalculateMovingState(Vector2 direction)
        {
            direction.Normalize();
            //var cos = Math.Acos(direction.X);
            //var sen = Math.Asin(direction.Y);
            // top or bottom
            var x = direction.X;
            var y = direction.Y;
            if (Math.Abs(y) > Math.Abs(x))
                movingState = y >= 0 ? MovingState.MovingDown : MovingState.MovingUp;
            else
                movingState = x >= 0 ? MovingState.MovingRight : MovingState.MovingLeft;
        }

        public string GetMovingStateString()
        {
            return GetMovingStateString(movingState);
        }

        public static string GetMovingStateString(MovingState state)
        {
            switch (state)
            {
                case MovingState.Idle:
                    return "idle";
                case MovingState.Inactive:
                    return "inactive";
                case MovingState.MovingLeft:
                    return "movingLeft";
                case MovingState.MovingRight:
                    return "movingRight";
                case MovingState.MovingDown:
                    return "movingDown";
                case MovingState.MovingUp:
                default:
                    return "movingUp";
            }
        }

    }
}