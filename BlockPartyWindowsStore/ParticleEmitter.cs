using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class ParticleEmitter
    {
        List<Particle> particles;

        static Random random = new Random();

        public ParticleEmitter(int particleCount, Vector2 position, Vector2 minVelocity, Vector2 maxVelocity, Vector2 acceleration, Color color, float scale, double duration)
        {
            particles = new List<Particle>();

            for (int p = 0; p < particleCount; p++)
            {
                particles.Add(new Particle(position, new Vector2((float)random.NextDouble() * (maxVelocity.X - minVelocity.X) + minVelocity.X, (float)random.NextDouble() * (maxVelocity.Y - minVelocity.Y) + minVelocity.Y), acceleration, color, scale, duration));
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Particle p in particles)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, GraphicsManager graphicsManager)
        {
            foreach (Particle p in particles)
            {
                p.Draw(gameTime, graphicsManager);
            }
        }
    }
}
