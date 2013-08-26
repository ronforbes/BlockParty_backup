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
                timeElapsed += gameTime.ElapsedGameTime;

                velocity *= 0.9f;
                velocity += acceleration * (float)timeElapsed.TotalSeconds;
                
                rectangle.X += (int)(velocity.X * (float)timeElapsed.TotalSeconds);
                rectangle.Y += (int)(velocity.Y * (float)timeElapsed.TotalSeconds);
                
                scale *= 0.99f;
                
                color = new Color((byte)(color.R * 0.99), (byte)(color.G * 0.99), (byte)(color.B * 0.99), (byte)(color.A * 0.99));
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (timeElapsed < duration)
            {
                Rectangle scaledRectangle = new Rectangle((int)(rectangle.X - rectangle.Width * (scale.X - 1) / 2), (int)(rectangle.Y - rectangle.Height * (scale.Y - 1) / 2), (int)(rectangle.Width * scale.X), (int)(rectangle.Height * scale.Y));
                emitter.Screen.ScreenManager.GraphicsManager.SpriteBatch.Draw(emitter.Screen.ScreenManager.GraphicsManager.ParticleTexture, scaledRectangle, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            }
        }
    }
}
