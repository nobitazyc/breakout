/*
 *file: Stretch.cs
 *Programmer: Yucong Zhou
 *introduction: this file is creating the stretches
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SAGDassignment1
{
    class Stretch
    {
        private Random r = new Random();
        private RectangleF extras;
        private int randomNum;
        private float x;
        private float y;
        private float speedY = 0.5F;
        private Image speed = Image.FromFile("../../img/speed.png");
        private Image toggle = Image.FromFile("../../img/toggle.png");
        private Image longer = Image.FromFile("../../img/long.png");
        private Image img; // the img that to be used finally


        /// <summary>
        /// the constructor of the stretch pass the brick that need to be moved and get the postion infomation from it
        /// </summary>
        /// <param name="brick"></param>
        public Stretch(RectangleF brick)
        {
            randomNum =(int)Math.Floor(r.NextDouble() * 3);
            createExtras(brick);
        }



        /// <summary>
        /// create the stretch, there are three types of stretches
        /// </summary>
        /// <param name="brick"></param>
        private void createExtras(RectangleF brick)
        {
            x = brick.X;
            y = brick.Y;
            //get one of the stretches
            switch (randomNum)
            {
                case 0:
                    extras = new RectangleF(x, y, 40, 40);
                    img = longer;
                    break;
                case 1:
                    extras = new RectangleF(x, y, 40, 40);
                    img = speed;
                    break;
                case 2:
                    extras = new RectangleF(x, y, 40, 40);
                    img = toggle;
                    break;
            }
        }


        // draw the stretch to the form
        public void DrawExtras(PaintEventArgs e)
        {
            if (extras != null)
                e.Graphics.DrawImage(img, extras);
        }


        // call the update outsid to move the stretch
        public void Update()
        {
            if(extras != null)
            extras.Y += speedY;
        }

        public RectangleF Extras
        {
            get
            {
                return extras;
            }
            set
            {
                extras = value;
            }
        }

        public int RandomNum
        {
            get
            {
                return randomNum;
            }
        }
        
    }
}
