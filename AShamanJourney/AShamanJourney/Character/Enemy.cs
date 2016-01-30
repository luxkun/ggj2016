using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AShamanJourney
{
    public class Enemy : Character
    {
        public Enemy(string name, int width, int height, Dictionary<string, float> levelUpModifiers) : base(name, width, height, levelUpModifiers)
        {
        }
    }
}
