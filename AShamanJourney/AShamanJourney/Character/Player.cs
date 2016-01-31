
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Aiv.Engine;
using Aiv.Fast2D;
using OpenTK;

namespace AShamanJourney
{
    public class Player : Character
    {
        private Vector2 lastPosition;

        public static readonly Dictionary<string, float> LevelUpModifiers = new Dictionary<string, float>()
        {
            { "attack", 1.1f }, { "maxHp", 1.1f }, { "xpReward", 1.1f },
            { "speed", 1.1f }, { "xpForNextLevel", 1.2f }, { "attackSpeed", 1.1f }
        };

        private readonly int maxCollisions = 10;

        public Player(string name, int width, int height) : base(name, width, height, LevelUpModifiers)
        {
            Stats.Hp = 100f;
            Stats.MaxHp = 100f;
            Stats.Speed = 250f;
            Stats.Attack = 100f;
            Stats.AttackSpeed = 0.8f;
            Stats.XpForNextLevel = 100;
            Stats.RangedSpeed = 300f;
            Order = 9;
        }

        public override void Start()
        {
            base.Start();
            LoadAnimations();

            var hud = (Hud) Engine.Objects["hud"];
            OnHpChanged += sender =>
            {
                hud.UpdateHp((Player) sender);
            };
            OnXpChanged += (sender, delta) => hud.UpdateXp((Player) sender);
            hud.UpdateHp(this);
            hud.UpdateXp(this);
        }

        private void LoadAnimations()
        {
            AddAnimation("idle", Utils.GetAssetName("playerIdle", 0, 0, 7), 8);
            AddAnimation("movingLeft", Utils.GetAssetName("playerIdle", 0, 0, 7), 15);
            AddAnimation("movingRight", Utils.GetAssetName("playerIdle", 0, 0, 7), 15);
            AddAnimation("movingDown", Utils.GetAssetName("playerMovingDown", 0, 0, 4, 2), 15);
            AddAnimation("movingUp", Utils.GetAssetName("playerIdle", 0, 0, 7), 15);
            //AddAnimation("shootingDown", Utils.GetAssetName("playerShootingDown", 0, 0, 7), 15);
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game") return;
            ManageInput();
            ManageCollisions();
        }

        private void ManageInput()
        {
            lastPosition = new Vector2(X, Y);
            // Keys.Right for windows.form (Engine)
            // (int) KeyCode.Right for OpenTK
            // should switch to Keys when game.usingOpenTK is false
            var movingDirection = new Vector2();
            if (Engine.IsKeyDown(KeyCode.D))
                movingDirection.X = 1f;
            if (Engine.IsKeyDown(KeyCode.A))
                movingDirection.X = -1f;
            if (Engine.IsKeyDown(KeyCode.W))
                movingDirection.Y = -1f;
            if (Engine.IsKeyDown(KeyCode.S))
                movingDirection.Y = 1f;

            // joystick controls
            //if (Game.Game.Instance.Joystick != null)
            //{
            //    var joyMovingDirection = new Vector2(
            //        Game.Game.Instance.Joystick.GetAxis(Game.Game.Instance.JoyStickConfig["Lx"]) / 127f,
            //        Game.Game.Instance.Joystick.GetAxis(Game.Game.Instance.JoyStickConfig["Ly"]) / 127f
            //        );
            //    if (joyMovingDirection.Length > 0.2)
            //    {
            //        movingDirection = joyMovingDirection;
            //    }
            //    /*foreach (var button in Game.JoystickButtons)
            //        if (Game.Instance.Joystick.GetButton(Game.Instance.JoyStickConfig[button]))
            //            Debug.WriteLine($"Pressed button: {button}");

            //    for (int index = 0; index < 150; index++)
            //    {
            //        if (Game.Instance.Joystick.GetButton(index))
            //            Debug.WriteLine($"Pressed button: {index}");
            //    }*/
            //}
            if (movingDirection.LengthFast > 0.2f)
            {
                movingDirection.Normalize();
                X += movingDirection.X * DeltaTime * Stats.Speed;
                Y += movingDirection.Y * DeltaTime * Stats.Speed;
                CalculateMovingState(movingDirection);
            }
            else
            {
                movingState = MovingState.Idle;
            }
        }

        private bool ManageCollisions()
        {
            bool collided = false;
            foreach (var collision in CheckCollisions())
            {
                if (!(collision.Other is Ritual) && InCollision > maxCollisions)
                    collided = true;
            }
            if (collided)
            {
                InCollision++;
                X = lastPosition.X;
                Y = lastPosition.Y;
                CalculateMovingState(Vector2.Zero);
            }
            else
            {
                InCollision = 0;
            }
            return collided;
        }

        public int InCollision { get; set; }
    }
}