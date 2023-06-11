using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GridIsland
{
    public class Controller
    {
        Random rg = new Random();
        private double ballTimer = 3;
        private double ballMaxTime = 3;
        private double waveTimer = 10;
        private double waveMaxTime = 10;
        private Texture2D anim;
        private int[] arr = {367, 490, 609, 730};

        public Controller(Texture2D anim)
        {
            this.anim = anim;
        }

        public void Update(GameTime gt, Rectangle playerRect)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            int ballIndex = Ball.balls.Count;
            int waveIndex = Wave.waves.Count;
            ballTimer -= dt;
            waveTimer -= dt;

            if(ballTimer <= 0)
            {
                ballTimer = ballMaxTime;
                if(ballMaxTime > 1)
                {
                    ballMaxTime -= 0.05;
                }
                Ball.balls.Add(new Ball(anim, Player.pos, rg.Next(-100, 1280), rg.Next(-300, -100)));
            }

            if(waveTimer <= 0)
            {
                waveTimer = waveMaxTime;
                if(waveMaxTime > 2)
                {
                    waveMaxTime -= 0.3;
                }
                Wave.waves.Add(new Wave(arr[rg.Next(0, 4)]));
            }

            //update ball
            for (int i = 0; i < ballIndex; i++)
            {
                Ball.balls[i].Update(gt);

                if (Ball.balls[i].Rect.Intersects(playerRect))
                {
                    Player.dead = true;
                }
                if (Ball.balls[i].Pos.Y > 750)
                {
                    Ball.balls.RemoveAt(i);
                    ballIndex--;
                }

            }

            //update wave
            for (int i = 0; i < waveIndex; i++)
            {
                Wave.waves[i].Update(gt);

                if (Wave.waves[i].Rect.Intersects(playerRect))
                {
                    Player.dead = true;
                }
                if (Wave.waves[i].Pos.Y > 750)
                {
                    Wave.waves.RemoveAt(i);
                    waveIndex--;
                }
            }

        }

        public void Reset()
        {
            ballTimer = 3;
            ballMaxTime = 3;
            waveTimer = 10;
            waveMaxTime = 10;
            Ball.balls.Clear();
            Wave.waves.Clear();
            Player.dead = false;
        }
    }
}
