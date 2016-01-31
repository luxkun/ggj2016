using System.Collections.Generic;
using System.Linq;
using Aiv.Engine;
using OpenTK;

namespace AShamanJourney
{
    internal class World : GameObject
    {
        public static List<List<SpriteObject>> SpawnedObjects;
        private readonly List<Dictionary<SpriteObject, float>> objectsSpawnRate;
        private readonly List<float> rndRanges;
        private readonly List<float> spawnChance;
        public Vector2 calculatedEnd;

        public Vector2 calculatedStart;
        // virtual size/2 of the open world
        private Vector2 maxPosition;

        public World()
        {
            calculatedStart = Vector2.Zero;
            calculatedEnd = Vector2.Zero;
            maxPosition = Vector2.Zero;

            objectsSpawnRate = new List<Dictionary<SpriteObject, float>>();
            SpawnedObjects = new List<List<SpriteObject>>
            {
                new List<SpriteObject>(),
                new List<SpriteObject>(),
                new List<SpriteObject>()
            };
            spawnChance = new List<float>
            {
                0.25f,
                1f,
                1f
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
            objectsSpawnRate.Add(new Dictionary<SpriteObject, float>());

            var tree0AssetTop = (SpriteAsset) Engine.GetAsset("tree0_top");
            var tree0AssetBottom = (SpriteAsset) Engine.GetAsset("tree0_bottom");
            var tree0 = new TruncatedObject("tree0", tree0AssetBottom, tree0AssetTop) {Order = 2};
            objectsSpawnRate[0][tree0] = 2f;

            var ritualAsset = (SpriteAsset) Engine.GetAsset("ritual0_0_0");
            var ritual0 = new Ritual(ritualAsset.Width, ritualAsset.Height, Ritual.RitualType.Demoniac) {Order = 2};
            var ritual1 = new Ritual(ritualAsset.Width, ritualAsset.Height, Ritual.RitualType.Earth) {Order = 2};
            var ritual2 = new Ritual(ritualAsset.Width, ritualAsset.Height, Ritual.RitualType.Life) {Order = 2};
            objectsSpawnRate[0][ritual0] = 0.066f;
            objectsSpawnRate[0][ritual1] = 0.066f;
            objectsSpawnRate[0][ritual2] = 0.066f;

            var swamp0Asset = (SpriteAsset)Engine.GetAsset("swamp0");
            var swamp0 = new SpriteObject(swamp0Asset.Width, swamp0Asset.Height, true)
            {
                Name = "swamp0",
                CurrentSprite = swamp0Asset,
                Order = 2
            };
            objectsSpawnRate[0][swamp0] = 0.1f;

            // backgrounds
            objectsSpawnRate.Add(new Dictionary<SpriteObject, float>());
            var background0Asset = (SpriteAsset) Engine.GetAsset("background0");
            var background0 = new SpriteObject(background0Asset.Width, background0Asset.Height)
            {
                Name = "background0",
                CurrentSprite = background0Asset,
                Order = 0
            };
            objectsSpawnRate[1][background0] = 1f;

            // enemies
            objectsSpawnRate.Add(new Dictionary<SpriteObject, float>());
            var bear = EnemyInfo.bear;
            bear.Name = "bear0";
            bear.Order = 6;
            objectsSpawnRate[2][bear] = 1f;

            var rhyno = EnemyInfo.rhyno;
            bear.Name = "rhyno0";
            bear.Order = 6;
            objectsSpawnRate[2][rhyno] = 1f;

            var wolf = EnemyInfo.wolf;
            bear.Name = "wolf0";
            bear.Order = 6;
            objectsSpawnRate[2][wolf] = 1f;


            var count = 0;
            foreach (var dict in objectsSpawnRate)
            {
                rndRanges.Add(0);
                foreach (var pair in dict)
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
            var player = (Player) Engine.Objects["player"];
            if (player.X - calculatedStart.X > maxPosition.X ||
                player.Y - calculatedStart.Y > maxPosition.Y ||
                player.X - calculatedEnd.X > maxPosition.X ||
                player.Y - calculatedEnd.Y > maxPosition.Y)
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

            var defaultBackground = objectsSpawnRate[1].Keys.ElementAt(0);
            var defaultDetails = objectsSpawnRate[0].Keys.ElementAt(0);

            // SPAWN BACKGROUND
            // 1
            for (var x = (int) calculatedStart.X; x < calculatedEnd.X; x += (int) defaultBackground.Width)
                for (var y = (int) calculatedStart.Y; y < oldCalculatedStart.Y; y += (int) defaultBackground.Height)
                    PickRandomObject(x, y, 1);
            // 2
            for (var x = (int) oldCalculatedEnd.X; x < calculatedEnd.X; x += (int) defaultBackground.Width)
                for (var y = (int) oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int) defaultBackground.Height)
                    PickRandomObject(x, y, 1);
            // 3
            for (var x = (int) oldCalculatedStart.X; x < oldCalculatedEnd.X; x += (int) defaultBackground.Width)
                for (var y = (int) oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int) defaultBackground.Height)
                    PickRandomObject(x, y, 1);
            // 4
            for (var x = (int) calculatedStart.X; x < oldCalculatedStart.X; x += (int) defaultBackground.Width)
                for (var y = (int) oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int) defaultBackground.Height)
                    PickRandomObject(x, y, 1);

