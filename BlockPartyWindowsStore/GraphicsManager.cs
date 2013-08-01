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
        Texture2D blankTexture;

        /// <summary>
        /// Block textures
        /// </summary>
        Texture2D circleTexture;
        Texture2D diamondTexture;
        Texture2D heartTexture;
        Texture2D starTexture;
        Texture2D triangleTexture;

        // Sprite font used to render text
        SpriteFont spriteFont;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            // Set the graphics device
            this.graphicsDevice = graphicsDevice;

            // Create the sprite batch
            spriteBatch = new SpriteBatch(graphicsDevice);

            // Create the texture
            blankTexture = new Texture2D(graphicsDevice, 1, 1);
            blankTexture.SetData(new Color[1] { Color.White });

            // Load the block textures
            circleTexture = contentManager.Load<Texture2D>("Circle");
            //diamondTexture = contentManager.Load<Texture2D>("Diamond");
            //heartTexture = contentManager.Load<Texture2D>("Heart");
            //starTexture = contentManager.Load<Texture2D>("Star");
            //triangleTexture = contentManager.Load<Texture2D>("Triangle");

            // Create the sprite font
            spriteFont = contentManager.Load<SpriteFont>("SpriteFont");
        }

        public void UnloadContent()
        {
            spriteBatch.Dispose();
            blankTexture.Dispose();
        }

        public void Begin()
        {
            Matrix scaleMatrix = Matrix.CreateScale((float)graphicsDevice.Viewport.Width / ScreenManager.WorldWidth, (float)graphicsDevice.Viewport.Height / ScreenManager.WorldHeight, 1.0f);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scaleMatrix);
        }

        public void DrawRectangle(string textureId, Rectangle rectangle, Color color, float rotation, float scale)
        {
            // Select the appropriate texture
            Texture2D texture = null;
            switch (textureId)
            {
                default: texture = blankTexture; break;
                case "Blank": texture = blankTexture; break;
                case "Circle": texture = circleTexture; break;
                //case "Circle": texture = circleTexture; break;
                //case "Diamond": texture = diamondTexture; break;
                //case "Heart": texture = heartTexture; break;
                //case "Star": texture = starTexture; break;
                //case "Triangle": texture = triangleTexture; break;
            }

            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, (int)(rectangle.Width * scale), (int)(rectangle.Height * scale)), null, color, rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 1.0f);
        }

        public void DrawText(string text, Vector2 position, Color color, bool centered)
        {
            Vector2 size = spriteFont.MeasureString(text);
            spriteBatch.DrawString(spriteFont, text, position - size / 2, color);
        }

        public void End()
        {
            spriteBatch.End();
        }
    }
}
