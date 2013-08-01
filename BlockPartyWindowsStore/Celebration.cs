using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class Celebration
    {
        string text; // Text to display in the celebration
        Vector2 position;
        double timeElapsed; // Amount of time the celebration has been active
        const double duration = 1000; // Duration of the celebration
        bool expired;

        public Celebration(string text, int row, int column)
        {
            this.text = text;
            position = new Vector2(column * Block.Width + Block.Width / 2, row * Block.Height + Block.Height / 2 - Block.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeElapsed >= duration)
            {
                expired = true;
            }
        }

        public void Draw(GameTime gameTime, GraphicsManager graphicsManager, Vector2 boardPosition)
        {
            if(!expired)
            {
                int y = (int)Tween.Linear(timeElapsed, position.Y, -1 * Block.Height, duration);
                byte rectangleR = (byte)Tween.Linear(timeElapsed, Color.Orange.R, -1 * Color.Orange.R, duration);
                byte rectangleG = (byte)Tween.Linear(timeElapsed, Color.Orange.G, -1 * Color.Orange.G, duration);
                byte rectangleB = (byte)Tween.Linear(timeElapsed, Color.Orange.B, -1 * Color.Orange.B, duration);
                byte rectangleA = (byte)Tween.Linear(timeElapsed, Color.Orange.A, -1 * Color.Orange.A, duration);
                byte textR = (byte)Tween.Linear(timeElapsed, Color.White.R, -1 * Color.White.R, duration);
                byte textG = (byte)Tween.Linear(timeElapsed, Color.White.G, -1 * Color.White.G, duration);
                byte textB = (byte)Tween.Linear(timeElapsed, Color.White.B, -1 * Color.White.B, duration);
                byte textA = (byte)Tween.Linear(timeElapsed, Color.White.A, -1 * Color.White.A, duration);

                graphicsManager.DrawRectangle("Blank", new Rectangle((int)(boardPosition.X + position.X), (int)(boardPosition.Y + y), (int)(Block.Width * 0.95), (int)(Block.Height * 0.95)), new Color(rectangleR, rectangleG, rectangleB, rectangleA), 0.0f, 1.0f);
                graphicsManager.DrawText(text, new Vector2(boardPosition.X + position.X, boardPosition.Y + y), new Color(textR, textG, textB, textA), true);
            }
        }
    }
}
