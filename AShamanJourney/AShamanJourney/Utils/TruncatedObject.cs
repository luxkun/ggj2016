using Aiv.Engine;

namespace AShamanJourney
{
    public class TruncatedObject : SpriteObject
    {
        private readonly SpriteObject topSprite;
        private readonly SpriteObject bottomSprite;
        private readonly SpriteAsset bottomSpriteAsset;
        private readonly SpriteAsset topSpriteAsset;

        public TruncatedObject(string name, SpriteAsset bottomSpriteAsset, SpriteAsset topSpriteAsset) : base(bottomSpriteAsset.Width, bottomSpriteAsset.Height)
        {
            topSprite = new SpriteObject(topSpriteAsset.Width, topSpriteAsset.Height);
            topSprite.CurrentSprite = topSpriteAsset;
            topSprite.Order = 9;
            bottomSprite = new SpriteObject(bottomSpriteAsset.Width, bottomSpriteAsset.Height, true);
            bottomSprite.CurrentSprite = bottomSpriteAsset;
            bottomSprite.Order = 1;
            this.bottomSpriteAsset = bottomSpriteAsset;
            this.topSpriteAsset = topSpriteAsset;
            Name = name;
        }

        // Add others override if needed
        public override float X
        {
            get { return topSprite.X; }
            set
            {
                topSprite.X = value;
                bottomSprite.X = value;
            }
        }
        public override float Y
        {
            get { return topSprite.Y; }
            set
            {
                topSprite.Y = value;
                bottomSprite.Y = value + topSprite.Height;
            }
        }

        public override void Start()
        {
            base.Start();
            Engine.SpawnObject($"{Name}_topSprite", topSprite);
            Engine.SpawnObject($"{Name}_bottomSprite", bottomSprite);
        }

        public override GameObject Clone()
        {
            var go = new TruncatedObject(Name, bottomSpriteAsset, topSpriteAsset);
            return go;
        }
    }
}