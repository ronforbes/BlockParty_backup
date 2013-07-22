using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class MouseManager
    {
        // World position of the mouse
        public int X;

        public int Y;

        // Determines whether the left button is currently pressed
        public bool LeftButton;

        // Determines whether the right button is currently pressed
        public bool RightButton;

        // Determines whether the left button was just pressed
        public bool LeftButtonPressed;

        // Determines whether the left button was just released
        public bool LeftButtonReleased;

        // Determines whether the right button was just pressed
        public bool RightButtonPressed;

        // Determines whether the right button was just released
        public bool RightButtonReleased;

        // Stores the previous mouse state to know if buttons were just pressed or released
        MouseState previousMouseState;

        public MouseManager(ScreenManager screenManager)
        {
            previousMouseState = Mouse.GetState();

            Update(screenManager);
        }

        public void Update(ScreenManager screenManager)
        {
            MouseState mouseState = Mouse.GetState();

            // Scale mouse coordinates to world dimensions
            X = mouseState.X * ScreenManager.WorldWidth / screenManager.ScreenWidth;
            Y = mouseState.Y * ScreenManager.WorldHeight / screenManager.ScreenHeight;

            // Determine whether the buttons are currently pressed
            LeftButton = mouseState.LeftButton == ButtonState.Pressed;
            RightButtonReleased = mouseState.RightButton == ButtonState.Pressed;

            // Determine whether buttons were just pressed
            LeftButtonPressed = (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) ? true : false;
            LeftButtonReleased = (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) ? true : false;
            RightButtonPressed = (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released) ? true : false;
            RightButtonReleased = (mouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed) ? true : false;

            previousMouseState = mouseState;
        }
    }
}
