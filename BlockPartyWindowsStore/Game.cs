using BlockPartyWindowsStore.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Windows.UI.ViewManagement;

namespace BlockPartyWindowsStore
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public Viewport WorldViewport;
        public GraphicsManager GraphicsManager;
        public InputManager InputManager;
        public AudioManager AudioManager;
        public ApplicationViewState ApplicationViewState;

        GraphicsDeviceManager graphicsDeviceManager;
        ScreenManager screenManager;
        FrameRateCounter frameRateCounter;

        const int defaultWorldWidth = 1600, defaultWorldHeight = 900;

        public Game()
        {
            // initialize the graphics device manager and setup the depth stencil buffer format
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

            Content.RootDirectory = "Content";

            // Setup the world viewport
            WorldViewport = new Viewport(0, 0, defaultWorldWidth, defaultWorldHeight);

            // Handle view state (snap, fill, full) changes
            ApplicationViewChanged += OnApplicationViewChanged;
        }

        void OnApplicationViewChanged(object sender, ViewStateChangedEventArgs e)
        {
            ApplicationViewState = e.ViewState;

            // Update the world viewport based on the view state
            if (ApplicationViewState == ApplicationViewState.FullScreenLandscape)
            {
                WorldViewport.Width = 1600;
                WorldViewport.Height = 900;
            }
            if (ApplicationViewState == ApplicationViewState.FullScreenPortrait)
            {
                WorldViewport.Width = 900;
                WorldViewport.Height = 1600;
            }
            if (ApplicationViewState == ApplicationViewState.Snapped)
            {
                WorldViewport.Width = GraphicsManager.ScreenViewport.Width;
                WorldViewport.Height = GraphicsManager.ScreenViewport.Height;
            }
            if (ApplicationViewState == ApplicationViewState.Filled)
            {
                WorldViewport.Width = 1600;
                WorldViewport.Height = 1200;
            }

            // Show the windows mouse if the game is snapped (to make it easier to change back to a supported state)
            IsMouseVisible = ApplicationViewState == ApplicationViewState.Snapped ? true : false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Setup the other component managers
            GraphicsManager = new GraphicsManager(this);
            InputManager = new InputManager(this);
            AudioManager = new AudioManager(this);
            screenManager = new ScreenManager(this);
            frameRateCounter = new FrameRateCounter(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GraphicsManager.LoadContent();
            AudioManager.LoadContent();
            screenManager.LoadContent();

            screenManager.AddScreen(new MainMenuScreen(screenManager));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            screenManager.Update(gameTime);
            frameRateCounter.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {            
            screenManager.Draw(gameTime);
            frameRateCounter.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
