using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class Particle
    {
        ParticleEmitter emitter;
        Rectangle rectangle = new Rectangle();
        Vector2 velocity = Vector2.Zero;
        Vector2 acceleration = Vector2.Zero;
        Color color = Color.White;
        Vector2 scale = Vector2.One;
        TimeSpan timeElapsed = TimeSpan.Zero;
        TimeSpan duration = TimeSpan.Zero;

        public Particle(ParticleEmitter emitter, Rectangle rectangle, Vector2 velocity, Vector2 acceleration, Color color, TimeSpan duration)
        {
            this.emitter = emitter;
            this.rectangle = rectangle;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.color = color;
            this.duration = duration;
        }

        public void Update(GameTime gameTime)
        {
            if(timeElapsed < duration)
            {
                timeElapsed = timeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));
                
                velocity += acceleration * (float)timeElapsed.TotalMilliseconds;
                velocity *= 0.85f;
                
                rectangle.X += (int)(velocity.X * (float)timeElapsed.TotalMilliseconds);
                rectangle.Y += (int)(velocity.Y * (float)timeElapsed.TotalMilliseconds);
                
                scale *= 0.99f;
                
                color = new Color((byte)(color.R * 0.99), (byte)(color.G * 0.99), (byte)(color.B * 0.99), (byte)(color.A * 0.99));
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (timeElapsed < duration)
            {
                Rectangle scaledRectangle = new Rectangle((int)(rectangle.X - rectangle.Width * (scale.X - 1) / 2), (int)(rectangle.Y - rectangle.Height * (scale.Y - 1) / 2), (int)(rectangle.Width * scale.X), (int)(rectangle.Height * scale.Y));
                emitter.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(emitter.Screen.ScreenManager.GraphicsManager.ParticleTexture, scaledRectangle, null, color, 0.0f, Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0.0f);
                //emitter.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(emitter.Screen.ScreenManager.GraphicsManager.ParticleTexture, new Vector2(rectangle.X, rectangle.Y), null, color, 0.0f, Vector2.Zero, new Vector2(rectangle.Width * scale.X, rectangle.Height * scale.Y), SpriteEffects.None, 0.0f);
            }
        }
    }
}
