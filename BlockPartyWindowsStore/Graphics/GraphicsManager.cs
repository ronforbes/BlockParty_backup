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
        ScreenManager screenManager;
        
        public SpriteBatch SpriteBatch;
        
        public Texture2D BlankTexture;
        public Texture2D ParticleTexture;

        SpriteFont spriteFont;

        public GraphicsManager(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        public void LoadContent()
        {
            // Create the sprite batch
            SpriteBatch = new SpriteBatch(screenManager.Game.GraphicsDevice);

            // Create the blank texture
            BlankTexture = new Texture2D(screenManager.Game.GraphicsDevice, 1, 1);
            BlankTexture.SetData(new Color[] { Color.White });

            // Create the particle texture
            ParticleTexture = screenManager.Game.Content.Load<Texture2D>("Particle");

            // Create the sprite font
            spriteFont = screenManager.Game.Content.Load<SpriteFont>("SpriteFont");
        }

        public void UnloadContent()
        {
            // textures are disposed by the global content manager being unloaded
        }

        public void Begin()
        {
            Matrix scaleMatrix = Matrix.CreateScale((float)screenManager.Screen.Width / screenManager.World.Width, (float)screenManager.Screen.Height / screenManager.World.Height, 1.0f);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, scaleMatrix);
        }

        public void DrawFullscreenSprite(Texture2D texture, Color color)
        {
            SpriteBatch.Draw(texture, screenManager.World.Bounds, color);
        }

        public void DrawText(string text, Vector2 position, Color color, bool centered)
        {
            Vector2 size = spriteFont.MeasureString(text);
            SpriteBatch.DrawString(spriteFont, text, position - size / 2, color);
        }

        public void End()
        {
            SpriteBatch.End();
        }
    }
}
