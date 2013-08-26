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

        public SpriteFont SpriteFont;

        public Matrix WorldToScreenScaleMatrix;

        public GraphicsManager(ScreenManager screenManager)
        {
            this.screenManager = screenManager;

            WorldToScreenScaleMatrix = Matrix.CreateScale((float)screenManager.Screen.Width / screenManager.World.Width, (float)screenManager.Screen.Height / screenManager.World.Height, 1.0f);
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
            SpriteFont = screenManager.Game.Content.Load<SpriteFont>("SpriteFont");
        }
    }
}
