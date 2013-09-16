using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Gameplay
{
    class BlockParticle
    {
        BlockRain blockRain;
        Rectangle rectangle;
        Vector2 velocity;
        Vector2 acceleration;
        Texture2D texture;
        public bool Active = true;
        static Random random = new Random();

        public BlockParticle(BlockRain blockRain)
        {
            this.blockRain = blockRain;

            int width = random.Next(blockRain.Screen.ScreenManager.Game.WorldViewport.Width > blockRain.Screen.ScreenManager.Game.WorldViewport.Height ?
                blockRain.Screen.ScreenManager.Game.WorldViewport.Height / 5 :
                blockRain.Screen.ScreenManager.Game.WorldViewport.Width / 5);
            int height = width;
            int x = random.Next(blockRain.Screen.ScreenManager.Game.WorldViewport.Width);
            int y = -1 * height;

            rectangle = new Rectangle(x, y, width, height);
            velocity = Vector2.Zero;
            acceleration = new Vector2(0, 0.25f);
            texture = blockRain.Textures[random.Next(blockRain.Textures.Count)];
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                velocity += acceleration;
                rectangle.Y += (int)velocity.Y;

                if (rectangle.Y >= blockRain.Screen.ScreenManager.Game.WorldViewport.Height)
                {
                    Active = false;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (Active)
            {
                blockRain.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(texture, rectangle, Color.White);
            }
        }
    }
}
