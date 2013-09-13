using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Utilities
{
    class FrameRateCounter
    {
        ScreenManager screenManager;
        int updateCounter = 0;
        int drawCounter = 0;
        int updatesPerSecond = 0;
        int drawsPerSecond = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        public void Update(GameTime gameTime)
        {
            updateCounter++;

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                // Set the update and draw frame rates
                updatesPerSecond = updateCounter;
                drawsPerSecond = drawCounter;                               

                // Reset the update and draw frame counters
                updateCounter = 0;
                drawCounter = 0;

                // Reset the elapsed time
                elapsedTime = TimeSpan.Zero; 
            }
        }

        public void Draw(GameTime gameTime)
        {
            drawCounter++;

            screenManager.GraphicsManager.SpriteBatch.Begin();
            screenManager.GraphicsManager.SpriteBatch.DrawString(screenManager.GraphicsManager.SpriteFont, "Updates per second: " + updatesPerSecond.ToString(), Vector2.Zero, Color.White);
            screenManager.GraphicsManager.SpriteBatch.DrawString(screenManager.GraphicsManager.SpriteFont, "Draws per second: " + drawsPerSecond.ToString(), new Vector2(0, 20), Color.White);
            screenManager.GraphicsManager.SpriteBatch.End();
        }
    }
}
