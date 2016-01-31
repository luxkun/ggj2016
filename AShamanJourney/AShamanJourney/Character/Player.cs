
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
            Stats.Hp = 500f;
            Stats.MaxHp = 500f;
            Stats.Speed = 250f;
            Stats.Attack = 40f;
            Stats.AttackSpeed = 0.8f;
            Stats.XpForNextLevel = 100;
            Stats.RangedSpeed = 300f;
            Order = 9;

            bulletHitMask = (Collision collision) => !(collision.Other is Player);
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

            bulletAsset = (SpriteAsset)Engine.GetAsset("genericBullet");
        }

        private void LoadAnimations()
        {
            var fps0 = 8;
            var fps1 = 12;

            var idle = Utils.GetAssetName("playerIdle", 0, 0, 4);
            idle.AddRange(Utils.GetAssetName("playerIdle", 0, 1, 3));
            AddAnimation("idle", idle, fps0);

            var movingLeft = Utils.GetAssetName("playerMovingLeft", 0, 0, 4);
            movingLeft.AddRange(Utils.GetAssetName("playerMovingLeft", 0, 1));
            AddAnimation("movingLeft", movingLeft, fps0);

            var movingRight = Utils.GetAssetName("playerMovingRight", 0, 0, 4);
            movingRight.AddRange(Utils.GetAssetName("playerMovingRight", 3, 1));
            AddAnimation("movingRight", movingRight, fps0);

            var movingDown = Utils.GetAssetName("playerMovingDown", 0, 0, 4);
            movingDown.AddRange(Utils.GetAssetName("playerMovingDown", 0, 1, 3));
            AddAnimation("movingDown", movingDown, fps1);

            var movingUp = Utils.GetAssetName("playerMovingUp", 0, 0, 4);
            movingUp.AddRange(Utils.GetAssetName("playerMovingUp", 0, 1, 3));
            AddAnimation("movingUp", movingUp, fps1);
            //AddAnimation("shottingLeft", Utils.GetAssetName("playerIdle", 0, 0, 7), 15);
            //AddAnimation("shottingRight", Utils.GetAssetName("playerIdle", 0, 0, 7), 15);
            //AddAnimation("shottingDown", Utils.GetAssetName("playerMovingDown", 0, 0, 4, 2), 15);
            //AddAnimation("shottingUp", Utils.GetAssetName("playerMovingUp", 0, 0, 7), 15);
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game" || Engine.TimeModifier != 1f) return;
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

            var shottingDirection = new Vector2();
            if (Engine.IsKeyDown(KeyCode.Right))
                shottingDirection.X = 1f;
            if (Engine.IsKeyDown(KeyCode.Left))
                shottingDirection.X = -1f;
            if (Engine.IsKeyDown(KeyCode.Up))
                shottingDirection.Y = -1f;
            if (Engine.IsKeyDown(KeyCode.Down))
                shottingDirection.Y = 1f;
            if (shottingDirection.LengthFast > 0.2f)
            {
                Shot(shottingDirection);
            }
        }

        private bool ManageCollisions()
        {
            bool collided = false;
            foreach (var collision in CheckCollisions())
            {
                if (!(collision.Other is Ritual) && !(collision.Other is Enemy))// && InCollision > maxCollisions)
                    collided = true;
            }
            var world = (World) Engine.Objects["world"];
            if (collided ||
                X < world.calculatedStart.X || Y < world.calculatedStart.Y ||
                X + Width > world.calculatedEnd.X || Y + Height > world.calculatedEnd.Y)
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