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
            AddAnimation("idle",  Utils.GetAssetName($"ritual{(int)ritualType}", 0, 0), 4);
            AddAnimation("burning", Utils.GetAssetName($"ritual{(int)ritualType}", 1, 0, 4),4);
            CurrentAnimation = "idle";
        }

        public override void Update()
        {
            if (GameManager.MainWindow != "game") return;
            if (!ActivatedRitual)
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
            throw new NotImplementedException();
        }
    }
}
