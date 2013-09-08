using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class InputManager
    {
        ScreenManager screenManager;

        // World position of the mouse
        public int WorldX, WorldY;

        // Screen position of the mouse
        public int ScreenX, ScreenY;

        // Determines whether the mouse buttons are currently pressed
        public bool LeftButton, RightButton;

        // Determines whether the mouse buttons were just pressed this frame
        public bool LeftButtonPressed, RightButtonPressed;
            
        // Determines whether the mouse buttons were just released this frame was just released
        public bool LeftButtonReleased, RightButtonReleased;

        // Stores the previous mouse state to know if buttons were just pressed or released
        MouseState previousMouseState;

        TouchCollection previousTouchCollection;

        public InputManager(ScreenManager screenManager)
        {
            this.screenManager = screenManager;

            previousMouseState = Mouse.GetState();
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            // Scale mouse coordinates to world dimensions
            WorldX = mouseState.X * screenManager.World.Width / screenManager.Screen.Width;
            WorldY = mouseState.Y * screenManager.World.Height / screenManager.Screen.Height;

            ScreenX = mouseState.X;
            ScreenY = mouseState.Y;

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

                WorldX = (int)touchLocation.Position.X * screenManager.World.Width / screenManager.Screen.Width;
                WorldY = (int)touchLocation.Position.Y * screenManager.World.Height / screenManager.Screen.Height;

                ScreenX = (int)touchLocation.Position.X;
                ScreenY = (int)touchLocation.Position.Y;

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
