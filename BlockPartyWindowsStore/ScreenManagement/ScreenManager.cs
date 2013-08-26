using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class ScreenManager
    {
        public Game Game;

        public Viewport Screen;
        public Viewport World;
        const int worldWidth = 1600, worldHeight = 900;
        
        public GraphicsManager GraphicsManager;
        public InputManager InputManager;
        public AudioManager AudioManager;
        
        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();
        Screen screenToLoad;
        bool loadingScreen = false;

        public ScreenManager(Game game)
        {
            Game = game;

            // Setup the screen viewport and update it when the window size changes
            Screen = new Viewport(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            // Setup the world viewport
            World = new Viewport(0, 0, worldWidth, worldHeight);

            // Setup the other component managers
            GraphicsManager = new GraphicsManager(this);
            InputManager = new InputManager(this);
            AudioManager = new AudioManager(this);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Screen.Width = Game.GraphicsDevice.Viewport.Width;
            Screen.Height = Game.GraphicsDevice.Viewport.Height;
        }

        public void LoadContent()
        {
            GraphicsManager.LoadContent();
            AudioManager.LoadContent();
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

        public void Draw(GameTime gameTime)
        {
            // Draw screens
            foreach (Screen screen in screens)
            {
                screen.Draw(gameTime);
            }
        }
    }
}
