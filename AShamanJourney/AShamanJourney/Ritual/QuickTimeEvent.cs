using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using Aiv.Fast2D;
using System.Drawing;

namespace AShamanJourney.Ritual
{
    class QuickTimeEvent:GameObject
    {
        private int speed;
        private int padding;

        private List<Aiv.Engine.TextObject> QTEList;
        private Aiv.Engine.TextObject qteBox;

        private bool isButtonPressedRight;
        public bool isQteWin;
        public QuickTimeEvent()
        {
            isButtonPressedRight = true;
            isQteWin = true;
            padding = 0;
            speed = 20 * (int)GameManager.GlobalTimer/60;   //need review
            QTEList = new List<Aiv.Engine.TextObject>(); //spawned text object list
            qteBox = new Aiv.Engine.TextObject(1,Color.Black);
        }
        public override void Start()
        {
            base.Start();
            foreach (var item in Utils.RandomKeys(5 + (int)GameManager.GlobalTimer / 60, new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Up, KeyCode.Right, KeyCode.Right, KeyCode.Left }))
            {
                var i = new Aiv.Engine.TextObject(1, Color.Aqua);
                QTEList.Add(i);
                i.Text = $"{item}";
                i.X = Engine.Width - i.Measure().X + padding;
                padding += (int)i.Measure().X + 20;
                Engine.SpawnObject($"QTE_{item}", i);
            }
            qteBox.X = Engine.Width / 2 - qteBox.Measure().X / 2;
            qteBox.Y = Engine.Height / 2 - qteBox.Measure().Y / 2;
            qteBox.Text = "ù";
            Engine.SpawnObject("qteBox", qteBox);

        }
        public override void Update()
        {
                base.Update();
                foreach (var item in QTEList)
                {
                    item.X -= Engine.DeltaTime;
                }
                Input();
        }

        private void Input()
        {
            if ((QTEList[0].X >= qteBox.X && QTEList[0].X <= qteBox.X + qteBox.Measure().X) && (QTEList[0].X + QTEList[0].Measure().X <= qteBox.X + qteBox.Measure().X && QTEList[0].X + QTEList[0].Measure().X >= qteBox.X))  //need check for keyPressed
            {
                QTEList.RemoveAt(0);
            }
            else if (!((QTEList[0].X >= qteBox.X && QTEList[0].X <= qteBox.X + qteBox.Measure().X) && (QTEList[0].X + QTEList[0].Measure().X <= qteBox.X + qteBox.Measure().X && QTEList[0].X + QTEList[0].Measure().X >= qteBox.X)))
            {
                isQteWin = false;
                isButtonPressedRight = false;
            }

            
            
        }
    }
}
