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

        List<Particle> particles;

        static Random random = new Random();

        public ParticleEmitter(Screen screen, int particleCount, Rectangle rectangle, Vector2 minVelocity, Vector2 maxVelocity, Vector2 acceleration, Color color, TimeSpan duration)
        {
            Screen = screen;
            particles = new List<Particle>();

            for (int p = 0; p < particleCount; p++)
            {
                float brightness = (float)random.NextDouble();
                particles.Add(new Particle(this, rectangle, new Vector2((float)random.NextDouble() * (maxVelocity.X - minVelocity.X) + minVelocity.X, (float)random.NextDouble() * (maxVelocity.Y - minVelocity.Y) + minVelocity.Y), acceleration, new Color(color.ToVector4() * brightness), duration));
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
