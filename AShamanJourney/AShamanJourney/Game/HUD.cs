using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using System.Drawing;
using OpenTK;

namespace AShamanJourney
{
    class Hud : GameObject
    {
        public int Padding { get; private set; }
        public int InnerPadding { get; private set; }

        private TextObject hp;
        private TextObject xp;
        private TextObject lvl;
        private TextObject timer;

        private RectangleObject hpBar;
        private RectangleObject xpBar;




        public Hud()
        {
            var fontSize = 0.4f;
            hp = new TextObject(fontSize, Color.IndianRed);
            xp = new TextObject(fontSize, Color.AliceBlue);
            lvl = new TextObject(fontSize, Color.ForestGreen);
            timer = new TextObject(fontSize, Color.BurlyWood);

            Padding = 20;
            InnerPadding = Padding / 2;
        }

        public override void Start()
        {
            base.Start();

            Engine.SpawnObject("hp", hp);
            hp.X = Padding;
            hp.Y = Padding;
            hp.IgnoreCamera = true;
            hp.Text = "HP";
            var hpMeasure = hp.Measure();

            Engine.SpawnObject("xp", xp);
            xp.X = hp.X;
            xp.Y = hp.Y + hpMeasure.Y + InnerPadding;
            xp.IgnoreCamera = true;
            xp.Text = "XP";
            var xpMeasure = xp.Measure();

            hpBar = new RectangleObject(150, (int) hpMeasure.Y)
            {
                Fill = true
            };
            xpBar = new RectangleObject(150, (int) xpMeasure.Y)
            {
                Fill = true
            };

            Engine.SpawnObject("lvl", lvl);
            lvl.X = xp.X;
            lvl.Y = xp.Y + xpMeasure.Y + InnerPadding;
            lvl.IgnoreCamera = true;
            lvl.Text = $"Level 0";

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
        }

        public void UpdateHP(Player player)
        {
            hpBar.Scale = new Vector2(player.Stats.Hp / player.Stats.MaxHp, 1f);
        }

        public void UpdateXP(Player player)
        {
            xpBar.Scale = new Vector2(player.Stats.Xp / player.Stats.XpForNextLevel, 1f);
            lvl.Text = $"Level {player.Stats.Level}";
        }

        public void UpdateTimer()
        {
            timer.Text = $"Timer {GameManager.LocalTimer} / {GameManager.GlobalTimer}";
        }
    }
}
