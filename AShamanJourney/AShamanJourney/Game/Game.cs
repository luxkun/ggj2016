using System;
using Aiv.Engine;

namespace AShamanJourney
{
    public class Game : GameObject
    {
        private float minWaveDelay = 20;

        public Game()
        {
        }

        public override void Update()
        {
            base.Update();

            UpdateTimers();
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            var world = (World)Engine.Objects["world"];
            if (Timer.Get("nextWave") <= 0 || World.SpawnedObjects[2].Count <= 0)
            {
                float delay = 90 - GameManager.LocalTimer/60f;
                if (delay < minWaveDelay)
                    delay = minWaveDelay;
                GameManager.Wave++;
                Timer.Set("nextWave", delay);

                for (int i = 0; i < 6 + GameManager.Wave*2; i++)
                {
                    var start = world.calculatedStart;
                    var end = world.calculatedEnd;
                    var xLen = end.X - start.X;
                    var yLen = end.Y - start.Y;
                    var choosenX = start.X + xLen*GameManager.Random.NextDouble();
                    var choosenY = start.Y + yLen * GameManager.Random.NextDouble();
                    while (world.PickRandomObject((int) choosenX, (int) choosenY, 2) == null) { }
                }
            }
            var hud = (Hud) Engine.Objects["hud"];
            hud.UpdateWave();
        }

        private void UpdateTimers()
        {
            GameManager.GlobalTimer += DeltaTime;
            GameManager.LocalTimer += DeltaTime;
            ((Hud)Engine.Objects["hud"]).UpdateTimer();
        }

        public override void Start()
        {
            base.Start();

            GameManager.MainWindow = "game";
            // spawn player
            var player = new Player("player", 250/3, 300/3);
            Engine.SpawnObject(player);

            // spawn world (procederal background + background parts + enemies + rituals)
            var world = new World();
            Engine.SpawnObject("world", world);
        }
    }
}