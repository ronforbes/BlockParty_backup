using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Gameplay
{
    class BlockRain
    {
        public Screen Screen;
        List<BlockParticle> blockParticles = new List<BlockParticle>();
        TimeSpan spawnDelayTimeElapsed = TimeSpan.Zero;
        readonly TimeSpan spawnDelayDuration = TimeSpan.FromMilliseconds(1);
        public List<Texture2D> Textures = new List<Texture2D>();

        public BlockRain(Screen screen)
        {
            Screen = screen;
        }

        public void LoadContent()
        {
            Textures.Add(Screen.ContentManager.Load<Texture2D>("BlockRedV2"));
            Textures.Add(Screen.ContentManager.Load<Texture2D>("BlockGreenV2"));
            Textures.Add(Screen.ContentManager.Load<Texture2D>("BlockBlueV2"));
            Textures.Add(Screen.ContentManager.Load<Texture2D>("BlockCyanV2"));
            Textures.Add(Screen.ContentManager.Load<Texture2D>("BlockMagentaV2"));
            Textures.Add(Screen.ContentManager.Load<Texture2D>("BlockYellowV2"));
        }

        public void Update(GameTime gameTime)
        {
            spawnDelayTimeElapsed += gameTime.ElapsedGameTime;

            if (spawnDelayTimeElapsed >= spawnDelayDuration)
            {
                blockParticles.Add(new BlockParticle(this));

                spawnDelayTimeElapsed = TimeSpan.Zero;
            }

            foreach (BlockParticle blockParticle in blockParticles)
            {
                blockParticle.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (BlockParticle blockParticle in blockParticles)
            {
                blockParticle.Draw(gameTime);
            }
        }
    }
}
