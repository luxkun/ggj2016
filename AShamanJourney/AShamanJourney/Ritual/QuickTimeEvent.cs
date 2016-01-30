using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using Aiv.Fast2D;
using System.Drawing;
using OpenTK;
using TextObject = Aiv.Engine.TextObject;

namespace AShamanJourney
{
    public class QuickTimeEvent : GameObject
    {
        private float speed;

        private readonly List<TextObject> qteList;
        private SpriteObject qteBox;
        private bool canFail;

        public QuickTimeEvent(Ritual ritual)
        {
            qteList = new List<TextObject>();

            Ritual = ritual;

            OnDestroy += DestroyEvent;
        }

        private void DestroyEvent(object sender)
        {
            Engine.TimeModifier = 1f;
            Ritual.Activated(Success);
        }

        public Ritual Ritual { get; set; }

        public override void Start()
        {
            base.Start();
            Engine.TimeModifier = 0f;

            speed = 160f + GameManager.GlobalTimer/60f;
            KeyPadding = 300f - GameManager.GlobalTimer / 60f * 3f;

            var xPos = 0f;
            var maxHeight = 0f;
            foreach (var key in Utils.RandomKeys(5 + (int)GameManager.GlobalTimer / 60, KeyCodeForQte))
            {
                var text = new TextObject(FontSize, Color.Crimson)
                {
                    Text = $"{key}",
                    Order = 10
                };
                var textMeasure = text.Measure();
                text.X = Engine.Width - textMeasure.X / 2f + xPos;
                text.Y = Engine.Height - textMeasure.Y - Padding;
                qteList.Add(text);

                xPos += textMeasure.X + KeyPadding;
                Engine.SpawnObject($"QTE_key_{Name}_{key}", text);
                if (textMeasure.Y > maxHeight)
                    maxHeight = textMeasure.Y;
            }
            var qteBoxAsset = (SpriteAsset) Engine.GetAsset("qteContainer");
            qteBox = new SpriteObject(qteBoxAsset.Width, qteBoxAsset.Height)
            {
                Order = 11, 
                CurrentSprite = qteBoxAsset
            };
            qteBox.X = Engine.Width / 2 - qteBox.Width / 2;
            qteBox.Y = Engine.Height - qteBox.Height - Padding;
            Engine.SpawnObject($"{Name}_qteBox", qteBox);
        }

        public float KeyPadding { get; private set; }

        public float Padding { get; set; } = 15f;

        public float FontSize { get; set; } = 0.5f;

        public static KeyCode[] KeyCodeForQte { get; } = {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Up, KeyCode.Right, KeyCode.Down, KeyCode.Left
        };

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game" || Success) return;
            foreach (var item in qteList)
            {
                item.X -= Engine.UnchangedDeltaTime * speed;
            }
            Input();
        }

        private void Input()
        {
            // current pressed keys
            var pressedKeys = new List<string>();
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Engine.IsKeyDown(key))
                    pressedKeys.Add(key.ToString());
            }
            //if (pressedKeys.Count == 0)
            //    return;

            if (qteList[0].X >= qteBox.X && qteList[0].X <= qteBox.X + qteBox.Width) // success
            {
                if (pressedKeys.Contains(qteList[0].Text)) { 
                    qteList.RemoveAt(0);
                    canFail = false;
                    if (qteList.Count == 0)
                    {
                        Success = true;
                        Destroy();
                    }
                } else
                    canFail = true;
            }
            else if (canFail) // failed hard
            {
                Success = false;
                Destroy();
            }
        }

        public bool Success { get; set; }
    }
}
