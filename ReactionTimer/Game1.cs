using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace ReactionTimer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MouseState oldState;
        private bool gameStarted = false;
        private bool colorChanged = false;
        Stopwatch sw = new Stopwatch();
        Random rnd = new Random();
        float waitBeforeChangingColor = 0;
        private SpriteFont font;

        // Time to sleep before changing the color in milliseconds
        int sleepMin = 1000;
        int sleepMax = 2000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.IsFullScreen = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            GraphicsDevice.Clear(Color.Black);
            font = Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState newState = Mouse.GetState();

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                if (gameStarted)
                {
                    if (colorChanged)
                    {
                        // Draw reaction time
                        sw.Stop();
                        string time = sw.ElapsedMilliseconds.ToString() + " ms";
                        spriteBatch.Begin();
                        spriteBatch.DrawString(font, time, new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(time).X / 2), (Window.ClientBounds.Height / 2) - (font.MeasureString(time).Y / 2)), Color.Black);
                        spriteBatch.End();
                    }
                    else
                    {
                        waitBeforeChangingColor = 0;
                        string text = "Too soon";
                        spriteBatch.Begin();
                        spriteBatch.DrawString(font, text, new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2), (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2)), Color.White);
                        spriteBatch.End();
                    }
                }
                else {
                    // Starting game
                    gameStarted = true;
                    int i = rnd.Next(sleepMin, sleepMax);
                    waitBeforeChangingColor = i;
                }
            }

            if (waitBeforeChangingColor > 0)
            {
                waitBeforeChangingColor -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (waitBeforeChangingColor <= 0)
                {
                    waitBeforeChangingColor = 0;
                    sw.Start();
                    GraphicsDevice.Clear(Color.Yellow);
                    colorChanged = true;
                }
            }

            oldState = newState;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here



            base.Draw(gameTime);
        }
    }
}
