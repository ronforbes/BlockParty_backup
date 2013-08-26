﻿using Microsoft.Xna.Framework;
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
            rectangle = new Rectangle(screen.ScreenManager.World.Width - 345, 45, 300, 200);
        }

        public void LoadContent()
        {
            backgroundTexture = screen.ContentManager.Load<Texture2D>("Rectangle");
        }

        public void Draw(GameTime gameTime)
        {
            // Draw the HUD background
            screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, rectangle, Color.White);

            // Draw the score display
            scoreDisplay += (board.Score - scoreDisplay) * 0.1;
            
            if (Math.Abs(board.Score - scoreDisplay) < 1)
                scoreDisplay = board.Score;

            Vector2 scoreLabelPosition = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y);
            Vector2 scoreLabelOrigin = new Vector2(screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString("Score:").X / 2, 0);
            Vector2 scoreOrigin = new Vector2(screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString(Math.Floor(scoreDisplay).ToString()).X / 2, 0);
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, "Score:", scoreLabelPosition, Color.White, 0f, scoreLabelOrigin, Vector2.One, SpriteEffects.None, 0f);
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, Math.Floor(scoreDisplay).ToString(), new Vector2(scoreLabelPosition.X, scoreLabelPosition.Y + 20), Color.White, 0f, scoreOrigin, Vector2.One, SpriteEffects.None, 0f);

            // Draw the level display
            Vector2 levelLabelPosition = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + 100);
            Vector2 levelLabelOrigin = new Vector2(screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString("Level:").X / 2, 0);
            Vector2 levelOrigin = new Vector2(screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString(board.Level.ToString()).X / 2, 0);
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, "Level:", levelLabelPosition, Color.White, 0f, levelLabelOrigin, Vector2.One, SpriteEffects.None, 0f);
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, board.Level.ToString(), new Vector2(levelLabelPosition.X, levelLabelPosition.Y + 20), Color.White, 0f, levelOrigin, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
