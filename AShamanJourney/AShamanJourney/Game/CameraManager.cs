using Aiv.Engine;

namespace AShamanJourney
{
    public class CameraManager : GameObject
    {
        public CameraManager()
        {

        }

        public override void Update()
        {
            var player = (Player) Engine.Objects["player"];
            var cameraX = player.X - player.Width / 2 - Engine.Width / 2;
            var cameraY = player.Y - player.Height / 2 - Engine.Height / 2;
            var world = (World) Engine.Objects["world"];

            if (cameraX < world.calculatedStart.X)
                cameraX = world.calculatedStart.X;
            if (cameraY < world.calculatedStart.Y)
                cameraY = world.calculatedStart.Y;
            if (cameraX + Engine.Width > world.calculatedEnd.X)
                cameraX = world.calculatedEnd.X - Engine.Width;
            if (cameraY + Engine.Height > world.calculatedEnd.Y)
                cameraY = world.calculatedEnd.Y - Engine.Height;
            Engine.Camera.X = cameraX;
            Engine.Camera.Y = cameraY;
        }
    }
}