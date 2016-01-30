using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using System.Drawing;
using OpenTK;

namespace AShamanJourney.Character
{
    class HUD : GameObject
    {
        public int Padding { get; private set; }
        public int InnerPadding { get; private set; }

        private TextObject hp;
        private TextObject xp;
        private TextObject lvl;
        private TextObject timer;

        private RectangleObject hpBar;
        private RectangleObject xpBar;




        public HUD()
        {
            hp = new TextObject(1f, Color.IndianRed);
            xp = new TextObject(1f, Color.AliceBlue);
            lvl = new TextObject(1f, Color.ForestGreen);
            timer = new TextObject(1f, Color.BurlyWood);

            hpBar = new RectangleObject(100, 20);
            xpBar = new RectangleObject(100, 20);

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

            Engine.SpawnObject("lvl", lvl);
            lvl.X = xp.X;
            lvl.Y = xp.Y + xpMeasure.Y + InnerPadding;
            lvl.IgnoreCamera = true;
            lvl.Text = $"Level: 0";

            Engine.SpawnObject("timer", timer);
            timer.X = lvl.X;
            timer.Y = lvl.Y + lvl.Measure().Y + InnerPadding;
            timer.IgnoreCamera = true;
            timer.Text = $"Global timer: 0s - Local timer: 0s";

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
            lvl.Text = $"Level: {player.Stats.Level}";
        }

        public void UpdateTimer()
        {
            timer.Text = $"Global timer: {GameManager.GlobalTimer} - Local timer: {GameManager.LocalTimer}";
        }
    }
}
