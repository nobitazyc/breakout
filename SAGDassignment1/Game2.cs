/*
 *file: Game2.cs
 *Programmer: Yucong Zhou
 *introduction: this file is almost the copy of the game.cs, just for the case that two players on two different screens
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SAGDassignment1
{
    class Game2
    {
        int round = 1;
        private bool win = true;
        private bool start = false;  //if the user press space to start
        private bool left = false; // if the user hold on left
        private bool right = false;  // if the user hold on right 
        private PaintEventArgs Myevent;
        List<Stretch> stretches = new List<Stretch>();
        List<List<RectangleF>> sets = new List<List<RectangleF>>();  // list of the bricks
        List<List<RectangleF>> sets2 = new List<List<RectangleF>>(); 
        RectangleF userPaddle = new RectangleF(1250F, 550F, 100F, 10F);
        RectangleF ball = new RectangleF(1300F, 540F, 10F, 10F);
        RectangleF wallLeft =new RectangleF(900F,0F,5F,600F);
        RectangleF wallRight = new RectangleF(1680F, 0F, 5F, 600F);
        RectangleF wallTop = new RectangleF(900F, 23F, 800F, 5F);
        RectangleF wallBottom = new RectangleF(900F, 560F, 800F, 5F);
        float initialX = 910F;
        float initialY = 100F;
        float brickWidth = 75F;
        float brickHeight = 20F;
        float heightGap = 2F;
        float widthGap = 2F;
        List<Brush> brickBrushes = new List<Brush>(new Brush[] { new SolidBrush(Color.FromArgb(255, 94, 159, 163)), new SolidBrush(Color.FromArgb(255, 220, 209, 180)), new SolidBrush(Color.FromArgb(255, 250, 184, 127)), new SolidBrush(Color.FromArgb(255, 248, 126, 123)), new SolidBrush(Color.FromArgb(255, 176, 85, 116)), new SolidBrush(Color.FromArgb(255, 38, 157, 128)) }); // list of the brushes
        Random r = new Random();
        public float speedX; // X speed of the ball
        public float speedY; // y speed of the ball
        public float speedX2;
        public float speedY2;
        double paddleSpeed = 500F; //speed of the user pannel
        int removeIndex1 = 0;
        int removeIndex2 = 0;
        bool intersect = false;
        bool hitmid = false;
        bool hittop = false;
        int hitnum = 0;
        int mark = 0;
        int mark2 = 0;
        int life = 3;
        int life2 = 3;
        float prevSpeedX;
        float prevSpeedY;
        float prevSpeedX2;
        float prevSpeedY2;
        int gamemode = 2;

        bool tunnel = false;




        /// <summary>
        /// the constructor of the Game, initialize all the bricks
        /// </summary>
        public Game2()
        {
            resetBrick();
            // speedX is randomly generated
            speedX = (float)(r.NextDouble() - 0.5);
            speedY = -0.5F;
          
        }


        /// <summary>
        /// draw all the objects
        /// </summary>
        /// <param name="e"></param>
        public void Draw(PaintEventArgs e)
        {
            Myevent = e;
            e.Graphics.FillRectangle(Brushes.White, userPaddle);
            
            e.Graphics.FillEllipse(Brushes.White, ball);
            e.Graphics.FillRectangle(Brushes.White, wallLeft);
            e.Graphics.FillRectangle(Brushes.White, wallRight);
            e.Graphics.FillRectangle(Brushes.White, wallTop);
            e.Graphics.FillRectangle(Brushes.White, wallBottom);

            //draw the stretches
            foreach (Stretch s in stretches)
            {
                if (s != null)
                {
                    s.DrawExtras(Myevent);
                }
            }
            e.Graphics.DrawString("Mark:" + mark +"                      "+"Life:"+life, new Font("Arial", 20), Brushes.White, 900, 30);
            int i = 5;
            win = true;

            foreach (List<RectangleF> set in sets)
            {
                foreach (RectangleF brick in set)
                {
                    if (brick != null)
                    {
                        e.Graphics.FillRectangle(brickBrushes[i], brick);
                        win = false;
                    }
                }
                i--;
            }
            if (win == true)
            {
                if (round == 1)
                {
                    resetBrick();
                    reset(ref ball, ref userPaddle, ref speedX, ref speedY);
                   
                }
                else
                    MessageBox.Show("YOU WIN!!!!!");
            }


        }
           
        

        /// <summary>
        /// if user press the specific buttons, then update the values
        /// </summary>
        /// <param name="elapsedTime"></param>
        public bool Update(double elapsedTime)
        {

            //result indicate that the ball touchs the bottom wall
            bool result = true;


            //move the paddle
            movePaddle(ref userPaddle,ref ball, elapsedTime,left,right,start);
            
           

            // start the round
            if (start == true)
            {
                ball.X += speedX;
                ball.Y += speedY;
            }
           
            foreach (Stretch s in stretches)
            {
                if( s != null)
                s.Update();
            }

            // get the index that the brick need to be removed
            removeBrick(ref ball, ref mark, ref speedX, ref speedY);
            // remove the breck that being intersected with
            if (intersect == true)
            {
                int instantNum = (int)Math.Floor(r.NextDouble() * 10);// 1/15 give the user a stretch
                if (instantNum == 0)
                {
                    Stretch stretch = new Stretch(sets[removeIndex1][removeIndex2]);
                    stretches.Add(stretch);
                }
                sets[removeIndex1].Remove(sets[removeIndex1][removeIndex2]);
                intersect = false;
            }

            
           
            


            //check if the ball intersece with the wall or the paddle
            result = ballIntersect(ref userPaddle, ref ball,ref life, ref speedX, ref speedY, ref start);
          
            return result;
        }



        public bool Start
        {
            set
            {
                start = value;
            }
        }
       
        public bool Left
        {
            set
            {
                left = value;
            }
        }

       
        public bool Right
        {
            set
            {
                right = value;
            }
        }

      
        public int Game2mode
        {
            set
            {
                gamemode = value;
            }
        }


        /// <summary>
        /// reset the new round base on different players
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="userPaddle"></param>
        /// <param name="speedX"></param>
        /// <param name="speedY"></param>
        private void reset(ref RectangleF ball, ref RectangleF userPaddle, ref float speedX, ref float speedY)
        {
            ball.X = 1300F;
            ball.Y = 540F;
            speedX = (float)(r.NextDouble() - 0.5);
            speedY = -0.5F;
            changemode();
            tunnel = false;
            paddleSpeed =500F;
        }



        /// <summary>
        /// pause all the game
        /// </summary>
        public void pause()
        {
            prevSpeedX = speedX;
            prevSpeedY = speedY;
            speedX = 0;
            speedY = 0;
            prevSpeedX2 = speedX2;
            prevSpeedY2 = speedY2;
            speedX2 = 0;
            speedY2 = 0;
            paddleSpeed = 0;
        }


        /// <summary>
        /// resume the game
        /// </summary>
        public void resume()
        {
            speedX = prevSpeedX;
            speedY = prevSpeedY;
            speedX2 = prevSpeedX2;
            speedY2 = prevSpeedY2;
            paddleSpeed = 500F;
        }


        /// <summary>
        /// change the mode of the game
        /// </summary>
        public void changemode()
        {
            switch (gamemode)
            {
                case 1:
                    userPaddle.X = 1200;
                    userPaddle.Width = 200;
                  
                    break;
                case 2:
                    userPaddle.X = 1250;
                    userPaddle.Width = 100;
                  
                    break;
                case 3:
                    userPaddle.X = 1275;
                    userPaddle.Width = 50;
                   
                    break;
            }
        }



        /// <summary>
        /// reset all the bricks
        /// </summary>
        public void resetBrick()
        {
            sets = new List<List<RectangleF>>();
            for (int i = 0; i < 6; i++)
            {
                List<RectangleF> newset = new List<RectangleF>();

                for (int j = 0; j < 10; j++)
                {
                    RectangleF newrectangle = new RectangleF(initialX + j * (brickWidth + widthGap), initialY + i * (brickHeight + heightGap), brickWidth, brickHeight);
                    newset.Add(newrectangle);
                }

                sets.Add(newset);
            }
        }

        /// <summary>
        /// movePaddle and make the ball movement if the game didn't start, base on different player
        /// </summary>
        /// <param name="myUserPaddle"></param>
        /// <param name="myBall"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="start"></param>
        public void movePaddle(ref RectangleF myUserPaddle, ref RectangleF myBall, double elapsedTime, bool left, bool right, bool start)
        {
            if (!myUserPaddle.IntersectsWith(wallRight)&& !myUserPaddle.IntersectsWith(wallLeft))
            {
                if (left == true)
                {
                    myUserPaddle.X -= Convert.ToSingle(paddleSpeed * elapsedTime);
                    if (start == false)
                    {

                        myBall.X -= Convert.ToSingle(paddleSpeed * elapsedTime);

                    }
                }
                else if (right == true)
                {
                    myUserPaddle.X += Convert.ToSingle(paddleSpeed * elapsedTime);
                    if (start == false)
                    {
                        myBall.X += Convert.ToSingle(paddleSpeed * elapsedTime);
                    }
                }
            }
            else if (myUserPaddle.IntersectsWith(wallLeft))
            {
                if (tunnel == false)
                {
                    if (right == true)
                    {
                        myUserPaddle.X += Convert.ToSingle(paddleSpeed * elapsedTime);
                        if (start == false)
                        {
                            myBall.X += Convert.ToSingle(paddleSpeed * elapsedTime);
                        }
                    }
                }
                else
                {
                    if (left == true)
                    {
                        myUserPaddle.X = 1581;
                       
                    }
                    else if (right == true)
                    {
                        myUserPaddle.X += Convert.ToSingle(paddleSpeed * elapsedTime);
                        if (start == false)
                        {
                            myBall.X += Convert.ToSingle(paddleSpeed * elapsedTime);
                        }
                    }
                }
            }
            else if (myUserPaddle.IntersectsWith(wallRight))
            {
                if (tunnel == false)
                {
                    if (left == true)
                    {
                        myUserPaddle.X -= Convert.ToSingle(paddleSpeed * elapsedTime);
                        if (start == false)
                        {

                            myBall.X -= Convert.ToSingle(paddleSpeed * elapsedTime);

                        }
                    }
                }
                else
                {
                    if (left == true)
                    {
                        myUserPaddle.X -= Convert.ToSingle(paddleSpeed * elapsedTime);
                        if (start == false)
                        {

                            myBall.X -= Convert.ToSingle(paddleSpeed * elapsedTime);

                        }
                    }
                    else if (right == true)
                    {
                        myUserPaddle.X = 900;
                    }
                }
            }
        }


        /// <summary>
        /// get the index of the brick that needs to be removed as well as increase the score base on the player and also change the ball's movement 
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="mark"></param>
        /// <param name="speedX"></param>
        /// <param name="speedY"></param>
        public void removeBrick(ref RectangleF ball, ref int mark, ref float speedX, ref float speedY)
        {
            removeIndex2 = 0;
            removeIndex1 = 0;
            foreach (List<RectangleF> set in sets)
            {
                removeIndex2 = 0;
                foreach (RectangleF brick in set)
                {
                    if (ball.IntersectsWith(brick))
                    {
                        speedY *= -1;
                        intersect = true;
                        hitnum++;
                        //if the ball hit the mid bricks
                        if (hitmid == false)
                        {
                            if (removeIndex1 == 2 || removeIndex1 == 3)
                            {
                                speedX *= 1.1F;
                                speedY *= 1.1F;
                                hitmid = true;
                            }
                        }

                        //if the ball hit the top bricks
                        if (hittop == false)
                        {
                            if (removeIndex1 == 0 || removeIndex1 == 1)
                            {
                                speedX *= 1.1F;
                                speedY *= 1.1F;
                                hittop = true;
                            }
                        }

                        if (hitnum == 4 || hitnum == 12)
                        {
                            speedX *= 1.1F;
                            speedY *= 1.1F;
                        }

                        switch (removeIndex1)
                        {
                            case 0:
                                mark += 5;
                                break;
                            case 1:
                                mark += 5;
                                break;
                            case 2:
                                mark += 3;
                                break;
                            case 3:
                                mark += 3;
                                break;
                            case 4:
                                mark += 1;
                                break;
                            case 5:
                                mark += 1;
                                break;

                        }
                        break;
                    }
                    removeIndex2++;
                }
                if (intersect == true)
                {
                    break;
                }
                removeIndex1++;
            }
        }



        /// <summary>
        /// change the balls movement when the ball hit the walls and the userPaddle based on the player
        /// </summary>
        /// <param name="userPaddle"></param>
        /// <param name="ball"></param>
        /// <param name="life"></param>
        /// <param name="speedX"></param>
        /// <param name="speedY"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool ballIntersect(ref RectangleF userPaddle, ref RectangleF ball,ref int life, ref float speedX, ref float speedY, ref bool start)
        {
            if (ball.IntersectsWith(wallLeft) || ball.IntersectsWith(wallRight))
            {
                speedX *= -1;
            }

            if (ball.IntersectsWith(wallTop))
            {
                speedY *= -1;
                if (userPaddle.Width >= 50)
                {
                    userPaddle.Width /= 1.5F;
                }
            }

            if (ball.IntersectsWith(userPaddle))
            {
                speedY *= -1;
                r.NextDouble();
                speedX = (float)(r.NextDouble() - 0.5);
            }

            if (ball.IntersectsWith(wallBottom))
            {
                if (life != 0)
                {
                    reset(ref ball, ref userPaddle, ref speedX, ref speedY);
                    life--;
                    start = false;
                }
                else
                {
                    MessageBox.Show("You lose");
                    return false;
                }
            }


            //check if the user panel hit the stretches
            int removeStretchIndex = 0;
            bool intersectCheck = false;
            foreach (Stretch s in stretches)
            {
                if (s.Extras.IntersectsWith(userPaddle))
                {
                    switch (s.RandomNum)
                    {
                        case 0:           //increse the length of the panel
                            switch (gamemode)
                            {
                                case 1:
                                    userPaddle.Width = 400;
                                    break;
                                case 2:
                                    userPaddle.Width = 200;
                                    break;
                                case 3:
                                    userPaddle.Width = 100;
                                    break;
                            }
                            break;
                        case 1:   // increse the speed of the panel
                            paddleSpeed += 300F;
                            break;
                        case 2:  // the panel can tunnel from the wall
                            tunnel = true;
                            break;


                    }
                    intersectCheck = true;
                    break;
                }
                removeStretchIndex++;
            }
            if (intersectCheck == true)
            {
                stretches.Remove(stretches[removeStretchIndex]);
            }
            return true;
        }
    }
}
