using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class Particle
    {
        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;
        Color color;
        float scale;
        double timeElapsed;
        double duration;

        public Particle(Vector2 position, Vector2 velocity, Vector2 acceleration, Color color, float scale, double duration)
        {
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.color = color;
            this.scale = scale;
            this.duration = (float)duration;
        }

        public void Update(GameTime gameTime)
        {
            if(timeElapsed < duration)
            {
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                
                velocity += acceleration * (float)timeElapsed;
                velocity *= 0.85f;
                
                position += velocity * (float)timeElapsed;
                
                scale *= 0.85f;
                
                color.R = (byte)(color.R * 0.85f);
                color.G = (byte)(color.G * 0.85f);
                color.B = (byte)(color.B * 0.85f);
                color.A = (byte)(color.A * 0.85f);
            }
        }

        public void Draw(GameTime gameTime, GraphicsManager graphicsManager)
        {
            if (timeElapsed < duration)
            {
                graphicsManager.DrawRectangle("Blank", new Rectangle((int)position.X, (int)position.Y, 1, 1), color, 0.0f, scale);
            }
        }
    }
}
