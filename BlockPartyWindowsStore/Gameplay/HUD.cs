using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Gameplay
{
    class HUD
    {
        Screen screen;
        Board board;
        Rectangle rectangle;
        double scoreDisplay;
        Texture2D backgroundTexture;

        public HUD(Screen screen, Board board)
        {
            this.screen = screen;
            this.board = board;
            rectangle = new Rectangle(screen.ScreenManager.Game.WorldViewport.Width - 345, 45, 300, 200);
        }

        public void LoadContent()
        {
            backgroundTexture = screen.ContentManager.Load<Texture2D>("Rectangle");
        }

        public void Draw(GameTime gameTime)
        {
            // Draw the HUD background
            screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(backgroundTexture, rectangle, Color.White);

            // Draw the score display
            scoreDisplay += (board.Stats.Score - scoreDisplay) * 0.1;
            
            if (Math.Abs(board.Stats.Score - scoreDisplay) < 1)
                scoreDisplay = board.Stats.Score;

            Vector2 scoreLabelPosition = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y);
            Vector2 scoreLabelOrigin = new Vector2(screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Score").X / 2, 0);
            Vector2 scoreOrigin = new Vector2(screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(Math.Floor(scoreDisplay).ToString()).X / 2, 0);
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Score", scoreLabelPosition, Color.White, 0f, scoreLabelOrigin, Vector2.One, SpriteEffects.None, 0f);
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, Math.Floor(scoreDisplay).ToString(), new Vector2(scoreLabelPosition.X, scoreLabelPosition.Y + 20), Color.White, 0f, scoreOrigin, Vector2.One, SpriteEffects.None, 0f);

            // Draw the level display
            Vector2 levelLabelPosition = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + 60);
            Vector2 levelLabelOrigin = new Vector2(screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Level").X / 2, 0);
            Vector2 levelOrigin = new Vector2(screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(board.Stats.Level.ToString()).X / 2, 0);
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Level", levelLabelPosition, Color.White, 0f, levelLabelOrigin, Vector2.One, SpriteEffects.None, 0f);
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, board.Stats.Level.ToString(), new Vector2(levelLabelPosition.X, levelLabelPosition.Y + 20), Color.White, 0f, levelOrigin, Vector2.One, SpriteEffects.None, 0f);

            // Draw the stop time
            Vector2 stopLabelPosition = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + 120);
            Vector2 stopLabelOrigin = new Vector2(screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Stop").X / 2, 0);
            Vector2 stopOrigin = new Vector2(screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(board.StopTimeRemaining.ToString()).X / 2, 0);
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Stop", stopLabelPosition, Color.White, 0f, stopLabelOrigin, Vector2.One, SpriteEffects.None, 0f);
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, board.StopTimeRemaining.ToString(), new Vector2(stopLabelPosition.X, stopLabelPosition.Y + 20), Color.White, 0f, stopOrigin, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
