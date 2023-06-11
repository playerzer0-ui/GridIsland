using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg;
using System.Collections.Generic;

namespace GridIsland
{
    public class Ball
    {
        private int speed = 240;
        private Vector2 pos;
        private Vector2 distance;
        private Rectangle rect;

        public static List<Ball> balls = new List<Ball>();

        public SpriteAnimation anim;
        public Ball(Texture2D anim, Vector2 playerPos, int x, int y)
        {
            pos = new Vector2(x, y);
            rect = new Rectangle(x, y, 10, 10);
            this.anim = new SpriteAnimation(anim, 4, 8);
            distance = playerPos - pos;
            distance.Normalize();
        }

        public Vector2 Pos { get => pos; set => pos = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
        public Vector2 Distance { get => distance; set => distance = value; }


        public void Update(GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;

            pos += distance * speed * dt;

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
            rect.Offset(5, 5);
            anim.Position = pos;
            anim.Update(gt);
        }
    }
}
