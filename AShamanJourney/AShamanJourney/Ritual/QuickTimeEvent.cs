using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using Aiv.Fast2D;

namespace AShamanJourney.Ritual
{
    class QuickTimeEvent
    {
        List<char> key;
        private int speed;

        public QuickTimeEvent()
        {
            speed = 100;

            key = new List<char>();

            Utils.RandomKeys(5 + (int)GameManager.GlobalTimer / 60, new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Up, KeyCode.Right, KeyCode.Right, KeyCode.Left });
        }

        public void Update()
        {
            
        }
    }
}
