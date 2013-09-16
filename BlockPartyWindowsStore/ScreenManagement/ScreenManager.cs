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
        
        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();
        Screen screenToLoad;
        UnsupportedViewStateScreen unsupportedViewStateScreen;
        bool loadingScreen = false;        

        public ScreenManager(Game game)
        {
            Game = game;

            // Setup an unsupported view state screen to show when the game is snapped
            unsupportedViewStateScreen = new UnsupportedViewStateScreen(this);
        }

        public void LoadContent()
        {
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
            if (Game.ApplicationViewState != ApplicationViewState.Snapped)
            {
                // Read input
                Game.InputManager.Update();

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
        }

        public void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(ClearOptions.Stencil | ClearOptions.Target, Color.Transparent, 0, 0);

            // Only draw the screen if the app isn't snapped
            if (Game.ApplicationViewState != ApplicationViewState.Snapped)
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
        }
    }
}
