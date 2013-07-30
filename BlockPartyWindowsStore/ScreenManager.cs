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
        // Width of the virtual world in which coordinates are specified
        public const int WorldWidth = 400;

        // Height of the virtual world in which coorinates are specified
        public const int WorldHeight = 300;

        // Aspect ratio of the virtual world
        public const float WorldAspectRatio = 16 / 9;

        // Width of the actual screen
        public int ScreenWidth;

        // Height of the actual screen
        public int ScreenHeight;

        // Graphics device used to determine actual screen size
        GraphicsDevice graphicsDevice;

        public ScreenManager(GraphicsDevice graphicsDevice)
        {
            // Set the graphics device
            this.graphicsDevice = graphicsDevice;
            
            // Set the initial screen dimensions
            ScreenWidth = graphicsDevice.Viewport.Width;
            ScreenHeight = graphicsDevice.Viewport.Height;
        }

        public void Update()
        {
            // Update the screen dimensions
            ScreenWidth = graphicsDevice.Viewport.Width;
            ScreenHeight = graphicsDevice.Viewport.Height;
        }
    }
}
