using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    internal class World : GameObject
    {
        private readonly List<Dictionary<GameObject, float>> objectsSpawnRate;
        private readonly List<List<GameObject>> spawnedObjects;
        private readonly List<float> spawnChance; 
        private readonly List<float> rndRanges;

        private Vector2 calculatedStart;
        private Vector2 calculatedEnd;
        // virtual size/2 of the open world
        private Vector2 maxPosition;

        public World()
        {
            calculatedStart = Vector2.Zero;
            calculatedEnd = Vector2.Zero;
            maxPosition = Vector2.Zero;

            objectsSpawnRate = new List<Dictionary<GameObject, float>>();
            spawnedObjects = new List<List<GameObject>>
            {
                new List<GameObject>(), new List<GameObject>(), new List<GameObject>()
            };
            spawnChance = new List<float>()
            {
                0.2f, 1f, 0.1f
            };
            rndRanges = new List<float>();
        }

        public override void Start()
        {
            base.Start();

            //var backgroundMaskSprite = (SpriteAsset)Engine.GetAsset("backgroundshadow0");
            //var backgroundMask = new SpriteObject(backgroundMaskSprite.Width, backgroundMaskSprite.Height);
            //backgroundMask.CurrentSprite = backgroundMaskSprite;
            //backgroundMask.Order = 1;
            //backgroundMask.IgnoreCamera = true;
            //Engine.SpawnObject("backgroundMask", backgroundMask);

            // details
            objectsSpawnRate.Add(new Dictionary<GameObject, float>());
            var tree0Asset = (SpriteAsset)Engine.GetAsset("tree0");
            var tree0 = new SpriteObject(tree0Asset.Width / 2, tree0Asset.Height / 2);
            tree0.CurrentSprite = tree0Asset;
            tree0.Order = 2;
            objectsSpawnRate[0][tree0] = 1f;

            var swamp0Asset = (SpriteAsset)Engine.GetAsset("swamp0");
            var swamp0 = new SpriteObject(swamp0Asset.Width, swamp0Asset.Height);
            swamp0.CurrentSprite = swamp0Asset;
            swamp0.Order = 2;
            objectsSpawnRate[0][swamp0] = 0.33f;

            // backgrounds
            objectsSpawnRate.Add(new Dictionary<GameObject, float>());
            var background0Asset = (SpriteAsset)Engine.GetAsset("background0");
            var background0 = new SpriteObject(background0Asset.Width, background0Asset.Height);
            background0.CurrentSprite = background0Asset;
            background0.Order = 0;
            objectsSpawnRate[1][background0] = 1f;

            // enemies
            objectsSpawnRate.Add(new Dictionary<GameObject, float>());
            var bearAsset = (SpriteAsset)Engine.GetAsset("bear");
            var bear = new SpriteObject(bearAsset.Width, bearAsset.Height);
            bear.CurrentSprite = bearAsset;
            bear.Order = 6;
            objectsSpawnRate[2][bear] = 1f;

            var count = 0;
            foreach (var list in objectsSpawnRate)
            {
                rndRanges.Add(0);
                foreach (var pair in list)
                {
                    rndRanges[count] += pair.Value;
                }
                count++;
            }

            maxPosition = new Vector2(Engine.Width*3, Engine.Height*3);
            var player = (Player) Engine.Objects["player"];
            calculatedStart = new Vector2(player.X, player.Y);
            calculatedEnd = new Vector2(player.X, player.Y);
            UpdateWorld(player);
        }

        public override void Update()
        {
            //if (GameManager.MainWindow == "game")
            //    OpenWorldManager();
        }

        private void OpenWorldManager()
        {
            // destroy everything outside 
            //foreach (var o )w
            // TODO: if slow calculate spawns in "boxes"
            var player = (Player)Engine.Objects["player"];
            if ((player.X - calculatedStart.X) > maxPosition.X ||
                (player.Y - calculatedStart.Y) > maxPosition.Y ||
                (player.X - calculatedEnd.X) > maxPosition.X ||
                (player.Y - calculatedEnd.Y) > maxPosition.Y)
            {
                UpdateWorld(player);
            }
        }

        private void SpawnBackground(int x, int y)
        {
            //if (x > oldCalculatedStart.X && x < oldCalculatedEnd.X &&
            //    y > oldCalculatedStart.Y && y < oldCalculatedEnd.Y)
            //    continue;
            var obj = PickRandomObject(1);
            obj.X = x;
            obj.Y = y;
            Engine.SpawnObject(obj);
            Console.WriteLine(x + " " + y);
        }

        private void UpdateWorld(Player player)
        {
            var oldCalculatedStart = calculatedStart;
            var oldCalculatedEnd = calculatedEnd;
            calculatedStart = new Vector2(player.X, player.Y) - maxPosition;
            calculatedEnd = new Vector2(player.X, player.Y) + maxPosition;

            var defaultBackground = (SpriteObject)objectsSpawnRate[1].Keys.ElementAt(0);
            var defaultDetails = (SpriteObject)objectsSpawnRate[0].Keys.ElementAt(0);

            // SPAWN BACKGROUND
            // 1
            for (int x = (int)calculatedStart.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)calculatedStart.Y; y < oldCalculatedStart.Y; y += (int)defaultBackground.Height)
                    SpawnBackground(x, y);
            // 2
            for (int x = (int)oldCalculatedEnd.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                    SpawnBackground(x, y);
            // 3
            for (int x = (int)oldCalculatedStart.X; x < oldCalculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                    SpawnBackground(x, y);
            // 4
            for (int x = (int)calculatedStart.X; x < oldCalculatedStart.X; x += (int)defaultBackground.Width)
                for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                    SpawnBackground(x, y);

            // SPAWN ENEMIES
            // 1
            for (int x = (int)calculatedStart.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)calculatedStart.Y; y < oldCalculatedStart.Y; y += (int)defaultBackground.Height)
                    SpawnEnemy(x, y);
            // 2
            for (int x = (int)oldCalculatedEnd.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                    SpawnEnemy(x, y);
            // 3
            for (int x = (int)oldCalculatedStart.X; x < oldCalculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                    SpawnEnemy(x, y);
            // 4
            for (int x = (int)calculatedStart.X; x < oldCalculatedStart.X; x += (int)defaultBackground.Width)
                for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                    SpawnEnemy(x, y);

            // SPAWN RANDOM OBJECTS
            // 1
            for (int x = (int)calculatedStart.X; x < calculatedEnd.X; x += (int)defaultDetails.Width)
                for (int y = (int)calculatedStart.Y; y < oldCalculatedStart.Y; y += (int)defaultDetails.Height)
                    SpawnDetail(x, y);
            // 2
            for (int x = (int)oldCalculatedEnd.X; x < calculatedEnd.X; x += (int)defaultDetails.Width)
                for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultDetails.Height)
                    SpawnDetail(x, y);
            // 3
            for (int x = (int)oldCalculatedStart.X; x < oldCalculatedEnd.X; x += (int)defaultDetails.Width)
                for (int y = (int)oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int)defaultDetails.Height)
                    SpawnDetail(x, y);
            // 4
            for (int x = (int)calculatedStart.X; x < oldCalculatedStart.X; x += (int)defaultDetails.Width)
                for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultDetails.Height)
                    SpawnDetail(x, y);
        }

        private void SpawnEnemy(int x, int y)
        {
            var obj = PickRandomObject(2);
            if (obj == null)
                return;
            obj.X = x;
            obj.Y = y;
            Engine.SpawnObject(obj);
        }

        private void SpawnDetail(int x, int y)
        {
            var obj = PickRandomObject(0);
            if (obj == null)
                return;
            obj.X = x;
            obj.Y = y;
            //obj.Rotation = (float)(Math.PI * 2 * GameManager.Random.NextDouble());
            Engine.SpawnObject(obj);
        }

        private SpriteObject PickRandomObject(int type)
        {
            if (spawnChance[type] < 1f && GameManager.Random.NextDouble() > spawnChance[type])
                return null;
            var range = (float)GameManager.Random.NextDouble() * rndRanges[type];
            var currentObjectsList = objectsSpawnRate[type].GetEnumerator();
            SpriteObject objectInfo = null;
            for (var i = 0; range >= 0f && i <= objectsSpawnRate.Count; i++)
            {
                currentObjectsList.MoveNext();
                range -= currentObjectsList.Current.Value;
                objectInfo = (SpriteObject) currentObjectsList.Current.Key;
            }

            //Debug.WriteLine($"Random object: {srange} to {range}, {rndRanges[roomType]} => {enemyInfo.CharacterName}");
            var result = (SpriteObject)objectInfo.Clone();
            result.Name += Utils.RandomString(10); // TODO: calculate this
            //result.Xp = result.LevelManager.levelUpTable[level].NeededXp;
            //result.LevelCheck();
            spawnedObjects[type].Add(result);
            return result;
        }
    }
}