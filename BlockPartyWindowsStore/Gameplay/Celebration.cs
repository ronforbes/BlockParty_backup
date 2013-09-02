using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class Celebration
    {
        Board board;
        string text; // Text to display in the celebration
        Rectangle position;
        TimeSpan timeElapsed; // Amount of time the celebration has been active
        readonly TimeSpan duration = TimeSpan.FromSeconds(1); // Duration of the celebration
        bool expired;

        public Celebration(Board board, string text, int row, int column)
        {
            this.board = board;
            this.text = text;
            position = new Rectangle(column * board.Blocks[0, 0].Renderer.Width, row * board.Blocks[0, 0].Renderer.Height, board.Blocks[0, 0].Renderer.Width, board.Blocks[0, 0].Renderer.Height);
            board.ParticleEmitters.Add(new ParticleEmitter(board.Screen, 50, new Rectangle(board.Blocks[row, column].Renderer.Rectangle.X + board.Blocks[0, 0].Renderer.Width / 2, board.Blocks[row, column].Renderer.Rectangle.Y + board.Blocks[0, 0].Renderer.Height / 2, 25, 25), new Vector2(-100f, -100f), new Vector2(100f, 100f), Vector2.Zero, Color.Black, Color.White, TimeSpan.FromSeconds(3)));
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;

            if (timeElapsed >= duration)
            {
                expired = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!expired)
            {
                int y = (int)Tween.Linear(timeElapsed.TotalMilliseconds, 0, (-1 * board.Blocks[0, 0].Renderer.Height), duration.TotalMilliseconds);
                byte rectangleR = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.Orange.R, -1 * Color.Orange.R, duration.TotalMilliseconds);
                byte rectangleG = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.Orange.G, -1 * Color.Orange.G, duration.TotalMilliseconds);
                byte rectangleB = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.Orange.B, -1 * Color.Orange.B, duration.TotalMilliseconds);
                byte rectangleA = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.Orange.A, -1 * Color.Orange.A, duration.TotalMilliseconds);
                byte textR = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.White.R, -1 * Color.White.R, duration.TotalMilliseconds);
                byte textG = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.White.G, -1 * Color.White.G, duration.TotalMilliseconds);
                byte textB = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.White.B, -1 * Color.White.B, duration.TotalMilliseconds);
                byte textA = (byte)Tween.Linear(timeElapsed.TotalMilliseconds, Color.White.A, -1 * Color.White.A, duration.TotalMilliseconds);

                // Adjust vertical position based on the board's raising state
                y -= (int)Tween.Linear(board.RaiseTimeElapsed.TotalMilliseconds, 0, board.Blocks[0, 0].Renderer.Height, board.RaiseDuration.TotalMilliseconds);

                board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(board.Screen.ScreenManager.GraphicsManager.BlankTexture, new Rectangle(board.Renderer.Rectangle.X + position.X, board.Renderer.Rectangle.Y + position.Y + y, position.Width, position.Height), null, new Color(rectangleR, rectangleG, rectangleB, rectangleA), 0f, Vector2.Zero, SpriteEffects.None, 0f);

                Vector2 origin = board.Screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString(text) / 2;
                board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, text, new Vector2(board.Renderer.Rectangle.X + position.X + board.Blocks[0, 0].Renderer.Width / 2, board.Renderer.Rectangle.Y + position.Y + board.Blocks[0, 0].Renderer.Height / 2 + y), new Color(textR, textG, textB, textA), 0f, origin, Vector2.One, SpriteEffects.None, 0f);
            }
        }
    }
}
