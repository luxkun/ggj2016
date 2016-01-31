using Aiv.Engine;
using Aiv.Fast2D;

namespace AShamanJourney
{
    public class Game : GameObject
    {
        private readonly float minWaveDelay = 20;

        public override void Update()
        {
            base.Update();

            UpdateTimers();
            if (GameManager.GlobalTimer > 10f)
                SpawnEnemies();
            ManageControls();
        }

        private void ManageControls()
        {
            // window change inputs
            if (Timer.Get("changeWindowTimer") > 0) return;
            if (Engine.IsKeyDown(KeyCode.Esc))
            {
                Timer.Set("changeWindowTimer", 1f, ignoreTimeModifier: true);
                if (GameManager.MainWindow == "pause")
                    UnPause();
                else
                    Pause();
            }
        }

        private void UnPause()
        {
            GameManager.MainWindow = "game";
            AudioSource.Resume();
            Engine.TimeModifier = 1f;
        }

        private void Pause()
        {
            GameManager.MainWindow = "pause";
            AudioSource.Pause();
            Engine.TimeModifier = 0f;
        }

        private void SpawnEnemies()
        {
            var world = (World) Engine.Objects["world"];
            if (Timer.Get("nextWave") <= 0 || World.SpawnedObjects[2].Count <= 0)
            {
                var delay = 90 - GameManager.LocalTimer/60f;
                if (delay < minWaveDelay)
                    delay = minWaveDelay;
                GameManager.Wave++;
                Timer.Set("nextWave", delay);

                for (var i = 0; i < 8 + GameManager.Wave*3; i++)
                {
                    var start = world.calculatedStart;
                    var end = world.calculatedEnd;
                    var xLen = end.X - start.X;
                    var yLen = end.Y - start.Y;
                    int choosenX;
                    int choosenY;
                    do
                    {
                        choosenX = (int) (start.X + xLen*GameManager.Random.NextDouble());
                        choosenY = (int) (start.Y + yLen*GameManager.Random.NextDouble());
                    } while (world.PickRandomObject(choosenX, choosenY, 2) == null);
                }
            }
        }

        private void UpdateTimers()
        {
            GameManager.GlobalTimer += DeltaTime;
            GameManager.LocalTimer += DeltaTime;
            var hud = (Hud) Engine.Objects["hud"];
            hud.UpdateTimer();
            hud.UpdateWave();
        }

        public override void Start()
        {
            base.Start();

            AudioSource.Volume = 0.4f;
            AudioSource.Stream(Engine.GetAsset("sound_soundtrack").FileName, true);

            GameManager.MainWindow = "game";
            // spawn player
            var player = new Player("player", 251/3, 300/3);
            Engine.SpawnObject(player);

            // spawn world (procederal background + background parts + enemies + rituals)
            var world = new World();
            Engine.SpawnObject("world", world);

            SpawnEnemies();
        }
    }
}