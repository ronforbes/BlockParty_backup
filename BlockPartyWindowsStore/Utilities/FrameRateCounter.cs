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
        Game game;
        int frameCounter = 0;
        int framesPerSecond = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(Game game)
        {
            this.game = game;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime >= TimeSpan.FromSeconds(1))
            {
                // Set the frame rate
                framesPerSecond = frameCounter;                               

                // Reset the frame counters
                frameCounter = 0;

                // Reset the elapsed time
                elapsedTime = TimeSpan.Zero; 
            }
        }

        public void Draw(GameTime gameTime)
        {
            frameCounter++;

            game.GraphicsManager.SpriteBatch.Begin();
            game.GraphicsManager.SpriteBatch.DrawString(game.GraphicsManager.SpriteFont, "Frames per second: " + framesPerSecond.ToString(), Vector2.Zero, Color.White);
            game.GraphicsManager.SpriteBatch.End();
        }
    }
}
