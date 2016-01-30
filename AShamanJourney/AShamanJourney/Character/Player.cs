
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

        private static readonly Dictionary<string, float> LevelUpModifiers = new Dictionary<string, float>()
        {
            { "attack", 0.1f }, { "maxHp", 0.1f }, { "xpReward", 0.1f },
            { "speed", 0.1f }, { "xpForNextLevel", 0.2f }
        };

        public Player(string name, int width, int height) : base(name, width, height, LevelUpModifiers)
        {
            Stats.Hp = 100f;
            Stats.Speed = 100f;
            Stats.Attack = 100f;
            Stats.XpForNextLevel = 100;
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
        }

        private void LoadAnimations()
        {
            // TODO: add animations
            CurrentSprite = (SpriteAsset) Engine.GetAsset("player");
        }

        public override void Update()
        {
            base.Update();
            ManageInput();
            ManageCamera();
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

        private void ManageCamera()
        {
            // center on the player
            var cameraX = X - Width / 2 - Engine.Width / 2;
            var cameraY = Y - Height / 2 - Engine.Height / 2;
            // check if it's out bounds ??

            Engine.Camera.X = cameraX;
            Engine.Camera.Y = cameraY;
            //Debug.WriteLine($"{Engine.Camera.X},{Engine.Camera.Y}");
        }
    }
}