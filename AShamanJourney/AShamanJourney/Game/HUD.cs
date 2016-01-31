using System;
using System.Drawing;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    internal class Hud : GameObject
    {
        private readonly TextObject hp;

        private RectangleObject hpBar;
        private RectangleObject hpBarBorder;
        private readonly TextObject hpNumber;
        private readonly TextObject lvl;
        private readonly TextObject timer;
        private readonly TextObject wave;
        private readonly TextObject waveTimer;
        private readonly TextObject xp;
        private RectangleObject xpBar;
        private RectangleObject xpBarBorder;
        private readonly TextObject xpNumber;


        public Hud()
        {
            var fontSize = 0.4f;
            hp = new TextObject(fontSize, Color.IndianRed);
            hp.Order = 10;
            xp = new TextObject(fontSize, Color.DarkBlue);
            xp.Order = 10;
            lvl = new TextObject(fontSize, Color.ForestGreen);
            lvl.Order = 10;
            timer = new TextObject(fontSize, Color.BurlyWood);
            timer.Order = 10;
            wave = new TextObject(fontSize, Color.BlanchedAlmond);
            wave.Order = 10;
            waveTimer = new TextObject(fontSize, Color.Beige);
            waveTimer.Order = 10;
            hpNumber = new TextObject(fontSize/2, Color.DarkGray);
            hpNumber.Order = 11;
            xpNumber = new TextObject(fontSize/2, Color.DarkGray);
            xpNumber.Order = 11;

            Padding = 20;
            InnerPadding = Padding/2;
        }

        public int Padding { get; }
        public int InnerPadding { get; }

        public override void Start()
        {
            base.Start();

            Engine.SpawnObject("hpNumber", hpNumber);
            hpNumber.IgnoreCamera = true;

            Engine.SpawnObject("hp", hp);
            hp.X = Padding;
            hp.Y = Padding;
            hp.IgnoreCamera = true;
            hp.Text = "HP";

            Engine.SpawnObject("xpNumber", xpNumber);
            xpNumber.IgnoreCamera = true;

            var hpMeasure = hp.Measure();
            Engine.SpawnObject("xp", xp);
            xp.X = hp.X;
            xp.Y = hp.Y + hpMeasure.Y + InnerPadding;
            xp.IgnoreCamera = true;
            xp.Text = "XP";
            var xpMeasure = xp.Measure();

            //WAVE
            Engine.SpawnObject("wave", wave);
            wave.IgnoreCamera = true;

            Engine.SpawnObject("waveTimer", waveTimer);
            waveTimer.IgnoreCamera = true;
            //

            hpBar = new RectangleObject(150, (int) hpMeasure.Y)
            {
                Fill = true,
                Order = 10
            };
            xpBar = new RectangleObject(150, (int) xpMeasure.Y)
            {
                Fill = true,
                Order = 10
            };

            hpBarBorder = new RectangleObject(150, (int) hpMeasure.Y)
            {
                Order = 10
            };
            xpBarBorder = new RectangleObject(150, (int) xpMeasure.Y)
            {
                Order = 10
            };

            Engine.SpawnObject("lvl", lvl);
            lvl.X = xp.X;
            lvl.Y = xp.Y + xpMeasure.Y + InnerPadding;
            lvl.IgnoreCamera = true;
            lvl.Text = $"Level 1";

            Engine.SpawnObject("timer", timer);
            timer.X = lvl.X;
            timer.Y = lvl.Y + lvl.Measure().Y + InnerPadding;
            timer.IgnoreCamera = true;
            timer.Text = $"Timer 0";

            Engine.SpawnObject("hpBar", hpBar);
            hpBar.X = hp.X + hpMeasure.X + InnerPadding;
            hpBar.Y = hp.Y;
            hpBar.Color = Color.IndianRed;
            hpBar.IgnoreCamera = true;

            Engine.SpawnObject("xpBar", xpBar);
            xpBar.X = xp.X + xpMeasure.X + InnerPadding;
            xpBar.Y = xp.Y;
            xpBar.Color = Color.DarkBlue;
            xpBar.IgnoreCamera = true;

            Engine.SpawnObject("hpBarBorder", hpBarBorder);
            hpBarBorder.X = hp.X + hpMeasure.X + InnerPadding;
            hpBarBorder.Y = hp.Y;
            hpBarBorder.Color = Color.Black;
            hpBarBorder.IgnoreCamera = true;

            Engine.SpawnObject("xpBarBorder", xpBarBorder);
            xpBarBorder.X = xp.X + xpMeasure.X + InnerPadding;
            xpBarBorder.Y = xp.Y;
            xpBarBorder.Color = Color.Black;
            xpBarBorder.IgnoreCamera = true;
        }

        public void UpdateHp(Player player)
        {
            hpBar.Box.scale = new Vector2((float) Math.Round(player.Stats.Hp/player.Stats.MaxHp, 2), 1f);
            hpNumber.Text = $"{(int) player.Stats.Hp} - {(int) player.Stats.MaxHp}";
            var hpNumberMeasure = hpNumber.Measure();
            hpNumber.X = hpBarBorder.X + hpBarBorder.Width/2 - hpNumberMeasure.X/2;
            hpNumber.Y = hpBarBorder.Y + hpBarBorder.Height/2 - hpNumberMeasure.Y/2;
        }

        public void UpdateXp(Player player)
        {
            xpBar.Box.scale = new Vector2((float) Math.Round((float) player.Stats.Xp/player.Stats.XpForNextLevel, 2), 1f);
            xpNumber.Text = $"{player.Stats.Xp} - {player.Stats.XpForNextLevel}";
            var xpNumberMeasure = xpNumber.Measure();
            xpNumber.X = xpBarBorder.X + xpBarBorder.Width/2 - xpNumberMeasure.X/2;
            xpNumber.Y = xpBarBorder.Y + xpBarBorder.Height/2 - xpNumberMeasure.Y/2;
            lvl.Text = $"Level {player.Stats.Level + 1}";
        }

        public void UpdateTimer()
        {
            timer.Text = $"Timer {(int) GameManager.LocalTimer} - {(int) GameManager.GlobalTimer}";
        }

        public void UpdateWave()
        {
            wave.Text = $"Wave {GameManager.Wave}";
            var waveMeasure = wave.Measure();
            wave.X = Engine.Width - waveMeasure.X - Padding;
            wave.Y = Padding;

            waveTimer.Text = $"Next Wave {(int) ((Game) Engine.Objects["game"]).Timer.Get("nextWave")}s";
            var waveTimerMeasure = waveTimer.Measure();
            waveTimer.X = Engine.Width - waveTimerMeasure.X - Padding;
            waveTimer.Y = wave.Y + waveMeasure.Y + Padding;
        }
    }
}