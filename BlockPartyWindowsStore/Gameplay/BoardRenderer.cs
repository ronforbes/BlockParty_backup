using BlockPartyWindowsStore.Gameplay;
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
            Rectangle = new Rectangle(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 - board.Columns * board.Blocks[0, 0].Renderer.Width / 2, board.Screen.ScreenManager.Game.WorldViewport.Height / 2 - board.Rows * board.Blocks[0, 0].Renderer.Height / 2, board.Columns * board.Blocks[0, 0].Renderer.Width, board.Rows * board.Blocks[0, 0].Renderer.Height);
        }

        public void LoadContent()
        {
            backgroundTexture = board.Screen.ContentManager.Load<Texture2D>("Rectangle");
        }

        public void Update(GameTime gameTime)
        {
            // Determine whether blocks should shake based on height of each column
            for (int column = 0; column < board.Columns; column++)
            {
                if (board.Blocks[board.Rows / 4, column].State != Block.BlockState.Empty)
                {
                    for (int row = 0; row < board.Rows; row++)
                    {
                        board.Blocks[row, column].Renderer.Shaking = true;
                    }
                }
                else
                {
                    for (int row = 0; row < board.Rows; row++)
                    {
                        board.Blocks[row, column].Renderer.Shaking = false;
                    }
                }
            }
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
                    Vector2 origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(Math.Ceiling(3 - board.CountdownTimeElapsed.TotalMilliseconds / 1000).ToString()) / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, Math.Ceiling(3 - board.CountdownTimeElapsed.TotalMilliseconds / 1000).ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, board.Screen.ScreenManager.Game.WorldViewport.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    break;

                case Board.BoardState.Playing:
                    DrawBlocks(gameTime);

                    board.CelebrationManager.Draw(gameTime);

                    // Draw particles
                    foreach (ParticleEmitter pe in board.ParticleEmitters)
                    {
                        pe.Draw(gameTime);
                    }

                    if (board.GoTimeElapsed < board.GoDuration)
                    {
                        origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Go!") / 2;
                        board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Go!", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, board.Screen.ScreenManager.Game.WorldViewport.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    }

                    if (board.GameOverDelayDuration - board.GameOverDelayTimeElapsed <= TimeSpan.FromSeconds(3))
                    {
                        origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(Math.Ceiling((board.GameOverDelayDuration - board.GameOverDelayTimeElapsed).TotalSeconds).ToString()) / 2;
                        board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, Math.Ceiling((board.GameOverDelayDuration - board.GameOverDelayTimeElapsed).TotalSeconds).ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, board.Screen.ScreenManager.Game.WorldViewport.Height / 2), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    }
                    break;

                case Board.BoardState.GameOver:
                    DrawBlocks(gameTime);
                    
                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Game Over!") / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Game Over!", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + 20), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    
                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Time elapsed") / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Time elapsed", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + 60), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    
                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(board.Stats.TimeElapsed.ToString()) / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, board.Stats.TimeElapsed.ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + 80), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    
                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Blocks matched") / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Blocks matched", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + 120), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
                    
                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(board.Stats.BlocksMatched.ToString()) / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, board.Stats.BlocksMatched.ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + 140), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);

                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Combos") / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Combos", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + 180), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);

                    int y = 180;
                    foreach (int key in board.Stats.ComboBreakdown.Keys)
                    {
                        if (board.Stats.ComboBreakdown[key] > 0)
                        {
                            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, key.ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 - 100, Rectangle.Y + y), Color.White, 0f, Vector2.One, Vector2.One, SpriteEffects.None, 0f);
                            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, board.Stats.ComboBreakdown[key].ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 + 100, Rectangle.Y + y), Color.White, 0f, Vector2.One, Vector2.One, SpriteEffects.None, 0f);
                            y += 20;
                        }
                    }

                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Chains") / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Chains", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + y + 40), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);

                    y += 40;
                    foreach (int key in board.Stats.ChainBreakdown.Keys)
                    {
                        if (board.Stats.ChainBreakdown[key] > 0)
                        {
                            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, key.ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 - 100, Rectangle.Y + y), Color.White, 0f, Vector2.One, Vector2.One, SpriteEffects.None, 0f);
                            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, board.Stats.ChainBreakdown[key].ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 + 100, Rectangle.Y + y), Color.White, 0f, Vector2.One, Vector2.One, SpriteEffects.None, 0f);
                            y += 20;
                        }
                    }

                    origin = board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Leaderboard") / 2;
                    board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, "Leaderboard", new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2, Rectangle.Y + y + 40), Color.White, 0f, origin, Vector2.One, SpriteEffects.None, 0f);

                    y += 40;
                    if (board.OrderedScores != null)
                    {
                        foreach (Score score in board.OrderedScores)
                        {
                            string userId = score.UserId != null ? score.UserId : "";
                            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, userId, new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 - 100, Rectangle.Y + y), Color.White, 0f, Vector2.One, Vector2.One, SpriteEffects.None, 0f);
                            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(board.Screen.ScreenManager.Game.GraphicsManager.SpriteFont, score.Value.ToString(), new Vector2(board.Screen.ScreenManager.Game.WorldViewport.Width / 2 + 100, Rectangle.Y + y), Color.White, 0f, Vector2.One, Vector2.One, SpriteEffects.None, 0f);
                            y += 20;
                        }
                    }
                    break;
            }
        }

        void DrawBackground()
        {
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.End();
            
            DepthStencilState depthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, depthStencilState, null, null, board.Screen.ScreenManager.Game.GraphicsManager.WorldToScreenScaleMatrix);

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(backgroundTexture, Rectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.5f);

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.End();

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, board.Screen.ScreenManager.Game.GraphicsManager.WorldToScreenScaleMatrix);
        }

        void DrawBlocks(GameTime gameTime)
        {
            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.End();

            DepthStencilState depthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, depthStencilState, null, null, board.Screen.ScreenManager.Game.GraphicsManager.WorldToScreenScaleMatrix);

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

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.End();

            board.Screen.ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, board.Screen.ScreenManager.Game.GraphicsManager.WorldToScreenScaleMatrix);
        }
    }
}
