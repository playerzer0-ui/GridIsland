using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg;

namespace GridIsland
{
    public class Player
    {
        public static Vector2 pos = new Vector2(380, 240);
        private KeyboardState old = Keyboard.GetState();
        private Rectangle rect = new Rectangle(380, 240, 30, 30);
        public static bool dead = false;
        private int jumpX = 62 * 60;
        private int jumpY = 58 * 60;
        public SpriteAnimation anim;

        public Rectangle Rect { get => rect; set => rect = value; }

        public Player(Texture2D anim) 
        {
            this.anim = new SpriteAnimation(anim, 2, 2);
        }

        public void Update(GameTime gt) 
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;

            if(kState.IsKeyDown(Keys.W) && old.IsKeyUp(Keys.W) && pos.Y > 240)
            {
                pos.Y -= jumpY * dt;
            }
            if (kState.IsKeyDown(Keys.S) && old.IsKeyUp(Keys.S) && pos.Y < 380)
            {
                pos.Y += jumpY * dt;
            }
            if (kState.IsKeyDown(Keys.A) && old.IsKeyUp(Keys.A) && pos.X > 380)
            {
                pos.X -= jumpX * dt;
            }
            if (kState.IsKeyDown(Keys.D) && old.IsKeyUp(Keys.D) && pos.X < 810)
            {
                pos.X += jumpX * dt;
            }


            old = kState;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
            rect.Offset(10, 10);
            anim.Position = pos;
            anim.Update(gt);
        }
    }
}
