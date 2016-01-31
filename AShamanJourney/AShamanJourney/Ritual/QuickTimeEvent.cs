using System;
using System.Collections.Generic;
using System.Drawing;
using Aiv.Engine;
using Aiv.Fast2D;
using TextObject = Aiv.Engine.TextObject;

namespace AShamanJourney
{
    public class QuickTimeEvent : GameObject
    {
        private readonly List<TextObject> qteList;
        private bool canFail;
        private SpriteObject qteBox;
        private float speed;
        private readonly float speedAccellerator = 0.06f;
        private bool started;

        public QuickTimeEvent(Ritual ritual)
        {
            qteList = new List<TextObject>();

            Ritual = ritual;

            OnDestroy += DestroyEvent;
        }

        public Ritual Ritual { get; set; }

        public float KeyPadding { get; private set; }

        public float Padding { get; set; } = 15f;

        public float FontSize { get; set; } = 0.6f;

        public static KeyCode[] KeyCodeForQte { get; } = {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Up, KeyCode.Right, KeyCode.Down, KeyCode.Left
        };

        public TextObject QteLogo0 { get; private set; }
        public TextObject QteLogo1 { get; private set; }

        public TextObject CurrentKey { get; set; }

        public bool Success { get; set; }

        private void DestroyEvent(object sender)
        {
            var game = (Game) Engine.Objects["game"];
            game.AudioSource.Resume();

            foreach (var qte in qteList.ToArray())
                qte.Destroy();
            qteBox.Destroy();

            Engine.TimeModifier = 1f;
            Ritual.Activated(Success);
        }

        public override void Start()
        {
            base.Start();

            var game = (Game) Engine.Objects["game"];
            game.AudioSource.Pause();

            Engine.TimeModifier = 0f;

            speed = 350f + GameManager.GlobalTimer;
            KeyPadding = 250f;

            AudioSource.Volume = 1f;
            AudioSource.Play(((AudioAsset) Engine.GetAsset("sound_ritual_intro")).Clip);

            QteLogo0 = new TextObject(3f, Color.Crimson)
            {
                Order = 12,
                Text = "RITUAL"
            };
            QteLogo1 = new TextObject(3f, Color.Crimson)
            {
                Order = 12,
                Text = "TIME !"
            };
            var logoMeasure0 = QteLogo0.Measure();
            var logoMeasure1 = QteLogo1.Measure();
            QteLogo0.X = Engine.Width/2 - logoMeasure0.X/2;
            QteLogo0.Y = Engine.Height/2 - (logoMeasure0.Y + logoMeasure1.Y)/2;
            QteLogo1.X = Engine.Width/2 - logoMeasure1.X/2;
            QteLogo1.Y = QteLogo0.Y + logoMeasure0.Y;
            Engine.SpawnObject($"{Name}_logo0", QteLogo0);
            Engine.SpawnObject($"{Name}_logo1", QteLogo1);
            Timer.Set("startQte", 1.5f, ignoreTimeModifier: true);
        }

        private void StartEvent()
        {
            started = true;

            var xPos = 0f;
            var maxHeight = 0f;
            foreach (var key in Utils.RandomKeys((int) (15 + GameManager.GlobalTimer/30f), KeyCodeForQte))
            {
                var text = new TextObject(FontSize, Color.Crimson)
                {
                    Text = $"{KeyToText(key)}",
                    Order = 10,
                    IgnoreCamera = true
                };
                var textMeasure = text.Measure();
                text.X = Engine.Width - textMeasure.X/2f + xPos;
                text.Y = Engine.Height - textMeasure.Y - Padding;
                qteList.Add(text);

                xPos += textMeasure.X + KeyPadding;
                Engine.SpawnObject($"QTE_key_{Name}_{key}", text);
                if (textMeasure.Y > maxHeight)
                    maxHeight = textMeasure.Y;
            }
            var qteBoxAsset = (SpriteAsset) Engine.GetAsset("qteContainer");
            qteBox = new SpriteObject(qteBoxAsset.Width, (int) (qteBoxAsset.Height*0.66f))
            {
                Order = 11,
                CurrentSprite = qteBoxAsset,
                IgnoreCamera = true
            };
            qteBox.X = Engine.Width/2 - qteBox.Width/2;
            qteBox.Y = Engine.Height - qteBox.Height - Padding*0.5f;
            Engine.SpawnObject($"{Name}_qteBox", qteBox);

            AudioSource.Volume = 0.7f;
            AudioSource.Play(((AudioAsset) Engine.GetAsset("sound_ritual_soundtrack")).Clip);
        }

        public override void Update()
        {
            base.Update();
            if (GameManager.MainWindow != "game" || Success) return;
            if (!started)
            {
                if (Timer.Get("startQte") <= 0)
                {
                    StartEvent();
                    QteLogo0.Destroy();
                    QteLogo1.Destroy();
                }
                return;
            }
            speed += speed*speedAccellerator*UnchangedDeltaTime;

            foreach (var item in qteList)
            {
                item.X -= Engine.UnchangedDeltaTime*speed;
            }
            Input();
        }

        private void Input()
        {
            // current pressed keys
            var pressedKeys = new List<string>();
            foreach (KeyCode key in Enum.GetValues(typeof (KeyCode)))
            {
                if (Engine.IsKeyDown(key))
                {
                    pressedKeys.Add(KeyToText(key));
                }
            }
            //if (pressedKeys.Count == 0)
            //    return;

            if (qteList[0].X >= qteBox.X && qteList[0].X <= qteBox.X + qteBox.Width) // success
            {
                canFail = true;
                if (CurrentKey != qteList[0])
                {
                    //qteList[0].Color = Color.AntiqueWhite;
                    qteList[0].Scale *= 2f;
                    qteList[0].Y = Engine.Height - qteList[0].Measure().Y - Padding;
                    CurrentKey = qteList[0];
                }
                if (pressedKeys.Contains(qteList[0].Text))
                {
                    CurrentKey.Destroy();
                    CurrentKey = null;
                    qteList.RemoveAt(0);
                    canFail = false;
                    if (qteList.Count == 0)
                    {
                        Success = true;
                        Destroy();
                    }
                }
                else if (pressedKeys.Count > 0)
                {
                    Success = false;
                    Destroy();
                }
            }
            else if (canFail) // failed hard
            {
                Success = false;
                Destroy();
            }
        }

        private string KeyToText(KeyCode key)
        {
            string result;
            if (key == KeyCode.Up)
                result = "è";
            else if (key == KeyCode.Left)
                result = "ò";
            else if (key == KeyCode.Down)
                result = "à";
            else if (key == KeyCode.Right)
                result = "ù";
            else
                result = key.ToString();
            return result.ToUpper();
        }
    }
}