using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class ParticleEmitter
    {
        public Screen Screen;
        public bool Active = true;

        List<Particle> particles;
        List<Particle> particlesToRemove = new List<Particle>();

        static Random random = new Random();

        public ParticleEmitter(Screen screen, int particleCount, Rectangle rectangle, Vector2 minVelocity, Vector2 maxVelocity, Vector2 acceleration, Color minColor, Color maxColor, TimeSpan duration)
        {
            Screen = screen;
            particles = new List<Particle>();

            for (int p = 0; p < particleCount; p++)
            {
                //Vector2 velocity = new Vector2((float)random.NextDouble() * (maxVelocity.X - minVelocity.X) + minVelocity.X, (float)random.NextDouble() * (maxVelocity.Y - minVelocity.Y) + minVelocity.Y);
                float degrees = random.Next(0, 359);
                float speed = (float)random.Next((int)maxVelocity.X);
                Vector2 velocity = new Vector2((float)Math.Cos(MathHelper.ToRadians(degrees)) * speed, (float)Math.Sin(MathHelper.ToRadians(degrees)) * speed);
                float brightness = (float)random.NextDouble();
                Color color = new Color(random.Next(minColor.R, maxColor.R), random.Next(minColor.G, maxColor.G), random.Next(minColor.B, maxColor.B), random.Next(minColor.A, maxColor.A));
                particles.Add(new Particle(this, rectangle, velocity, acceleration, color, duration));
            }
        }

        public void Update(GameTime gameTime)
        {
            if (particles.Count > 0)
            {
                particlesToRemove.Clear();

                foreach (Particle p in particles)
                {
                    if (p.Active)
                    {
                        p.Update(gameTime);
                    }
                    else
                    {
                        particlesToRemove.Add(p);
                    }
                }

                foreach (Particle particle in particlesToRemove)
                {
                    particles.Remove(particle);
                }
            }
            else
            {
                Active = false;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (Active)
            {
                Screen.ScreenManager.GraphicsManager.SpriteBatch.End();
                Screen.ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Screen.ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);

                foreach (Particle p in particles)
                {
                    p.Draw(gameTime);
                }

                Screen.ScreenManager.GraphicsManager.SpriteBatch.End();
                Screen.ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Screen.ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);
            }
        }
    }
}
