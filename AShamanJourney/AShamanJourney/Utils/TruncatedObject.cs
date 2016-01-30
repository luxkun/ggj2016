using Aiv.Engine;

namespace AShamanJourney
{
    public class TruncatedObject : SpriteObject
    {
        private SpriteObject topSprite;
        private SpriteObject bottomSprite;
        
        public TruncatedObject(string name, SpriteAsset bottomSpriteAsset, SpriteAsset topSpriteAsset) : base(bottomSpriteAsset.Width, bottomSpriteAsset.Height)
        {
            topSprite = new SpriteObject(topSpriteAsset.Width, topSpriteAsset.Height);
            topSprite.CurrentSprite = topSpriteAsset;
            bottomSprite = new SpriteObject(bottomSpriteAsset.Width, bottomSpriteAsset.Height);
            bottomSprite.CurrentSprite = bottomSpriteAsset;
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
    }
}