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

        public static int Wave { get; set; }

        public static Random Random { get; set; }
        public static float GlobalTimer { get; set; }
        public static float LocalTimer { get; set; }

        //static GameManager()
        public static void Initialize()
        {
            engine = new Engine("A Shaman's Journey", 1280, 720, 60, true);
#if DEBUG
            engine.debugCollisions = true;
#endif

            Random = new Random();

            LoadAssets();
            LoadFonts();
        }

        private static void LoadFonts()
        {
            TextConfig.Default = new TextConfig(
                new Asset("ArcadeFont.png"),
                new Dictionary<char, Tuple<Vector2, Vector2>>
                {
                    {"è".ToUpper()[0], Tuple.Create(new Vector2(287f, 290f), new Vector2(63f, 63f))}, // TOP
                    {"à".ToUpper()[0], Tuple.Create(new Vector2(357f, 290f), new Vector2(63f, 63f))}, // LEFT
                    {"ò".ToUpper()[0], Tuple.Create(new Vector2(428f, 290f), new Vector2(63f, 63f))}, // DOWN
                    {"ù".ToUpper()[0], Tuple.Create(new Vector2(498f, 290f), new Vector2(63f, 63f))}, // RIGHT
                    {'0', Tuple.Create(new Vector2(682f, 415f), new Vector2(63f, 63f))},
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
                    {',', Tuple.Create(new Vector2(206f, 290f), new Vector2(30f, 63f))},
                    //{',', Tuple.Create(new Vector2(272f, 93f), new Vector2(13f, 31f))},
                    {'"', Tuple.Create(new Vector2(564f, 186f), new Vector2(51f, 63f))}
                    //{'\'', Tuple.Create(new Vector2(285f, 93f), new Vector2(13f, 31f))}
                }, Color.White, 32f, 63f, staticColor: false);
        }

        private static void LoadAssets()
        {
            Asset.BasePath = "../../assets";
            //Utils.LoadAnimation(engine, "player", "player_sheet.png", 10, 10);
            // enemies
            engine.LoadAsset("genericBullet", new SpriteAsset("singleBullet.png"));
            Utils.LoadAnimation(engine, "playerIdle", "player/idle.png", 4, 2);
            Utils.LoadAnimation(engine, "playerMovingUp", "player/movingUp.png", 4, 2);
            Utils.LoadAnimation(engine, "playerMovingDown", "player/movingDown.png", 4, 2);
            Utils.LoadAnimation(engine, "playerMovingLeft", "player/movingLeft.png", 4, 2);
            Utils.LoadAnimation(engine, "playerMovingRight", "player/movingRight.png", 4, 2);
            //Utils.LoadAnimation(engine, "playerShottingUp", "player/shottingUp.png", 4, 2);
            //Utils.LoadAnimation(engine, "playerShootingDown", "player/shottingDown.png", 4, 2);

            engine.LoadAsset("background0", new SpriteAsset("background0.png"));
            engine.LoadAsset("swamp0", new SpriteAsset("swamp0.png"));
            var tree = new SpriteAsset("tree0.png");
            engine.LoadAsset("tree0_top", new SpriteAsset("tree0.png", 0, 0, tree.Width, tree.Height/2));
            engine.LoadAsset("tree0_bottom", new SpriteAsset("tree0.png", 0, tree.Height/2, tree.Width, tree.Height/2));

            // enemies
            Utils.LoadAnimation(engine, "earthMinion", "earthMinion.png", 4, 4);
            Utils.LoadAnimation(engine, "demoniacMinion", "demoniacMinion.png", 4, 4);
            Utils.LoadAnimation(engine, "lifeMinion", "lifeMinion.png", 4, 4);

            Utils.LoadAnimation(engine, "ritual0", "ritual0.png", 5, 1);
            Utils.LoadAnimation(engine, "ritual1", "ritual1.png", 5, 1);
            Utils.LoadAnimation(engine, "ritual2", "ritual2.png", 5, 1);
            engine.LoadAsset("qteContainer", new SpriteAsset("QteContainer.png"));

            // animals (enemies)
            Utils.LoadAnimation(engine, "bear", "bear.png", 3, 4);
            Utils.LoadAnimation(engine, "wolf", "wolf.png", 3, 4);
            Utils.LoadAnimation(engine, "rhyno", "rhyno.png", 3, 4);

            // SOUND
            engine.LoadAsset("sound_soundtrack", new AudioAsset("sound/soundtrack.ogg"));
            engine.LoadAsset("sound_ritual_intro", new AudioAsset("sound/ritual_intro.ogg"));
            engine.LoadAsset("sound_ritual_soundtrack", new AudioAsset("sound/ritual_soundtrack.ogg"));
            engine.LoadAsset("sound_damage", new AudioAsset("sound/damage.ogg"));
            engine.LoadAsset("sound_heal", new AudioAsset("sound/heal.ogg"));
            engine.LoadAsset("sound_bullet", new AudioAsset("sound/bullet.ogg"));

            // pregame
            engine.LoadAsset("logo", new SpriteAsset("preGame.png"));
        }

        public static void Run()
        {
            EnemyInfo.Initialize(engine);

            StartLogo();

            engine.Run();
        }

        private static void StartLogo()
        {
            var preGame = new PreGame();
            engine.SpawnObject("preGame", preGame);
        }

        private static void StartGame()
        {
            var hud = new Hud();
            engine.SpawnObject("hud", hud);

            var game = new Game();
            engine.SpawnObject("game", game);

            var cameraManager = new CameraManager {Order = 99};
            engine.SpawnObject("cameraManager", cameraManager);
        }
    }
}