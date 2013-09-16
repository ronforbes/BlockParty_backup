using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    public class InputManager
    {
        Game game;

        // World position of the mouse
        public Point WorldPosition = new Point();

        // Screen position of the mouse
        public Point ScreenPosition = new Point();

        // Determines whether the mouse buttons are currently pressed
        public bool LeftButton, RightButton;

        // Determines whether the mouse buttons were just pressed this frame
        public bool LeftButtonPressed, RightButtonPressed;
            
        // Determines whether the mouse buttons were just released this frame was just released
        public bool LeftButtonReleased, RightButtonReleased;

        // Stores the previous mouse state to know if buttons were just pressed or released
        MouseState previousMouseState;

        public InputManager(Game game)
        {
            this.game = game;
            
            previousMouseState = Mouse.GetState();
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            ScreenPosition.X = mouseState.X;
            ScreenPosition.Y = mouseState.Y;

            // Scale mouse coordinates to world dimensions
            WorldPosition.X = ScreenPosition.X * game.WorldViewport.Width / game.GraphicsManager.ScreenViewport.Width;
            WorldPosition.Y = ScreenPosition.Y * game.WorldViewport.Height / game.GraphicsManager.ScreenViewport.Height;

            // Determine whether the buttons are currently pressed
            LeftButton = mouseState.LeftButton == ButtonState.Pressed;
            RightButton = mouseState.RightButton == ButtonState.Pressed;

            // Determine whether buttons were just pressed
            LeftButtonPressed = (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) ? true : false;
            LeftButtonReleased = (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) ? true : false;
            RightButtonPressed = (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released) ? true : false;
            RightButtonReleased = (mouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed) ? true : false;

            previousMouseState = mouseState;

            // Process touch input
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touchLocation in touchCollection)
            {
                LeftButtonPressed = false;
                LeftButtonReleased = false;

                WorldPosition.X = (int)touchLocation.Position.X * game.WorldViewport.Width / game.GraphicsManager.ScreenViewport.Width;
                WorldPosition.Y = (int)touchLocation.Position.Y * game.WorldViewport.Height / game.GraphicsManager.ScreenViewport.Height;

                ScreenPosition.X = (int)touchLocation.Position.X;
                ScreenPosition.Y = (int)touchLocation.Position.Y;

                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    LeftButton = true;
                    LeftButtonPressed = true;
                }
                if (touchLocation.State == TouchLocationState.Released)
                {
                    LeftButton = false;
                    LeftButtonReleased = true;
                }
            }
        }
    }
}
