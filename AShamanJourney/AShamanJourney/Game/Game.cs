using System;
using Aiv.Engine;

namespace AShamanJourney
{
    public class Game : GameObject
    {
        public Game()
        {
        }


        public override void Start()
        {
            base.Start();

            GameManager.MainWindow = "game";
            // spawn player
            var player = new Player("player", 64, 32);

            // spawn world (procederal background + background parts + enemies + rituals)
            var world = new World();

            Engine.SpawnObject(player);
            Engine.SpawnObject("world", world);
        }
    }
}