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
        public enum BlockAnimationState
        {
            Idle,
            Pressing,
            Pressed,
            Releasing
        }

        Block block;
        int row = 0, column = 0;
        public int Width, Height;
        public Rectangle Rectangle = new Rectangle();
        public Color Color = Color.White;
        public Vector2 Scale = Vector2.One;
        float rotation = 0f;
        const int flashFrequency = 100;
        const int margin = 0;

        Texture2D texture;
        Texture2D redTexture;
        Texture2D redMatchedTexture;
        Texture2D greenTexture;
        Texture2D blueTexture;
        Texture2D cyanTexture;
        Texture2D magentaTexture;
        Texture2D yellowTexture;

        public BlockAnimationState AnimationState = BlockAnimationState.Idle;

        TimeSpan pressTimeElapsed = TimeSpan.Zero;
        readonly TimeSpan pressDuration = TimeSpan.FromSeconds(0.25);

        TimeSpan releaseTimeElapsed = TimeSpan.Zero;
        readonly TimeSpan releaseDuration = TimeSpan.FromSeconds(0.25);

        public bool Shaking = false;

        static Random random = new Random();

        public BlockRenderer(Block block, int row, int column)
        {
            this.block = block;
            this.row = row;
            this.column = column;

            Width = Height = block.Board.Screen.ScreenManager.World.Width > block.Board.Screen.ScreenManager.World.Height ?
                block.Board.Screen.ScreenManager.World.Height / 11 :
                block.Board.Screen.ScreenManager.World.Width / 11;

            texture = block.Board.Screen.ScreenManager.GraphicsManager.BlankTexture;
        }

        public void LoadContent()
        {
            redTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockRedV2");
            redMatchedTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockRedV2Matched");
            greenTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockGreenV2");
            blueTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockBlueV2");
            cyanTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockCyanV2");
            magentaTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockMagentaV2");
            yellowTexture = block.Board.Screen.ContentManager.Load<Texture2D>("BlockYellowV2");
        }

        public void Update(GameTime gameTime)
        {
            Width = Height = block.Board.Screen.ScreenManager.World.Width > block.Board.Screen.ScreenManager.World.Height ?
                block.Board.Screen.ScreenManager.World.Height / 11 :
                block.Board.Screen.ScreenManager.World.Width / 11;

            // Calculate base position
            Rectangle.X = block.Board.Renderer.Rectangle.X + column * Width;
            Rectangle.Y = block.Board.Renderer.Rectangle.Y + row * Height;
            Rectangle.Width = Width;
            Rectangle.Height = Height;

            // Adjust position if the board is populating
            if (block.Board.State == Board.BoardState.Populating)
            {
                Rectangle.Y += (int)Tween.ElasticEaseOut(Math.Max(block.Board.PopulatingTimeElapsed.TotalMilliseconds - block.PopulatingDelay.TotalMilliseconds, 0), -1 * block.Board.Rows * block.Board.Blocks[0, 0].Renderer.Height, block.Board.Rows * block.Board.Blocks[0, 0].Renderer.Height, block.Board.PopulatingDuration.TotalMilliseconds);
            }

            if (block.Board.State == Board.BoardState.GameOver)
            {
                Rectangle.Y += (int)Tween.QuadraticEaseIn(Math.Max(block.Board.GameOverTimeElapsed.TotalMilliseconds - block.PopulatingDelay.TotalMilliseconds, 0), 0, 2 * block.Board.Rows * block.Board.Blocks[0, 0].Renderer.Height, block.Board.GameOverDuration.TotalMilliseconds);
            }

            //Color targetColor = Color = Color.White;

            // Adjust vertical position based on the board's raising state
            Rectangle.Y -= (int)Tween.Linear(block.Board.RaiseTimeElapsed.TotalMilliseconds, 0, Height, block.Board.RaiseDuration.TotalMilliseconds);

            // Set the color based on the block's type
            switch (block.Type)
            {
                case 0: texture = redTexture; Color = Color.Red; break;
                case 1: texture = greenTexture; Color = Color.Green; break;
                case 2: texture = blueTexture; Color = Color.MediumBlue; break;
                case 3: texture = cyanTexture; Color = Color.Cyan; break;
                case 4: texture = magentaTexture; Color = Color.Magenta; break;
                case 5: texture = yellowTexture; Color = Color.Yellow; break;
                default: return;
            }

            switch (AnimationState)
            {
                case BlockAnimationState.Idle:
                    Scale.X = 1.0f;
                    Scale.Y = 1.0f;
                    //Color = targetColor;
                    break;
                case BlockAnimationState.Pressing:
                    pressTimeElapsed += gameTime.ElapsedGameTime;
                    
                    Scale.X = (float)Tween.BounceEaseOut(pressTimeElapsed.TotalMilliseconds, 1, -0.15, pressDuration.TotalMilliseconds);
                    Scale.Y = (float)Tween.BounceEaseOut(pressTimeElapsed.TotalMilliseconds, 1, -0.15, pressDuration.TotalMilliseconds);
                    //float brightness = (float)Tween.BounceEaseOut(pressTimeElapsed.TotalMilliseconds, 0, 0.5, pressDuration.TotalMilliseconds);
                    //Color = new Color(targetColor.ToVector4() + new Vector4(brightness, brightness, brightness, brightness));
                    
                    if (pressTimeElapsed >= pressDuration)
                    {
                        AnimationState = BlockAnimationState.Pressed;
                    }
                    break;
                case BlockAnimationState.Pressed:
                    Scale.X = 0.85f;
                    Scale.Y = 0.85f;
                    Color = new Color(Color.ToVector4() + new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
                    break;
                case BlockAnimationState.Releasing:
                    releaseTimeElapsed += gameTime.ElapsedGameTime;
                    
                    Scale.X = (float)Tween.BounceEaseOut(releaseTimeElapsed.TotalMilliseconds, 0.85, 0.15, releaseDuration.TotalMilliseconds);
                    Scale.Y = (float)Tween.BounceEaseOut(releaseTimeElapsed.TotalMilliseconds, 0.85, 0.15, releaseDuration.TotalMilliseconds);
                    //brightness = (float)Tween.BounceEaseOut(pressTimeElapsed.TotalMilliseconds, 0, 0.5, pressDuration.TotalMilliseconds);
                    //Color = new Color(targetColor.ToVector4() + new Vector4(0.5f, 0.5f, 0.5f, 0.5f) - new Vector4(brightness, brightness, brightness, brightness));

                    if (releaseTimeElapsed >= releaseDuration)
                    {
                        AnimationState = BlockAnimationState.Idle;
                    }
                    break;
            }

            switch (block.State)
            {
                case Block.BlockState.Empty:
                    return;

                case Block.BlockState.Idle:
                    // Adjust rotation if the block is shaking
                    Vector2 originalScale = Scale;
                    if (Shaking)
                    {
                        Scale.X += (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10) / 15;
                        Scale.Y += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 10) / 15;
                    }
                    else
                    {
                        Scale = originalScale;
                    }
                    break;

                case Block.BlockState.Sliding:
                    // Adjust position based on progress through the slide
                    int direction = block.SlideDirection == Block.BlockSlideDirection.Left ? -1 : 1;
                    Rectangle.X += (int)Tween.Linear(block.SlideTimeElapsed.TotalMilliseconds, 0, direction * Width, block.SlideDuration.TotalMilliseconds);
                    break;

                case Block.BlockState.WaitingToFall: break;

                case Block.BlockState.Falling:
                    // Adjust position based on progress through falling
                    Rectangle.Y += (int)Tween.Linear(block.FallTimeElapsed.TotalMilliseconds, 0, Height, block.FallDuration.TotalMilliseconds);
                    break;

                case Block.BlockState.Matched: break;

                case Block.BlockState.Flashing:
                    Scale = new Vector2(Math.Min((float)Tween.Linear(block.FlashTimeElapsed.TotalSeconds, 1, 0.25, block.FlashDuration.TotalSeconds / 8), 1.25f));
                    break;

                case Block.BlockState.WaitingToPop:
                    Scale = new Vector2(1.25f);
                    break;

                case Block.BlockState.Popping:
                    // Adjust scale and color based on progress through the pop
                    Scale = new Vector2((float)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, 1.25, -1.25, block.PopDuration.TotalMilliseconds));
                    Color = new Color((int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, Color.R, -1 * Color.R, block.PopDuration.TotalMilliseconds),
                        (int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, Color.G, -1 * Color.G, block.PopDuration.TotalMilliseconds),
                        (int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, Color.B, -1 * Color.B, block.PopDuration.TotalMilliseconds),
                        (int)Tween.Linear(block.PopTimeElapsed.TotalMilliseconds, Color.A, -1 * Color.A, block.PopDuration.TotalMilliseconds));
                    break;

                case Block.BlockState.WaitingToEmpty:
                    // Reset scale and color
                    Scale = Vector2.One;
                    Color = Color.Black;
                    return;

                case Block.BlockState.Preview:
                    // Darken the color
                    Color = new Color(Color.ToVector4() - new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
                    break;
            }
        }

        public void Press()
        {
            AnimationState = BlockAnimationState.Pressing;
            pressTimeElapsed = TimeSpan.Zero;
        }

        public void Release()
        {
            AnimationState = BlockAnimationState.Releasing;
            releaseTimeElapsed = TimeSpan.Zero;
        }

        public void Draw(GameTime gameTime)
        {
            if (block.State == Block.BlockState.Empty || block.State == Block.BlockState.WaitingToEmpty || block.Type == -1)
            {
                return;
            }

            Rectangle scaledRectangle = new Rectangle((int)(Rectangle.X - Rectangle.Width * (Scale.X - 1) / 2 + margin), (int)(Rectangle.Y - Rectangle.Height * (Scale.Y - 1) / 2 + margin), (int)(Rectangle.Width * Scale.X - 2 * margin), (int)(Rectangle.Height * Scale.Y - 2 * margin));

            block.Board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(texture, scaledRectangle, null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 1f);
            //block.Board.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(block.Board.Screen.ScreenManager.GraphicsManager.BlankTexture, scaledRectangle, null, Color, 0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }
    }
}
