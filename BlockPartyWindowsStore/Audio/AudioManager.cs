using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    public class AudioManager
    {
        public bool Muted;

        Game game;
        Dictionary<string, SoundEffect> soundEffects;

        public AudioManager(Game game)
        {
            this.game = game;

            soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadContent()
        {
            soundEffects.Add("BlockSlide", game.Content.Load<SoundEffect>("BlockSlide"));
            soundEffects.Add("BlockPop", game.Content.Load<SoundEffect>("BlockPop"));
            soundEffects.Add("BlockLand", game.Content.Load<SoundEffect>("BlockLand"));
            soundEffects.Add("Celebration", game.Content.Load<SoundEffect>("Celebration"));
        }

        public void Play(string key, float volume, float pitch, float pan)
        {
            if (!Muted)
            {
                soundEffects[key].Play(volume, pitch, pan);
            }
        }
    }
}
