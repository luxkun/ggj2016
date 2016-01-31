using Aiv.Engine;

namespace AShamanJourney
{
    public class PreGame : GameObject
    {
        private SpriteObject logo;

        public PreGame()
        {
            OnDestroy += DestroyEvent;
        }

        private void DestroyEvent(object sender)
        {
            logo.Destroy();
            GameManager.StartGame();
        }

        public override void Start()
        {
            base.Start();

            var logoAsset = (SpriteAsset) Engine.GetAsset("logo");
            logo = new SpriteObject(Engine.Width, Engine.Height);
            logo.CurrentSprite = logoAsset;
            Engine.SpawnObject($"{Name}_logo", logo);
        }

        public override void Update()
        {
            base.Update();

            if (Engine.AnyKeyDown())
            {
                Destroy();
            }
        }
    }
}