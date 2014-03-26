using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SAGDassignment1
{
    public partial class Breakout : Form
    {
        Stopwatch timer = new Stopwatch();
        double lastTime;
        Game myGame = new Game();
        Game2 myGame2;
        bool twoPlayerOneScreen = false;
        /// <summary>
        /// when start the game initialize the component and the timer
        /// </summary>
        public Breakout()
        {
            InitializeComponent();
            initializeTimer();
        }


        /// <summary>
        /// initialize the timer using in the game
        /// </summary>
        private void initializeTimer()
        {
            lastTime = 0;
            timer.Reset();
            timer.Start();
        }


        private void Breakout_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }



        private void Breakout_Paint(object sender, PaintEventArgs e)
        {
            double gameTime = timer.ElapsedMilliseconds / 1000.0;
            double elapsedTime = gameTime - lastTime;
            lastTime = gameTime;
            bool result = myGame.Update(elapsedTime);
            myGame.Draw(e);
            if (myGame2 != null)
            {
                result = myGame2.Update(elapsedTime);
                myGame2.Draw(e);
            }
            if (result == true)
                this.Invalidate();
            else
                this.Close();
        }

        private void Breakout_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyCode = e.KeyChar;
            if (keyCode == (char)Keys.Space)
            {
                myGame.Start = true;
                myGame.Start2 = true;
                if (myGame2 != null)
                {
                    myGame2.Start = true;
                }
            }
        }

        private void Breakout_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (myGame2 == null && twoPlayerOneScreen == false)
            {
                if (keyCode == Keys.Left)
                {
                    myGame.Left = true;
                }
                if (keyCode == Keys.Right)
                {
                    myGame.Right = true;
                }
            }
            else if ( myGame2 != null )
            {
                if (keyCode == Keys.A)
                {
                    myGame.Left = true;
                }
                if (keyCode == Keys.D)
                {
                    myGame.Right = true;
                }
                if (keyCode == Keys.Left)
                {
                    myGame2.Left = true;
                }
                if (keyCode == Keys.Right)
                {
                    myGame2.Right = true;
                }
            }
            else if (twoPlayerOneScreen == true)
            {
                if (keyCode == Keys.Left)
                {
                    myGame.Left2 = true;
                }
                if (keyCode == Keys.Right)
                {
                    myGame.Right2 = true;
                }
                if (keyCode == Keys.A)
                {
                    myGame.Left = true;
                }
                if (keyCode == Keys.D)
                {
                    myGame.Right = true;
                }
            }
        }

        private void Breakout_KeyUp(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (myGame2 == null && twoPlayerOneScreen == false)
            {
                if (keyCode == Keys.Left)
                {
                    myGame.Left = false;
                }
                if (keyCode == Keys.Right)
                {
                    myGame.Right = false;
                }
            }
            else if (myGame2 !=null)
            {
                if (keyCode == Keys.A)
                {
                    myGame.Left = false;
                }
                if (keyCode == Keys.D)
                {
                    myGame.Right = false;
                }
                if (keyCode == Keys.Left)
                {
                    myGame2.Left = false;
                }
                if (keyCode == Keys.Right)
                {
                    myGame2.Right = false;
                }
            }
            else if (twoPlayerOneScreen == true)
            {
                if (keyCode == Keys.Left)
                {
                    myGame.Left2 = false;
                }
                if (keyCode == Keys.Right)
                {
                    myGame.Right2 = false;
                }
                if (keyCode == Keys.A)
                {
                    myGame.Left = false;
                }
                if (keyCode == Keys.D)
                {
                    myGame.Right = false;
                }
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pauseToolStripMenuItem.Text == "pause")
            {
                pauseToolStripMenuItem.Text = "resume";
                myGame.pause();
                if (myGame2 != null)
                {
                    myGame2.pause();
                }
                
            }
            else
            {
                pauseToolStripMenuItem.Text = "pause";
                myGame.resume();
                if (myGame2 != null)
                {
                    myGame2.resume();
                }
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myGame.Gamemode = 1;
            myGame.changemode();
            if (myGame2 != null)
            {
                myGame2.Game2mode = 1;
                myGame2.changemode();
            }
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myGame.Gamemode = 2;
            myGame.changemode();
            if (myGame2 != null)
            {
                myGame2.Game2mode = 2;
                myGame2.changemode();
            }
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myGame.Gamemode = 3;
            myGame.changemode();
            if (myGame2 != null)
            {
                myGame2.Game2mode = 3;
                myGame2.changemode();
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myGame.Start = true;
            if (myGame2 != null)
            {
                myGame2.Start = true;
            }
        }

        private void twoPlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 1700;
            myGame = new Game();
            myGame2 = new Game2();
        }

        private void singleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 800;
            myGame = new Game();
            myGame2 = null;
        }

        private void twoPlayersOneScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 800;
            twoPlayerOneScreen = true;
            myGame = new Game(2);
            myGame2 = null;
        }

    }
}
