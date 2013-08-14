using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class BoardRenderer
    {
        Board board;
        Rectangle rectangle;
        const int rectangleHorizontalMargin = 25;

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        double scoreDisplay;

        Texture2D backgroundTexture;

        public BoardRenderer(Board board)
        {
            this.board = board;
            rectangle = new Rectangle(board.Screen.ScreenManager.World.Width - board.Columns * board.Blocks[0, 0].Renderer.Width - rectangleHorizontalMargin, board.Screen.ScreenManager.World.Height / 2 - board.Rows * board.Blocks[0, 0].Renderer.Height / 2, board.Columns * board.Blocks[0, 0].Renderer.Width, board.Rows * board.Blocks[0, 0].Renderer.Height);
        }

        public void LoadContent()
        {
            backgroundTexture = board.Screen.ContentManager.Load<Texture2D>("BoardBackground");
        }

        public void Draw(GameTime gameTime)
        {
            DrawBackground();

            switch (board.State)
            {
                case Board.BoardState.Populating:
                    DrawBlocks(gameTime);
                    break;

                case Board.BoardState.CountingDown:
                    DrawBlocks(gameTime);
                    board.Screen.ScreenManager.GraphicsManager.DrawText(Math.Ceiling(3 - board.CountdownTimeElapsed.TotalMilliseconds / 1000).ToString(), new Vector2(board.Screen.ScreenManager.World.Width / 2, board.Screen.ScreenManager.World.Height / 2), Color.White, true);
                    break;

                case Board.BoardState.Playing:
                    DrawBlocks(gameTime);

                    // Draw the next row of blocks
                    for (int column = 0; column < board.Columns; column++)
                    {
                        //nextRowBlocks[column].Draw(gameTime, rows, column, new Vector2(screen.ScreenManager.WorldViewport.Width - columns * BlockRenderer.Width - 25, (int)(-1 * BlockRenderer.Height * raiseTimeElapsed.TotalMilliseconds / raiseDuration.TotalMilliseconds)));
                    }

                    // Draw the score display
                    scoreDisplay += (board.Score - scoreDisplay) * 0.1;
                    if (Math.Abs(board.Score - scoreDisplay) < 1)
                        scoreDisplay = board.Score;
                    board.Screen.ScreenManager.GraphicsManager.DrawText("Score: " + Math.Floor(scoreDisplay).ToString(), new Vector2(50, 200), Color.Orange, false);

                    // Draw the level display
                    board.Screen.ScreenManager.GraphicsManager.DrawText("Level: " + board.Level.ToString(), new Vector2(50, 250), Color.Orange, false);

                    // Draw celebrations
                    //foreach (Celebration celebration in celebrations)
                    //{
                    //    celebration.Draw(gameTime, new Vector2(screen.ScreenManager.WorldViewport.Width - columns * BlockRenderer.Width / 2, (int)(-1 * BlockRenderer.Height * raiseTimeElapsed.TotalMilliseconds / raiseDuration.TotalMilliseconds)));
                    //}

                    // Draw particles
                    foreach (ParticleEmitter pe in board.ParticleEmitters)
                    {
                        pe.Draw(gameTime);
                    }
                    break;

                case Board.BoardState.GameOver:
                    board.Screen.ScreenManager.GraphicsManager.DrawText("GAME OVER", new Vector2(board.Screen.ScreenManager.World.Width / 2, board.Screen.ScreenManager.World.Height / 2), Color.White, true);
                    break;
            }
        }

        void DrawBackground()
        {
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, rectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        void DrawBlocks(GameTime gameTime)
        {
            for (int row = 0; row < board.Rows; row++)
            {
                for (int column = 0; column < board.Columns; column++)
                {
                    board.Blocks[row, column].Draw(gameTime);
                }
            }
        }
    }
}
