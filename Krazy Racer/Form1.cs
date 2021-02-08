using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Input;
using WMPLib;

namespace Krazy_Racer
{
    public partial class FormRace : Form
    {
        int bg;

        List<PictureBox> listPurple = new List<PictureBox>();
        List<PictureBox> listBlue = new List<PictureBox>();
        List<PictureBox> listRed = new List<PictureBox>();

        Random generator = new Random();

        int score;
        int level;

        string resourcesPath = Application.StartupPath + "\\Resources\\";
        WindowsMediaPlayer loopSound = new WindowsMediaPlayer();
        WindowsMediaPlayer normalSound = new WindowsMediaPlayer();

        public FormRace()
        {
            InitializeComponent();
        }

        private void FormRace_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            timerBG.Enabled = true;
            timerGame.Enabled = true;
            timerRed.Enabled = false;
            timerPurple.Enabled = true;
            timerBlue.Enabled = false;
            timerScore.Enabled = true;
            timerRed.Interval = 15000;
            timerBlue.Interval = 10000;
            timerPurple.Interval = 5000;

            loopSound.URL = resourcesPath + "loopMusic.mp3";
            loopSound.settings.setMode("loop", true);
            loopSound.settings.mute = false;

            labelShowLevel.Text = "";
            bg = 0;
            score = 0;
            level = 1;
        }

        private void timerBG_Tick(object sender, EventArgs e)
        {
            //Background Images
            if (bg == 0)
            {
                bg++;
                this.BackgroundImage = Properties.Resources.road1;
            }
            else if (bg == 1)
            {
                bg++;
                this.BackgroundImage = Properties.Resources.road2;
            }
            else if (bg == 2)
            {
                bg++;
                this.BackgroundImage = Properties.Resources.road3;
            }
            else
            {
                bg = 0;
                this.BackgroundImage = Properties.Resources.road4;
            }
        }

