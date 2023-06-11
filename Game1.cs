using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace GridIsland
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D menu;
        private Texture2D waveSprite;
        private Texture2D ballSprite;
        private Texture2D playerSprite;
        private Texture2D option;
        private Texture2D pause;
        private Texture2D background;
        private double score = 0;
        private SpriteFont gameFont;

        private Rectangle retryRect;
        private Rectangle homeRect;
        private Rectangle pauseRect;
        private Rectangle buttonRect;
        private Rectangle resumeRect;
        private Texture2D pixel;
        private bool isPaused = false;

        private MouseState _mState;
        private MouseState old;

        private int scene = 0;

        Player player;
        Controller controller;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // sprites
            menu = Content.Load<Texture2D>("menu");
            ballSprite = Content.Load<Texture2D>("ball");
            waveSprite = Content.Load<Texture2D>("wave");
            playerSprite = Content.Load<Texture2D>("player");
            option = Content.Load<Texture2D>("option");
            pause = Content.Load<Texture2D>("pause");
            background = Content.Load<Texture2D>("background");
            gameFont = Content.Load<SpriteFont>("gameFont");

            //sounds
            MusicBoards.waveSound = Content.Load<SoundEffect>("music/waveSound");
            MusicBoards.warningSound = Content.Load<SoundEffect>("music/warningSound");
            MusicBoards.clickSound = Content.Load<SoundEffect>("music/clickSound");
            MusicBoards.ocean = Content.Load<Song>("music/ocean");
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(MusicBoards.ocean);


            //pixel
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            //rectangles
            buttonRect = new Rectangle(477, 440, 350, 100);
            pauseRect = new Rectangle(0, 0, 100, 100);
            retryRect = new Rectangle(456, 296, 170, 170);
            homeRect = new Rectangle(650, 296, 170, 170);
            resumeRect = new Rectangle(500, 240, 280, 35);

            //class
            player = new Player(playerSprite);
            controller = new Controller(ballSprite);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // USER INTERFACE
            _mState = Mouse.GetState();
            //menu
            if(_mState.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released && buttonRect.Contains(_mState.Position) && scene == 0)
            {
                scene = 1;
                MusicBoards.clickSound.Play();
            }
            //pause
            if(_mState.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released && pauseRect.Contains(_mState.Position))
            {
                isPaused = true;
                MusicBoards.clickSound.Play();
            }
            //retry
            if (_mState.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released && retryRect.Contains(_mState.Position) && isPaused)
            {
                Player.pos = new Vector2(380, 240);
                controller.Reset();
                score = 0;
                isPaused = false;
                MusicBoards.clickSound.Play();
            }
            //home
            if (_mState.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released && homeRect.Contains(_mState.Position) && isPaused)
            {
                Player.pos = new Vector2(380, 240);
                controller.Reset();
                scene = 0;
                score = 0;
                isPaused = false;
                MusicBoards.clickSound.Play();
            }
            //resume
            if (_mState.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released && resumeRect.Contains(_mState.Position) && isPaused)
            {
                isPaused = false;
                MusicBoards.clickSound.Play();
            }

            //game loop
            if (!Player.dead && !isPaused)
            {
                score += gameTime.ElapsedGameTime.TotalSeconds;
                controller.Update(gameTime, player.Rect);
                player.Update(gameTime);
            }

            old = _mState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (scene)
            {
                case 0:
                    _spriteBatch.Draw(menu, new Vector2(0, 0), Color.White);
                    _spriteBatch.Draw(pixel, buttonRect, Color.Transparent);
                    break;
                case 1:
                    _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                    foreach(Ball ball in Ball.balls)
                    {
                        ball.anim.Draw(_spriteBatch);
                        //_spriteBatch.Draw(pixel, ball.Rect, new Color(255, 0, 0, 128));
                    }

                    player.anim.Draw(_spriteBatch);
                    //_spriteBatch.Draw(pixel, player.Rect, new Color(255, 0, 0, 128));
                    
                    foreach(Wave wave in Wave.waves)
                    {
                        _spriteBatch.Draw(waveSprite, wave.Pos, Color.White);

                        if (wave.Timer >= 0)
                        {
                            _spriteBatch.Draw(pixel, wave.Warning, new Color(255, 0, 0, 128));
                        }
                    }

                    _spriteBatch.Draw(pause, new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(gameFont, "score: " + Math.Floor(score), new Vector2(110, 0), Color.Black);
                    //_spriteBatch.Draw(pixel, pauseRect, new Color(255, 0, 0, 128));

                    if (isPaused || Player.dead)
                    {
                        _spriteBatch.Draw(option, new Vector2(440, 160), Color.White);
                        //_spriteBatch.Draw(pixel, homeRect, new Color(255, 0, 0, 128));
                        //_spriteBatch.Draw(pixel, retryRect, new Color(255, 0, 0, 128));
                        _spriteBatch.Draw(pixel, resumeRect, new Color(0, 0, 0, 128));
                        if (!Player.dead)
                        {
                            _spriteBatch.DrawString(gameFont, "click here to resume", new Vector2(500, 240), Color.White);
                        }
                        else
                        {
                            _spriteBatch.DrawString(gameFont, "GAME OVER", new Vector2(500, 240), Color.White);
                        }
                    }

                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}