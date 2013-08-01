using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BlockPartyWindowsStore
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public int WorldWidth = 1600;
        public int WorldHeight = 900;
        GraphicsDeviceManager graphicsDeviceManager;
        GraphicsManager graphicsManager;
        ScreenManager screenManager;
        MouseManager mouseManager;
        SoundManager soundManager;
        Board board;

        public Game()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphicsManager = new GraphicsManager();
            screenManager = new ScreenManager(GraphicsDevice);
            mouseManager = new MouseManager(screenManager);
            soundManager = new SoundManager();
            board = new Board(GraphicsDevice, soundManager);
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphicsManager.LoadContent(GraphicsDevice, Content);
            soundManager.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            graphicsManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            screenManager.Update();
            mouseManager.Update(screenManager);

            // Add a bit of a delay until there's a menu in front of this
            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                board.Update(gameTime, mouseManager, soundManager);
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            graphicsManager.Begin();

            board.Draw(gameTime, graphicsManager);

            graphicsManager.End();

            base.Draw(gameTime);
        }
    }
}
