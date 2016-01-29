using System;
using Aiv.Engine;

namespace AShamanJourney
{
    public static class GameManager
    {
        private static Engine engine;
        public static string MainWindow { get; set; }
        static GameManager()
        {
            engine = new Engine("A Shaman's Journey", 1280, 720, 60, false);

            Random = new Random();

            LoadAssets();
        }

        public static Random Random { get; set; }

        private static void LoadAssets()
        {
            Asset.BasePath = "../../assets";
            //Utils.LoadAnimation(engine, "player", "player_sheet.png", 10, 10);
            engine.LoadAsset("player", new SpriteAsset("player.png"));
            engine.LoadAsset("background0", new SpriteAsset("background0.png"));
            engine.LoadAsset("details0", new SpriteAsset("details0.png"));
        }

        public static void Run()
        {
            var game = new Game();
            engine.SpawnObject("game", game);

            engine.Run();
        }
    }
}