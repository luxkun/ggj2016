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
                new List<GameObject>(), new List<GameObject>()
            };
            rndRanges = new List<float>();
        }

        public override void Start()
        {
            base.Start();

            objectsSpawnRate.Add(new Dictionary<GameObject, float>());
            var details0Asset = (SpriteAsset)Engine.GetAsset("details0");
            var details0 = new SpriteObject(details0Asset.Width/2, details0Asset.Height/2);
            details0.CurrentSprite = details0Asset;
            objectsSpawnRate[0][details0] = 1f;

            objectsSpawnRate.Add(new Dictionary<GameObject, float>());
            var background0Asset = (SpriteAsset)Engine.GetAsset("background0");
            var background0 = new SpriteObject(background0Asset.Width/3, background0Asset.Height/3);
            background0.CurrentSprite = background0Asset;
            objectsSpawnRate[1][background0] = 1f;

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

            maxPosition = new Vector2(Engine.Width*2, Engine.Height*2);
            var player = (Player) Engine.Objects["player"];
            calculatedStart = new Vector2(player.X, player.Y);
            calculatedEnd = new Vector2(player.X, player.Y);
            UpdateWorld(player);
        }

        public override void Update()
        {
            if (GameManager.MainWindow == "game")
                OpenWorldManager();
        }

        private void OpenWorldManager()
        {
            // destroy everything outside 
            //foreach (var o )w
            // TODO: if slow calculate spawns in "boxes"
            var player = (Player) Engine.Objects["player"];
            if ((player.X - calculatedStart.X) > maxPosition.X ||
                (player.Y - calculatedStart.Y) > maxPosition.Y ||
                (player.X - calculatedEnd.X) > maxPosition.X ||
                (player.Y - calculatedEnd.Y) > maxPosition.Y)
            {
                UpdateWorld(player);
            }
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
            for (int x = (int)calculatedStart.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
                for (int y = (int)calculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                {
                    if (x > oldCalculatedStart.X && x < oldCalculatedEnd.X && 
                        y > oldCalculatedStart.Y && y < oldCalculatedEnd.Y)
                        continue;
                    var obj = PickRandomObject(1);
                    obj.X = x;
                    obj.Y = y;
                    Engine.SpawnObject(obj);
                    Console.WriteLine(x + " " + y);
                }

            //for (int x = (int)calculatedStart.X; x < oldCalculatedStart.X; x += (int)defaultBackground.Width)
            //    for (int y = (int)calculatedStart.Y; y < oldCalculatedStart.Y; y += (int)defaultBackground.Height)
            //    {
            //        var obj = PickRandomObject(1);
            //        obj.X = x;
            //        obj.Y = y;
            //        Engine.SpawnObject(obj);
            //        Console.WriteLine(x + " " + y);
            //    }
            //// new parts at right
            //for (int x = (int)oldCalculatedEnd.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
            //    for (int y = (int)oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
            //    {
            //        var obj = PickRandomObject(1);
            //        obj.X = x;
            //        obj.Y = y;
            //        Engine.SpawnObject(obj);
            //    }

            // SPAWN RANDOM OBJECTS
            for (int x = (int)calculatedStart.X; x < calculatedEnd.X; x += (int)defaultDetails.Width)
                for (int y = (int)calculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
                {
                    if (x > oldCalculatedStart.X && x < oldCalculatedEnd.X &&
                        y > oldCalculatedStart.Y && y < oldCalculatedEnd.Y)
                        continue;
                    var obj = PickRandomObject(0);
                    obj.X = x;
                    obj.Y = y;
                    Engine.SpawnObject(obj);
                    Console.WriteLine("--" + x + " " + y);
                }
            //// new parts at left
            //for (int x = (int)calculatedStart.X; x < oldCalculatedStart.X; x += (int)defaultDetails.Width)
            //    for (int y = (int)calculatedStart.Y; y < oldCalculatedStart.Y; y += (int)defaultDetails.Height)
            //    {
            //        var obj = PickRandomObject(0);
            //        obj.X = x;
            //        obj.Y = y;
            //        Engine.SpawnObject(obj);
            //    }
            //// new parts at right
            //for (int x = (int)oldCalculatedEnd.X; x < calculatedEnd.X; x += (int)defaultDetails.Width)
            //    for (int y = (int)oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int)defaultDetails.Height)
            //    {
            //        var obj = PickRandomObject(0);
            //        obj.X = x;
            //        obj.Y = y;
            //        Engine.SpawnObject(obj);
            //    }
        }

        private SpriteObject PickRandomObject(int type)
        {
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