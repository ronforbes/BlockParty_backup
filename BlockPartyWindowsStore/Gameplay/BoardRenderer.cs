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
        public Rectangle Rectangle;

        Texture2D backgroundTexture;

        public BoardRenderer(Board board)
        {
            this.board = board;
            Rectangle = new Rectangle(board.Screen.ScreenManager.World.Width / 2 - board.Columns * board.Blocks[0, 0].Renderer.Width / 2, board.Screen.ScreenManager.World.Height / 2 - board.Rows * board.Blocks[0, 0].Renderer.Height / 2, board.Columns * board.Blocks[0, 0].Renderer.Width, board.Rows * board.Blocks[0, 0].Renderer.Height);
        }

        public void LoadContent()
        {
            backgroundTexture = board.Screen.ContentManager.Load<Texture2D>("Rectangle");
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
                    Vector2 origin = board.Screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString(Math.Ceiling(3 - board.CountdownTimeElapsed.TotalMilliseconds / 1000).ToString()) / 2;
                    board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, Math.Ceiling(3 - board.CountdownTimeElapsed.TotalMilliseconds / 1000).ToString(), new Vector2(board.Screen.ScreenManager.World.Width / 2, board.Screen.ScreenManager.World.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    break;

                case Board.BoardState.Playing:
                    DrawBlocks(gameTime);

                    // Draw celebrations
                    foreach (Celebration celebration in board.Celebrations)
                    {
                        celebration.Draw(gameTime);
                    }

                    // Draw particles
                    foreach (ParticleEmitter pe in board.ParticleEmitters)
                    {
                        pe.Draw(gameTime);
                    }

                    if (board.GoTimeElapsed < board.GoDuration)
                    {
                        origin = board.Screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString("Go!") / 2;
                        board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, "Go!", new Vector2(board.Screen.ScreenManager.World.Width / 2, board.Screen.ScreenManager.World.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    }

                    if (board.GameOverDelayDuration - board.GameOverDelayTimeElapsed <= TimeSpan.FromSeconds(3))
                    {
                        origin = board.Screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString(Math.Ceiling((board.GameOverDelayDuration - board.GameOverDelayTimeElapsed).TotalSeconds).ToString()) / 2;
                        board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, Math.Ceiling((board.GameOverDelayDuration - board.GameOverDelayTimeElapsed).TotalSeconds).ToString(), new Vector2(board.Screen.ScreenManager.World.Width / 2, board.Screen.ScreenManager.World.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    }
                    break;

                case Board.BoardState.GameOver:
                    DrawBlocks(gameTime);
                    origin = board.Screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString("Game Over!") / 2;
                    board.Screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.GraphicsManager.SpriteFont, "Game Over!", new Vector2(board.Screen.ScreenManager.World.Width / 2, board.Screen.ScreenManager.World.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    break;
            }
        }

        void DrawBackground()
        {
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.End();
            
            DepthStencilState depthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, depthStencilState, null, null, board.Screen.ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, Rectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.5f);

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.End();

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, board.Screen.ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);
        }

        void DrawBlocks(GameTime gameTime)
        {
            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.End();

            DepthStencilState depthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, depthStencilState, null, null, board.Screen.ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);

            for (int row = 0; row < board.Rows; row++)
            {
                for (int column = 0; column < board.Columns; column++)
                {
                    board.Blocks[row, column].Draw(gameTime);
                }
            }

            // Draw the next row of blocks
            for (int column = 0; column < board.Columns; column++)
            {
                board.NextBlocks[column].Draw(gameTime);
            }

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.End();

            board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, board.Screen.ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);
        }
    }
}
