using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; //jpg compressor

namespace Snake_CSharpGame
{
    public partial class snakeForm : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;
        int score;
        int highScore;

        Random rand= new Random();
        
        bool goLeft, goRight, goDown, goUp;     //initial false

        public snakeForm()
        {
            InitializeComponent();

            new Settings();
        }
        #region PressKey
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true; ;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }
        #endregion

        #region ReleaseKey
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
                //when key is realesed values back to false
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false; ;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }
        #endregion

        #region StartGame
        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }
        #endregion

        #region TakeSnapShot
        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = $"I scored: {score} and my HighScore is {highScore}";
            caption.Font = new Font("Calibri", 14, FontStyle.Regular);
            caption.ForeColor = Color.White;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake_Game_Snap";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;    //tells if name is valid or not (, # @)

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0,0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);     //delete caption for snap

            }
        }
        #endregion

        #region GameTimerEvent
        private void GameTimerEvent(object sender, EventArgs e)
        {
                //setting directions
            if (goLeft)
            {
                Settings.directions = "left"; 
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }

            for (int i = Snake.Count -1;  i >= 0; i--)
            {
                if (i == 0)     //head of snake
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].x--;
                            break;
                        case "right":
                            Snake[i].x++;
                            break;
                        case "down":
                            Snake[i].y++;
                            break;
                        case "up":
                            Snake[i].y--;
                            break;
                    }
                        //set boundaries
                    if (Snake[i].x < 0)
                    {
                        Snake[i].x = maxWidth;
                    }
                    if (Snake[i].x > maxWidth)
                    {
                        Snake[i].x = 0;
                    }
                    if (Snake[i].y < 0)
                    {
                        Snake[i].y = maxHeight;
                    }
                    if (Snake[i].y > maxHeight)
                    {
                        Snake[i].y = 0;
                    }

                    if (Snake[i].x == food.x && Snake[i].y == food.y)
                    {
                        EatFood();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                            // Snake[i] = head
                            // Snake[j] = body
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            GameOver();
                        }
                    }
                }
                else //body
                {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
            }
            picCanvas.Invalidate();

        }
        #endregion

        #region UpdatePictureBoxGraphics
        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
                //paint event
            Graphics canvas = e.Graphics;
            
            Brush snakeColour;

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i  == 0)    //acces snake
                {
                    snakeColour = Brushes.White;
                }
                else
                {
                    snakeColour = Brushes.White;
                }

                canvas.FillEllipse(snakeColour, new Rectangle
                        (
                            //define x y hiegth and width 
                        Snake[i].x * Settings.Width,
                        Snake[i].y * Settings.Height,
                        Settings.Width, Settings.Height
                        ));
            }
            canvas.FillEllipse(Brushes.White, new Rectangle
                        (
                            //define x y hiegth and width 
                        food.x * Settings.Width,
                        food.y * Settings.Height,
                        Settings.Width, Settings.Height
                        ));
        }
        #endregion

        #region RestartGame
        private void RestartGame()
        {
                //default values when game starts
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snake.Clear();

                //disable keys
            startButton.Enabled = false;
            snapButton.Enabled = false;
            

            score = 0;
            txtScore.Text = $"Score: {score}";

                //body snake
            Circle head = new Circle { x = 10, y = 5 };
            Snake.Add(head); 
                //head = index 0, add to  list
                //for loop to add body parts to snake
            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { x = rand.Next(2, maxWidth), y = rand.Next(2, maxHeight) };

            gameTimer.Start();
        }
        #endregion

        #region EatFood
        private void EatFood()
        {
            score += 1;
            txtScore.Text = $"Score: {score}";

            Circle body = new Circle
            {
                x = Snake[Snake.Count - 1].x,
                y = Snake[Snake.Count - 1].y
            };
            Snake.Add(body);

            food = new Circle { x = rand.Next(2, maxWidth), y = rand.Next(2, maxHeight) };

        }
        #endregion

        #region GameOver
        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            if (score > highScore)
            {
                highScore = score;
                txtHighScore.Text = $"High Score: {highScore}";
                txtHighScore.ForeColor = Color.White;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
        #endregion
    }
}
