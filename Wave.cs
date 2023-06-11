using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GridIsland
{
    public class Wave
    {
        private int speed = 1000;
        private Vector2 pos;
        private Rectangle rect;
        private Rectangle warning;
        private double timer = 3;
        private double maxTime = 3;
        private bool begin = false;

        private bool warnPlayed = false;
        private bool wavePlayed = false;

        public static List<Wave> waves = new List<Wave>();
        public Wave(int x) 
        {
            pos = new Vector2(x, 700);
            rect = new Rectangle(x, (int)pos.Y, 110, 530);
            warning = new Rectangle(x, 240, 115, 240);
            warning.Offset(14, 0);
        }

        public Rectangle Rect { get => rect; set => rect = value; }
        public Vector2 Pos { get => pos; set => pos = value; }
        public Rectangle Warning { get => warning; set => warning = value; }
        public double Timer { get => timer; set => timer = value; }
        public double MaxTime { get => maxTime; set => maxTime = value; }

        public void Update(GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;

            timer -= dt;

            if (timer <= 0)
            {
                if (!wavePlayed)
                {
                    MusicBoards.waveSound.Play();
                    wavePlayed = true;
                }
                if (pos.Y > 200 && !begin)
                {
                    pos.Y -= speed * dt;
                }
                else
                {
                    begin = true;
                    pos.Y += speed * dt;
                }
            }
            else
            {
                if (!warnPlayed)
                {
                    MusicBoards.warningSound.Play();
                    warnPlayed = true;
                }
            }

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
            rect.Offset(14, 30);
        }
    }
}
