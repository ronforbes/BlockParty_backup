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
        Screen screen;
        public Screen Screen
        {
            get { return screen; }
        }

        List<Particle> particles;

        static Random random = new Random();

        public ParticleEmitter(Screen screen, int particleCount, Rectangle rectangle, Vector2 minVelocity, Vector2 maxVelocity, Vector2 acceleration, Color color, TimeSpan duration)
        {
            this.screen = screen;
            particles = new List<Particle>();

            for (int p = 0; p < particleCount; p++)
            {
                particles.Add(new Particle(this, rectangle, new Vector2((float)random.NextDouble() * (maxVelocity.X - minVelocity.X) + minVelocity.X, (float)random.NextDouble() * (maxVelocity.Y - minVelocity.Y) + minVelocity.Y), acceleration, color, duration));
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Particle p in particles)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Particle p in particles)
            {
                p.Draw(gameTime);
            }
        }
    }
}
