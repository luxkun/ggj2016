using System;
using System.Drawing;
using System.Net.Mime;
using System.Reflection;
using Aiv.Engine;

namespace AShamanJourney
{
    public class GameOver : GameObject
    {
        public GameOver()
        {
        }

        public override void Start()
        {
            ((Game)Engine.Objects["game"]).AudioSource.Pause();

            GameManager.MainWindow = "gameover";
            Engine.TimeModifier = 0f;

            Timer.Set("limit", 1f, ignoreTimeModifier: true);

            var background = new RectangleObject(Engine.Width, Engine.Height)
            {
                Order = 20, 
                Color = Color.FromArgb(225, 52, 6, 6),
                IgnoreCamera = true,
                Fill = true,
            };
            Engine.SpawnObject("gameOverBg", background);

            var gameOverText = new TextObject(2f, Color.Crimson, 0.9f)
            {
                Order = 20,
                Text = "GAMEOVER"
            };
            var gameOverMeasure = gameOverText.Measure();
            gameOverText.X = Engine.Width/2 - gameOverMeasure.X/2;
            gameOverText.Y = Engine.Height/2 - gameOverMeasure.Y/2;
            gameOverText.IgnoreCamera = true;
            Engine.SpawnObject("gameOverTxt", gameOverText);
        }

        public override void Update()
        {
            base.Update();
            if (Timer.Get("limit") <= 0 && Engine.AnyKeyDown())
            {
                Engine.IsGameRunning = false;

                // Starts a new instance of the program itself
                var fileName = Assembly.GetExecutingAssembly().Location;
                System.Diagnostics.Process.Start(fileName);

                // Closes the current process
                Environment.Exit(0);
            }
        }
    }
}