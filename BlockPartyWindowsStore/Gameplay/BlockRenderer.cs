using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class BlockRenderer
    {
        Block block;
        int row = 0, column = 0;
        public int Width, Height;

        Rectangle rectangle = new Rectangle();
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        Color color = Color.White;
        public Color Color
        {
            get { return color; }
        }

        Vector2 scale = Vector2.One;
        const int flashFrequency = 100;

        const float dampeningFactor = 0.15f;

        const int margin = 5;

        Texture2D texture;
        Texture2D redTexture;
        Texture2D greenTexture;
        Texture2D blueTexture;
        Texture2D cyanTexture;
        Texture2D magentaTexture;
        Texture2D yellowTexture;

        public BlockRenderer(Block block, int row, int column)
        {
            this.block = block;
            this.row = row;
            this.column = column;

            Width = Height = block.Board.Screen.ScreenManager.World.Width > block.Board.Screen.ScreenManager.World.Height ?
                block.Board.Screen.ScreenManager.World.Height / 10 :
                block.Board.Screen.ScreenManager.World.Width / 10;
        }

        public void LoadContent()
        {
            redTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockRed");
            greenTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockGreen");
            blueTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockBlue");
            cyanTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockCyan");
            magentaTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockMagenta");
            yellowTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockYellow");
        }

        public void Draw(GameTime gameTime)
        {
            Width = Height = block.Board.Screen.ScreenManager.World.Width > block.Board.Screen.ScreenManager.World.Height ?
                block.Board.Screen.ScreenManager.World.Height / 10 :
                block.Board.Screen.ScreenManager.World.Width / 10;

            // Calculate base position
            rectangle.X = block.Board.Renderer.Rectangle.X + column * Width;
            rectangle.Y = block.Board.Renderer.Rectangle.Y + row * Height;
            rectangle.Width = Width;
            rectangle.Height = Height;

            // Adjust position if the board is populating
            if (block.Board.State == Board.BoardState.Populating)
            {
                rectangle.Y += (int)Tween.ElasticEaseOut(Math.Max(block.Board.PopulatingTimeElapsed.TotalMilliseconds - block.PopulatingDelay.TotalMilliseconds, 0), -1 * block.Board.Rows * block.Board.Blocks[0, 0].Renderer.Height, block.Board.Rows * block.Board.Blocks[0, 0].Renderer.Height, block.Board.PopulatingDuration.TotalMilliseconds);
            }

            Color targetColor = color = Color.White;

            // Adjust vertical position based on the board's raising state
            rectangle.Y -= (int)Tween.Linear(block.Board.RaiseTimeElapsed.TotalMilliseconds, 0, Height, block.Board.RaiseDuration.TotalMilliseconds);

            // Set the color based on the block's type
            switch (block.Type)
            {
                //default: color = targetColor = Color.TransparentBlack; break;
                //case 0: color = targetColor = Color.Red; break;
                //case 1: color = targetColor = Color.Green; break;
                //case 2: color = targetColor = Color.Blue; break;
                //case 3: color = targetColor = Color.Cyan; break;
                //case 4: color = targetColor = Color.Magenta; break;
                //case 5: color = targetColor = Color.Yellow; break;
                default: return;
                case 0: texture = redTexture; break;
                case 1: texture = greenTexture; break;
                case 2: texture = blueTexture; break;
                case 3: texture = cyanTexture; break;
                case 4: texture = magentaTexture; break;
                case 5: texture = yellowTexture; break;
            }

            switch (block.State)
            {
                case Block.BlockState.Empty:
                    return;

                case Block.BlockState.Idle:
                    // Adjust color and scale if the block is selected, or dampen otherwise
                    if (block.Selected)
                    {
                        color = new Color(color.ToVector4() + new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
                    }
                    else
                    {
                        scale += (Vector2.One - scale) * dampeningFactor;
                        if ((scale - Vector2.One).Length() < 0.1f)
                        {
                            scale = Vector2.One;
                        }

                        color = new Color(targetColor.ToVector4() - color.ToVector4() * dampeningFactor);
                    }
                    break;

                case Block.BlockState.Sliding:
                    // Adjust position based on progress through the slide
                    int direction = block.SlideDirection == Block.BlockSlideDirection.Left ? -1 : 1;
                    rectangle.X += (int)Tween.Linear(block.SlideTimeElapsed.TotalMilliseconds, 0, direction * Width, block.SlideDuration.TotalMilliseconds);
                    break;

                case Block.BlockState.WaitingToFall: break;

                case Block.BlockState.Falling:
                    // Adjust position based on progress through falling
                    rectangle.Y += (int)Tween.Linear(block.FallTimeElapsed.TotalMilliseconds, 0, Height, block.FallDuration.TotalMilliseconds);
                    break;

                case Block.BlockState.Matched: break;

                case Block.BlockState.Flashing:
                    // Alternate between white and the original color
                    if (gameTime.TotalGameTime.TotalMilliseconds % flashFrequency > flashFrequency / 2)
                    {
                        color = Color.White;
                    }
                    break;

                case Block.BlockState.WaitingToPop:
                    break;

                case Block.BlockState.Popping:
                    // Adjust scale and color based on progress through the pop
                    scale = new Vector2((float)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, 1.0, 1.0, block.PopDuration.TotalMilliseconds));
                    color = new Color((int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, color.R, -1 * color.R, block.PopDuration.TotalMilliseconds),
                        (int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, color.G, -1 * color.G, block.PopDuration.TotalMilliseconds),
                        (int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, color.B, -1 * color.B, block.PopDuration.TotalMilliseconds),
                        (int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, color.A, -1 * color.A, block.PopDuration.TotalMilliseconds));
                    break;

                case Block.BlockState.WaitingToEmpty:
                    // Reset scale and color
                    scale = Vector2.One;
                    color = Color.Black;
                    return;

                case Block.BlockState.Preview:
                    // Darken the color
                    color = new Color(color.ToVector3() - new Vector3(0.5f, 0.5f, 0.5f));
                    break;
            }

            Rectangle scaledRectangle = new Rectangle((int)(rectangle.X - rectangle.Width * (scale.X - 1) / 2 + margin), (int)(rectangle.Y - rectangle.Height * (scale.Y - 1) / 2 + margin), (int)(rectangle.Width * scale.X - margin), (int)(rectangle.Height * scale.Y - margin));
            block.Board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(texture, scaledRectangle, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            //block.Board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(block.Board.Screen.ScreenManager.GraphicsManager.BlockTexture, scaledRectangle, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            //block.Board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(block.Board.Screen.ScreenManager.GraphicsManager.BlankTexture, new Vector2(rectangle.X, rectangle.Y), null, color, 0.0f, Vector2.Zero, new Vector2(rectangle.Width * scale.X, rectangle.Height * scale.Y), SpriteEffects.None, 0.0f);
        }
    }
}