        private void timerGame_Tick(object sender, EventArgs e)
        {
            //Moving the P1
            if (Keyboard.IsKeyDown(Key.Up) && pictureBoxPlayer.Top > 0)
            {
                pictureBoxPlayer.Top -= 5;
            }
            else if (Keyboard.IsKeyDown(Key.Down) && pictureBoxPlayer.Bottom < 600)
            {
                pictureBoxPlayer.Top += 5;
            }   
            if (Keyboard.IsKeyDown(Key.Left) && pictureBoxPlayer.Left > 0)
            {
                pictureBoxPlayer.Left -= 8;
            }
            else if (Keyboard.IsKeyDown(Key.Right) && pictureBoxPlayer.Right < this.ClientSize.Width)
            {
                pictureBoxPlayer.Left += 8;
            }
            
            //Moving the opponents
            //Moving Purple
            for (int i=0;i<listPurple.Count;i++)
            {
                listPurple[i].Top += (int)listPurple[i].Tag;
                if (listPurple[i].Top > this.ClientSize.Height)
                {
                    listPurple[i].Top += (int)listPurple[i].Tag;
                    listPurple[i].Dispose();
                    listPurple.RemoveAt(i);
                }
                else
                {
                    listPurple[i].Refresh();
                }
            }
            //Moving Blue
            for (int i = 0; i < listBlue.Count; i++)
            {
                listBlue[i].Top += (int)listBlue[i].Tag;
                //Moving Purple Aside
                for (int j=0; j<listPurple.Count; j++)
                {
                    if (listPurple[j].Location.Y > listBlue[i].Location.Y)
                    {
                        if (listPurple[j].Location.Y - listBlue[i].Location.Y < 250)
                        {
                            if (listPurple[j].Location.X >= listBlue[i].Location.X && listPurple[j].Location.X <= listBlue[i].Location.X + 65)
                            {
                                listPurple[j].Left += 2;
                                listPurple[j].Refresh();
                            }
                            else if (listPurple[j].Location.X >= listBlue[i].Location.X - 65 && listPurple[j].Location.X <= listBlue[i].Location.X)
                            {
                                listPurple[j].Left -= 2;
                                listPurple[j].Refresh();
                            }
                        }
                    }
                }
                if (listBlue[i].Top > this.ClientSize.Height)
                {
                    listBlue[i].Top += (int)listBlue[i].Tag;
                    listBlue[i].Dispose();
                    listBlue.RemoveAt(i);
                }
                else
                {
                    listBlue[i].Refresh();
                }
            }

            //Moving Red
            for (int i = 0; i < listRed.Count; i++)
            {
                listRed[i].Top -= (int)listRed[i].Tag;
                //Moving Red Aside
                for (int j =0; j<listPurple.Count; j++)
                {
                    if (listPurple[j].Location.Y < listRed[i].Location.Y )
                    {
                        if (listRed[i].Location.Y - listPurple[j].Location.Y < 180)
                        {
                            if (listPurple[j].Location.X >= listRed[i].Location.X && listPurple[j].Location.X <= listRed[i].Location.X + 65)
                            {
                                listRed[i].Left -= 7;
                                listRed[i].Refresh();
                            }
                            else if (listPurple[j].Location.X >= listRed[i].Location.X - 65 && listPurple[j].Location.X <= listRed[i].Location.X)
                            {
                                listRed[i].Left += 7;
                                listRed[i].Refresh();
                            }
                        }
                    }
                }
                for (int j= 0; j< listBlue.Count; j++)
                {
                    if ( listBlue[j].Location.Y  < listRed[i].Location.Y)
                    {
                        if (listRed[i].Location.Y - listBlue[j].Location.Y < 180)
                        {
                            if (listBlue[j].Location.X >= listRed[i].Location.X && listBlue[j].Location.X <= listRed[i].Location.X + 65)
                            {
                                listRed[i].Left -= 7;
                                listRed[i].Refresh();
                            }
                            else if (listBlue[j].Location.X >= listRed[i].Location.X - 65 && listBlue[j].Location.X <= listRed[i].Location.X)
                            {
                                listRed[i].Left += 7;
                                listRed[i].Refresh();
                            }
                        }
                    }
                }
                if (listRed[i].Top < -200)
                {
                    listRed[i].Top -= (int)listRed[i].Tag;
                    listRed[i].Dispose();
                    listRed.RemoveAt(i);
                }
                else
                {
                    listRed[i].Refresh();
                }
            }

            // Check Collision
            
            for (int i = 0; i < listPurple.Count; i++)
            {
                if (pictureBoxPlayer.Bounds.IntersectsWith(listPurple[i].Bounds))
                {
                    GameOver();
                }
            }
            for (int i = 0; i < listBlue.Count; i++)
            {
                if (pictureBoxPlayer.Bounds.IntersectsWith(listBlue[i].Bounds))
                {
                    GameOver();
                }
            }
            for (int i = 0; i < listRed.Count; i++)
            {
                if (pictureBoxPlayer.Bounds.IntersectsWith(listRed[i].Bounds))
                {
                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            timerGame.Enabled = false;
            timerBG.Enabled = false;
            timerPurple.Enabled = false;
            timerRed.Enabled = false;
            timerBlue.Enabled = false;
            timerScore.Enabled = false;
            loopSound.settings.mute = true;
            DialogResult answer = MessageBox.Show("Game Over. You Scored : " + score + " Would you like to restart ?", "Well Played", MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes) //Reset Game
            {
                for (int i = 0; i < listRed.Count; i++)
                {
                    listRed[i].Dispose();
                }
                listRed.Clear();
                for (int i = 0; i < listPurple.Count; i++)
                {
                    listPurple[i].Dispose();
                }
                listPurple.Clear();
                for (int i = 0; i < listBlue.Count; i++)
                {
                    listBlue[i].Dispose();
                }
                listBlue.Clear();

                timerBG.Enabled = true;
                timerGame.Enabled = true;
                timerRed.Enabled = false;
                timerPurple.Enabled = true;
                timerBlue.Enabled = false;
                timerScore.Enabled = true;
                timerRed.Interval = 15000;
                timerBlue.Interval = 10000;
                timerPurple.Interval = 5000;
                loopSound.settings.mute = false;
                pictureBoxPlayer.Location = new Point(170, 295);
                score = 0;
                level = 1;
            }
            else
            {
                this.Close();
            }
        }

        private void timerPurple_Tick(object sender, EventArgs e)
        {
            //Purple Opponent Generator
            PictureBox pict = new PictureBox();
            int spawnX = generator.Next(26, 323);
            pict.BackColor = Color.Transparent;
            pict.Size = new System.Drawing.Size(60, 110);
            pict.SizeMode = PictureBoxSizeMode.StretchImage;
            pict.Location = new Point(spawnX, -500);
            pict.Image = Properties.Resources.cpumedium;
            pict.Tag = 5; //Speed Slow
            listPurple.Add(pict);
            this.Controls.Add(pict);
        }

        private void timerBlue_Tick(object sender, EventArgs e)
        {
            //Blue Opponent Generator
            PictureBox pict = new PictureBox();
            int spawnX = generator.Next(26, 323);
            pict.BackColor = Color.Transparent;
            pict.Size = new System.Drawing.Size(60, 110);
            pict.SizeMode = PictureBoxSizeMode.StretchImage;
            pict.Location = new Point(spawnX, -500);
            pict.Image = Properties.Resources.cpuslow;
            pict.Tag = 10; //Speed Med
            listBlue.Add(pict);
            this.Controls.Add(pict);
        }

        private void timerRed_Tick(object sender, EventArgs e)
        {
            //Red Opponent Generator
            PictureBox pict = new PictureBox();
            int spawnX = generator.Next(26, 323);
            pict.BackColor = Color.Transparent;
            pict.Size = new System.Drawing.Size(60, 110);
            pict.SizeMode = PictureBoxSizeMode.StretchImage;
            pict.Location = new Point(spawnX, 800);
            pict.Image = Properties.Resources.cpufast;
            pict.Tag = 3; //Speed Slow
            listRed.Add(pict);
            this.Controls.Add(pict);
        }

        private void timerScore_Tick(object sender, EventArgs e)
        {
            score++;
            labelScore.Text = score.ToString();
            labelLevel.Text = level.ToString();
            if (score >= 50 && score < 100)
            { 
                level = 2;
                timerPurple.Interval = 4000;
            }
            else if (score >= 100 && score < 150)
            {
                level = 3;
                timerPurple.Interval = 3500;
                timerBlue.Enabled = true;
            }
            else if (score >= 150 && score < 200)
            {
                level = 4;
                timerBlue.Interval = 9000;
            }
            else if (score >= 200 && score < 250)
            {
                level = 5;
                timerPurple.Interval = 3000;
                timerBlue.Interval = 8000;
                timerRed.Enabled = true;
            }
            else if (score >= 250 && score < 300)
            {
                level = 6;
                timerBlue.Interval = 7000;
                timerRed.Interval = 12000;
            }
            else if (score >= 300 && score < 400)
            {
                level = 7;
                timerPurple.Interval = 2500;
                timerBlue.Interval = 6000;
                timerRed.Interval = 9000;
            }
            else if (score >= 400 && score < 600)
            {
                level = 8;
                timerPurple.Interval = 2000;
                timerBlue.Interval = 5000;
                timerRed.Interval = 7000;
            }
            else if (score >= 600 && score < 800)
            {
                level = 9;
                timerPurple.Interval = 1800;
                timerBlue.Interval = 4000;
                timerRed.Interval = 6000;
            }
            else if (score >= 800 && score < 1000)
            {
                level = 10;
                timerPurple.Interval = 1500;
                timerBlue.Interval = 3500;
                timerRed.Interval = 5500;
            }
            else if (score >= 1000)
            {
                timerGame.Enabled = false;
                timerBG.Enabled = false;
                timerPurple.Enabled = false;
                timerRed.Enabled = false;
                timerBlue.Enabled = false;
                timerScore.Enabled = false;
                DialogResult answer = MessageBox.Show("CONGRATULATIONS. You've Completed All The Levels. Would You Like To Restart?", "GAME FINISHED", MessageBoxButtons.YesNo);
                if (answer == DialogResult.Yes) //Reset Game
                {
                    for (int i = 0; i < listRed.Count; i++)
                    {
                        listRed[i].Dispose();
                    }
                    listRed.Clear();
                    for (int i = 0; i < listPurple.Count; i++)
                    {
                        listPurple[i].Dispose();
                    }
                    listPurple.Clear();
                    for (int i = 0; i < listBlue.Count; i++)
                    {
                        listBlue[i].Dispose();
                    }
                    listBlue.Clear();

                    timerBG.Enabled = true;
                    timerGame.Enabled = true;
                    timerRed.Enabled = false;
                    timerPurple.Enabled = true;
                    timerBlue.Enabled = false;
                    timerScore.Enabled = true;
                    timerRed.Interval = 15000;
                    timerBlue.Interval = 10000;
                    timerPurple.Interval = 5000;
                    pictureBoxPlayer.Location = new Point(170, 295);
                    score = 0;
                    level = 1;
                }
                else
                {
                    this.Close();
                }
            }

            if (score == 1 || score == 50 || score == 100 || score == 150 || score == 200 || score == 250 || score == 300 || score == 400 || score == 600 || score == 800)
            {
                ShowLevel();
                normalSound.URL = resourcesPath + "RCPass.wav";
            }
        }

        private void ShowLevel()
        {
            timerLevel.Enabled = true;
            labelShowLevel.Text = "LEVEL " + level;
        }

        private void timerLevel_Tick(object sender, EventArgs e)
        {
            labelShowLevel.Text = "";
            timerLevel.Enabled = false;
        }
    }
}
