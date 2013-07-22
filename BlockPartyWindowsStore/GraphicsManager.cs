using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class GraphicsManager
    {
        // Graphics device used to draw things
        GraphicsDevice graphicsDevice;

        // Sprite batch used to draw sprites
        SpriteBatch spriteBatch;

        // Texture used to render sprites
        Texture2D texture;

        // Sprite font used to render text
        SpriteFont spriteFont;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            // Set the graphics device
            this.graphicsDevice = graphicsDevice;

            // Create the sprite batch
            spriteBatch = new SpriteBatch(graphicsDevice);

            // Create the texture
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[1] { Color.White });

            // Create the sprite font
            spriteFont = contentManager.Load<SpriteFont>("SpriteFont");
        }

        public void UnloadContent()
        {
            spriteBatch.Dispose();
            texture.Dispose();
        }

        public void Begin()
        {
            Matrix scaleMatrix = Matrix.CreateScale((float)graphicsDevice.Viewport.Width / ScreenManager.WorldWidth, (float)graphicsDevice.Viewport.Height / ScreenManager.WorldHeight, 1.0f);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scaleMatrix);
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }

        public void DrawText(string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public void End()
        {
            spriteBatch.End();
        }
    }
}
