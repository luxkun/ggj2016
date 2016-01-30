using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using Aiv.Fast2D;

namespace AShamanJourney.Ritual
{
    class Ritual : SpriteObject
    {
        public enum RitualType { Demoniac, Earth };
        RitualType ritualType;

        public Ritual(int width, int height,RitualType rt) : base(width, height)
        {
            ritualType = rt;
        }
        public override void Start()
        {
            base.Start();
            AddAnimation($"{ritualType}_Idle",  Utils.GetAssetName($"ritual_{(int)ritualType}", 0, 0), 4);
            AddAnimation($"{ritualType}_Burning", Utils.GetAssetName($"ritual_{(int)ritualType}", 1, 0, 4),4);
            //ciao
        }
    }
}