            // SPAWN ENEMIES
            // 1
            //for (int x = (int)calculatedStart.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
            //    for (int y = (int)calculatedStart.Y; y < oldCalculatedStart.Y; y += (int)defaultBackground.Height)
            //        PickRandomObject(x, y, 2);
            //// 2
            //for (int x = (int)oldCalculatedEnd.X; x < calculatedEnd.X; x += (int)defaultBackground.Width)
            //    for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
            //        PickRandomObject(x, y, 2);
            //// 3
            //for (int x = (int)oldCalculatedStart.X; x < oldCalculatedEnd.X; x += (int)defaultBackground.Width)
            //    for (int y = (int)oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
            //        PickRandomObject(x, y, 2);
            //// 4
            //for (int x = (int)calculatedStart.X; x < oldCalculatedStart.X; x += (int)defaultBackground.Width)
            //    for (int y = (int)oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int)defaultBackground.Height)
            //        PickRandomObject(x, y, 2);

            // SPAWN RANDOM OBJECTS
            // 1
            for (var x = (int) calculatedStart.X; x < calculatedEnd.X; x += (int) defaultDetails.Width)
                for (var y = (int) calculatedStart.Y; y < oldCalculatedStart.Y; y += (int) defaultDetails.Height)
                    PickRandomObject(x, y, 0);
            // 2
            for (var x = (int) oldCalculatedEnd.X; x < calculatedEnd.X; x += (int) defaultDetails.Width)
                for (var y = (int) oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int) defaultDetails.Height)
                    PickRandomObject(x, y, 0);
            // 3
            for (var x = (int) oldCalculatedStart.X; x < oldCalculatedEnd.X; x += (int) defaultDetails.Width)
                for (var y = (int) oldCalculatedEnd.Y; y < calculatedEnd.Y; y += (int) defaultDetails.Height)
                    PickRandomObject(x, y, 0);
            // 4
            for (var x = (int) calculatedStart.X; x < oldCalculatedStart.X; x += (int) defaultDetails.Width)
                for (var y = (int) oldCalculatedStart.Y; y < calculatedEnd.Y; y += (int) defaultDetails.Height)
                    PickRandomObject(x, y, 0);
        }

        public SpriteObject PickRandomObject(int x, int y, int type)
        {
            if (spawnChance[type] < 1f && GameManager.Random.NextDouble() > spawnChance[type])
                return null;
            var range = (float) GameManager.Random.NextDouble()*rndRanges[type];
            var currentObjectsList = objectsSpawnRate[type].GetEnumerator();
            SpriteObject objectInfo = null;
            for (var i = 0; range >= 0f && i <= objectsSpawnRate.Count; i++)
            {
                currentObjectsList.MoveNext();
                range -= currentObjectsList.Current.Value;
                objectInfo = currentObjectsList.Current.Key;
            }

            //Debug.WriteLine($"Random object: {srange} to {range}, {rndRanges[roomType]} => {enemyInfo.CharacterName}");
            var result = (SpriteObject) objectInfo.Clone();
            result.Name += Utils.RandomString(12); // TODO: calculate this
            //result.Xp = result.LevelManager.levelUpTable[level].NeededXp;
            //result.LevelCheck();
            result.X = x;
            result.Y = y;
            result.OnDestroy += sender => { SpawnedObjects[type].Remove((SpriteObject) sender); };
            Engine.SpawnObject(result);
            var player = (Player) Engine.Objects["player"];
            if (type != 1 &&
                ((x + result.Width >= player.X && x <= player.X + player.Width &&
                  y + result.Height >= player.Y && y <= player.Y + player.Height) ||
                 (x + result.Width >= -200 && x <= 200 &&
                  y + result.Height >= -200 && y <= 200)))
            {
                result.Destroy();
                return null;
            }
            SpawnedObjects[type].Add(result);
            return result;
        }
    }
}