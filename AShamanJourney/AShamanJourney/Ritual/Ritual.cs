using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using Aiv.Fast2D;

namespace AShamanJourney
{
    public class Ritual : SpriteObject
    {
        private static int activatedRituals;
        public bool ActivatedRitual { get; private set; }
        public enum RitualType { Demoniac, Earth, Life};
        public RitualType ritualType { get; }

        public Ritual(int width, int height, RitualType ritualType) : base(width, height, true)
        {
            this.ritualType = ritualType;
            Name = "ritual" + (int) ritualType;
        }
        public override void Start()
        {
            base.Start();
            AddAnimation("idle",  Utils.GetAssetName($"ritual{(int)ritualType}", 0, 0), 8);
            AddAnimation("burning", Utils.GetAssetName($"ritual{(int)ritualType}", 1, 0, 4), 8);
            CurrentAnimation = "idle";
        }

        public override void Update()
        {
            if (GameManager.MainWindow != "game") return;
            if (!ActivatedRitual && Engine.TimeModifier == 1f)
                foreach (var collision in CheckCollisions())
                {
                    var player = collision.Other as Player;
                    if (player != null)
                    {
                        var quickTimeEvent = new QuickTimeEvent(this);
                        Engine.SpawnObject($"{Name}_quicktimeevent", quickTimeEvent);
                        ActivatedRitual = true;
                        CurrentAnimation = "burning";
                        break;
                    }
                }
        }
        public override GameObject Clone()
        {
            var go = new Ritual((int) BaseWidth, (int) BaseHeight, ritualType)
            {
                Name = Name
            };
            return go;
        }

        public void Activated(bool success)
        {
            if (success)
            {
                switch (ritualType)
                {
                    case RitualType.Demoniac:
                    case RitualType.Earth:
                    case RitualType.Life:
                        SpawnMinion();
                        break;
                }
                GameManager.LocalTimer *= 0.5f;
            }
            else
            {
                GameManager.LocalTimer *= 1.1f;
            }
            AutomaticHitBox = false;
            HitBoxes.Clear();
        }

        private void SpawnMinion()
        {
            var minion = new Minion(60, 40, (Minion.MinionType) ritualType);
            var player = (Player)Engine.Objects["player"];
            minion.X = player.X - 100f;
            minion.Y = player.Y - 100f;
            Engine.SpawnObject($"{Name}_{minion.Name}_n{activatedRituals++}", minion);
        }
    }
}
