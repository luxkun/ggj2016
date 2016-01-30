using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;

namespace AShamanJourney
{
    public class EnemyInfo
    {
        public Enemy bear;
        public Enemy wolf;
        public Enemy boar;

        public void Enemies(Engine engine)
        {
            bear = new Enemy();
            bear.Stats.Attack = 30f;

            

            wolf = new Enemy();
            boar = new Enemy();
        }
    }
}
