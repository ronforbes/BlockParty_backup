using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace BlockPartyWindowsStore
{
    public class GraphicsManager
    {
        Game game;
        
        public Viewport ScreenViewport;        
        public SpriteBatch SpriteBatch;
        public SpriteFont SpriteFont;
        public Matrix WorldToScreenScaleMatrix;

        public GraphicsManager(Game game)
        {
            this.game = game;

            // Setup the screen viewport and update it when the window size changes
            ScreenViewport = new Viewport(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            WorldToScreenScaleMatrix = Matrix.CreateScale((float)ScreenViewport.Width / game.WorldViewport.Width, (float)ScreenViewport.Height / game.WorldViewport.Height, 1.0f);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Update the screen viewport based on the new dimensions
            ScreenViewport.Width = game.GraphicsDevice.Viewport.Width;
            ScreenViewport.Height = game.GraphicsDevice.Viewport.Height;

            WorldToScreenScaleMatrix = Matrix.CreateScale((float)ScreenViewport.Width / game.WorldViewport.Width, (float)ScreenViewport.Height / game.WorldViewport.Height, 1.0f);
        }

        public void LoadContent()
        {
            // Create the sprite batch
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);

            // Create the sprite font
            SpriteFont = game.Content.Load<SpriteFont>("SpriteFont");
        }
    }
}
