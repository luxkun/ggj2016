using System;
using System.Collections.Generic;
using System.Drawing;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    public static class GameManager
    {
        private static Engine engine;
        public static string MainWindow { get; set; }
        //static GameManager()
        public static void Initialize()
        {
            engine = new Engine("A Shaman's Journey", 1280, 720, 60, false);

            Random = new Random();

            LoadAssets();
            LoadFonts();
        }

        private static void LoadFonts()
        {
            TextConfig.Default = new TextConfig(
                new Asset("ArcadeFont.png"),
                new Dictionary<char, Tuple<Vector2, Vector2>>  {
                    { '0', Tuple.Create(new Vector2(682f, 415f), new Vector2(63f, 63f))},
                    {'1', Tuple.Create(new Vector2(22f, 415f), new Vector2(57f, 63f))},
                    {'2', Tuple.Create(new Vector2(89f, 415f), new Vector2(64f, 63f))},
                    {'3', Tuple.Create(new Vector2(163f, 415f), new Vector2(64f, 63f))},
                    {'4', Tuple.Create(new Vector2(237f, 415f), new Vector2(64f, 63f))},
                    {'5', Tuple.Create(new Vector2(312f, 415f), new Vector2(63f, 63f))},
                    {'6', Tuple.Create(new Vector2(386f, 415f), new Vector2(63f, 63f))},
                    {'7', Tuple.Create(new Vector2(460f, 415f), new Vector2(63f, 63f))},
                    {'8', Tuple.Create(new Vector2(534f, 415f), new Vector2(63f, 63f))},
                    {'9', Tuple.Create(new Vector2(608f, 415f), new Vector2(63f, 63f))},
                    {'A', Tuple.Create(new Vector2(11f, 39f), new Vector2(63f, 63f))},
                    {'B', Tuple.Create(new Vector2(84f, 39f), new Vector2(63f, 63f))},
                    {'C', Tuple.Create(new Vector2(159f, 39f), new Vector2(63f, 63f))},
                    {'D', Tuple.Create(new Vector2(233f, 39f), new Vector2(63f, 63f))},
                    {'E', Tuple.Create(new Vector2(307f, 39f), new Vector2(63f, 63f))},
                    {'F', Tuple.Create(new Vector2(381f, 39f), new Vector2(63f, 63f))},
                    {'G', Tuple.Create(new Vector2(455f, 39f), new Vector2(63f, 63f))},
                    {'H', Tuple.Create(new Vector2(529f, 39f), new Vector2(63f, 63f))},
                    {'I', Tuple.Create(new Vector2(603f, 39f), new Vector2(31f, 63f))},
                    {'J', Tuple.Create(new Vector2(646f, 39f), new Vector2(63f, 63f))},
                    {'K', Tuple.Create(new Vector2(720f, 39f), new Vector2(63f, 63f))},
                    {'L', Tuple.Create(new Vector2(11f, 113f), new Vector2(57f, 63f))},
                    {'M', Tuple.Create(new Vector2(78f, 113f), new Vector2(64f, 63f))},
                    {'N', Tuple.Create(new Vector2(152f, 113f), new Vector2(64f, 63f))},
                    {'O', Tuple.Create(new Vector2(226f, 113f), new Vector2(64f, 63f))},
                    {'P', Tuple.Create(new Vector2(301f, 113f), new Vector2(63f, 63f))},
                    {'Q', Tuple.Create(new Vector2(375f, 113f), new Vector2(63f, 63f))},
                    {'R', Tuple.Create(new Vector2(449f, 113f), new Vector2(63f, 63f))},
                    {'S', Tuple.Create(new Vector2(523f, 113f), new Vector2(63f, 63f))},
                    {'T', Tuple.Create(new Vector2(597f, 113f), new Vector2(57f, 63f))},
                    {'U', Tuple.Create(new Vector2(665f, 113f), new Vector2(63f, 63f))},
                    {'V', Tuple.Create(new Vector2(11f, 186f), new Vector2(63f, 63f))},
                    {'W', Tuple.Create(new Vector2(85f, 186f), new Vector2(63f, 63f))},
                    {'X', Tuple.Create(new Vector2(159f, 186f), new Vector2(63f, 63f))},
                    {'Y', Tuple.Create(new Vector2(233f, 186f), new Vector2(57f, 63f))},
                    {'Z', Tuple.Create(new Vector2(301f, 186f), new Vector2(63f, 63f))},
                    //{'%', Tuple.Create(new Vector2(44f, 93f), new Vector2(44f, 31f))},
                    {'!', Tuple.Create(new Vector2(392f, 186f), new Vector2(32f, 63f))},
                    {'?', Tuple.Create(new Vector2(435f, 186f), new Vector2(63f, 63f))},
                    //{'+', Tuple.Create(new Vector2(142f, 93f), new Vector2(36f, 31f))},
                    {'-', Tuple.Create(new Vector2(626f, 186f), new Vector2(38f, 63f))},
                    //{'*', Tuple.Create(new Vector2(209f, 93f), new Vector2(30f, 31f))},
                    //{'/', Tuple.Create(new Vector2(238f, 93f), new Vector2(34f, 31f))},
                    //{':', Tuple.Create(new Vector2(296f, 93f), new Vector2(13f, 31f))},
                    {'.', Tuple.Create(new Vector2(515f, 186f), new Vector2(33f, 63f))},
                    //{',', Tuple.Create(new Vector2(272f, 93f), new Vector2(13f, 31f))},
                    {'"', Tuple.Create(new Vector2(564f, 186f), new Vector2(51f, 63f))},
                    //{'\'', Tuple.Create(new Vector2(285f, 93f), new Vector2(13f, 31f))}
            }, Color.White, 32f, 63f, staticColor: false);
        }

        public static Random Random { get; set; }
        public static float GlobalTimer { get; set; }
        public static float LocalTimer { get; set; }

        private static void LoadAssets()
        {
            Asset.BasePath = "../../assets";
            //Utils.LoadAnimation(engine, "player", "player_sheet.png", 10, 10);
            // enemies
            engine.LoadAsset("bear", new SpriteAsset("bear.png"));

            engine.LoadAsset("player", new SpriteAsset("player.png"));
            engine.LoadAsset("background0", new SpriteAsset("background0.png"));
            engine.LoadAsset("swamp0", new SpriteAsset("swamp0.png"));
            engine.LoadAsset("tree0", new SpriteAsset("tree0.png"));
            //engine.LoadAsset("backgroundshadow0", new SpriteAsset("background_shadow.png"));
        }

        public static void Run()
        {
            var hud = new Hud();
            engine.SpawnObject("hud", hud);

            var game = new Game();
            engine.SpawnObject("game", game);

            engine.Run();
        }
    }
}