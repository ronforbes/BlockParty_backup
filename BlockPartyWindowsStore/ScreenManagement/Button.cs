using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.ScreenManagement
{
    class Button
    {
        public string Text;
        public event EventHandler Selected;

        enum ButtonAnimationState
        {
            Idle,
            Pressing,
            Releasing
        }

        Screen screen;        
        Color textColor = Color.White;
        Rectangle rectangle;
        Color rectangleColor = Color.Gray;
        Vector2 scale = Vector2.One;
        bool pressed;
        ButtonAnimationState animationState = ButtonAnimationState.Idle;
        TimeSpan scaleTimeElapsed = TimeSpan.Zero;
        readonly TimeSpan scaleDuration = TimeSpan.FromSeconds(0.5);

        public Button(Screen screen, string text, Color textColor, Rectangle rectangle, Color rectangleColor)
        {
            this.screen = screen;
            this.Text = text;
            this.textColor = textColor;
            this.rectangle = rectangle;
            this.rectangleColor = rectangleColor;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Scale the button based on animation state
            switch (animationState)
            {
                case ButtonAnimationState.Idle: break;
                case ButtonAnimationState.Pressing:
                    scaleTimeElapsed += gameTime.ElapsedGameTime;
                    scale.X = (float)Tween.BounceEaseOut(scaleTimeElapsed.TotalMilliseconds, 1, -0.25, scaleDuration.TotalMilliseconds);
                    scale.Y = (float)Tween.BounceEaseOut(scaleTimeElapsed.TotalMilliseconds, 1, -0.25, scaleDuration.TotalMilliseconds);
                    if (scaleTimeElapsed >= scaleDuration)
                    {
                        animationState = ButtonAnimationState.Idle;
                    }
                    break;

                case ButtonAnimationState.Releasing:
                    scaleTimeElapsed += gameTime.ElapsedGameTime;
                    scale.X = (float)Tween.BounceEaseOut(scaleTimeElapsed.TotalMilliseconds, 0.75, 0.25, scaleDuration.TotalMilliseconds);
                    scale.Y = (float)Tween.BounceEaseOut(scaleTimeElapsed.TotalMilliseconds, 0.75, 0.25, scaleDuration.TotalMilliseconds);
                    if (scaleTimeElapsed >= scaleDuration)
                    {
                        animationState = ButtonAnimationState.Idle;
                    }
                    break;
            }

            // Make the button pulse
            scale.X += (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 4) / 500;
            scale.Y += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) / 500;
        }

        public virtual void HandleInput(GameTime gameTime)
        {
            if(screen.ScreenManager.InputManager.LeftButtonPressed && rectangle.Contains(screen.ScreenManager.InputManager.WorldX, screen.ScreenManager.InputManager.WorldY))
            {
                pressed = true;
                animationState = ButtonAnimationState.Pressing;
                scaleTimeElapsed = TimeSpan.Zero;
            }

            if (pressed && !rectangle.Contains(screen.ScreenManager.InputManager.WorldX, screen.ScreenManager.InputManager.WorldY))
            {
                pressed = false;
                animationState = ButtonAnimationState.Releasing;
                scaleTimeElapsed = TimeSpan.Zero;
            }

            if (pressed && screen.ScreenManager.InputManager.LeftButtonReleased && rectangle.Contains(screen.ScreenManager.InputManager.WorldX, screen.ScreenManager.InputManager.WorldY))
            {
                OnSelect();
                pressed = false;
                animationState = ButtonAnimationState.Releasing;
                scaleTimeElapsed = TimeSpan.Zero;
            }
        }

        protected internal virtual void OnSelect()
        {
            if (Selected != null)
            {
                Selected(this, null);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            Color drawTextColor = textColor;
            Color drawRectangleColor = rectangleColor;

            if (pressed)
            {
                drawTextColor = new Color(drawTextColor.ToVector3() / 2);
                drawRectangleColor = new Color(drawRectangleColor.ToVector3() / 2);
            }

            screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(screen.ScreenManager.GraphicsManager.BlankTexture, new Rectangle((int)(rectangle.X - rectangle.Width * (scale.X - 1) / 2), (int)(rectangle.Y - rectangle.Height * (scale.Y - 1) / 2), (int)(rectangle.Width * scale.X), (int)(rectangle.Height * scale.Y)), drawRectangleColor);

            Vector2 position = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            Vector2 origin = screen.ScreenManager.GraphicsManager.SpriteFont.MeasureString(Text) / 2;
            screen.ScreenManager.GraphicsManager.SpriteBatch.DrawString(screen.ScreenManager.GraphicsManager.SpriteFont, Text, position, drawTextColor, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
