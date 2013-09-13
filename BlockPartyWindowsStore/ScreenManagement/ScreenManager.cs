using BlockPartyWindowsStore.Screens;
using BlockPartyWindowsStore.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace BlockPartyWindowsStore
{
    class ScreenManager
    {
        public Game Game;
        public Viewport Screen;
        public Viewport World;
        public GraphicsManager GraphicsManager;
        public InputManager InputManager;
        public AudioManager AudioManager;

        const int defaultWorldWidth = 1600, defaultWorldHeight = 900;

        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();
        Screen screenToLoad;
        UnsupportedViewStateScreen unsupportedViewStateScreen;
        bool loadingScreen = false;
        ApplicationViewState applicationViewState;
        FrameRateCounter frameRateCounter;

        public ScreenManager(Game game)
        {
            Game = game;

            // Setup the screen viewport and update it when the window size changes
            Screen = new Viewport(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            
            // Handle view state (snap, fill, full) changes
            game.ApplicationViewChanged += game_ApplicationViewChanged;

            // Setup the world viewport
            World = new Viewport(0, 0, defaultWorldWidth, defaultWorldHeight);

            // Setup the other component managers
            GraphicsManager = new GraphicsManager(this);
            InputManager = new InputManager(this);
            AudioManager = new AudioManager(this);
            frameRateCounter = new FrameRateCounter(this);

            // Setup an unsupported view state screen to show when the game is snapped
            unsupportedViewStateScreen = new UnsupportedViewStateScreen(this);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Update the screen viewport based on the new dimensions
            Screen.Width = Game.GraphicsDevice.Viewport.Width;
            Screen.Height = Game.GraphicsDevice.Viewport.Height;

            // Update the world viewport based on the view state
            if (applicationViewState == ApplicationViewState.FullScreenLandscape)
            {
                World.Width = 1600;
                World.Height = 900;
            }
            if (applicationViewState == ApplicationViewState.FullScreenPortrait)
            {
                World.Width = 900;
                World.Height = 1600;
            }
            if (applicationViewState == ApplicationViewState.Snapped)
            {
                World.Width = Screen.Width;
                World.Height = Screen.Height;
            }
            if (applicationViewState == ApplicationViewState.Filled)
            {
                World.Width = 400;
                World.Height = 300;
            }
        }

        void game_ApplicationViewChanged(object sender, ViewStateChangedEventArgs e)
        {
            applicationViewState = e.ViewState;

            // Show the windows mouse if the game is snapped (to make it easier to change back to a supported state)
            if (applicationViewState == ApplicationViewState.Snapped)
            {
                Game.IsMouseVisible = true;
            }
            else
            {
                Game.IsMouseVisible = false;
            }
        }

        public void LoadContent()
        {
            GraphicsManager.LoadContent();
            AudioManager.LoadContent();

            unsupportedViewStateScreen.LoadContent();
        }

        public void AddScreen(Screen screen)
        {
            screen.LoadContent();

            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            screen.UnloadContent();

            screens.Remove(screen);
        }

        public void LoadScreen(Screen screen)
        {
            // Transition out of the current screens
            foreach (Screen s in screens)
            {
                s.TransitionOff();
            }

            screenToLoad = screen;
            loadingScreen = true;
        }

        public void Update(GameTime gameTime)
        {
            // Only update the screen if the app isn't snapped
            if (applicationViewState != ApplicationViewState.Snapped)
            {
                // Read input
                InputManager.Update();

                // Make a copy of the list of screens to allow for adds/removes
                // not interfering with updates
                screensToUpdate.Clear();

                foreach (Screen screen in screens)
                {
                    screensToUpdate.Add(screen);
                }

                while (screensToUpdate.Count > 0)
                {
                    // Pop the topmost screen off the stack
                    Screen screen = screensToUpdate[screensToUpdate.Count - 1];
                    screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                    // Update the screen
                    screen.Update(gameTime);
                    screen.HandleInput(gameTime);
                }

                if (loadingScreen)
                {
                    if (screens.Count == 0)
                    {
                        AddScreen(screenToLoad);
                        loadingScreen = false;
                    }
                }
            }
            else
            {
                unsupportedViewStateScreen.Update(gameTime);
            }

            frameRateCounter.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // Only draw the screen if the app isn't snapped
            if (applicationViewState != ApplicationViewState.Snapped)
            {
                // Draw screens
                foreach (Screen screen in screens)
                {
                    screen.Draw(gameTime);
                }
            }
            else
            {
                unsupportedViewStateScreen.Draw(gameTime);
            }

            frameRateCounter.Draw(gameTime);
        }
    }
}
