using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class SoundManager
    {
        Dictionary<string, SoundEffect> soundEffects;

        public SoundManager()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            soundEffects.Add("BlockSlide", contentManager.Load<SoundEffect>("BlockSlide"));
            soundEffects.Add("BlockPop", contentManager.Load<SoundEffect>("BlockPop"));
            soundEffects.Add("BlockLand", contentManager.Load<SoundEffect>("BlockLand"));
            soundEffects.Add("Celebration", contentManager.Load<SoundEffect>("Celebration"));
        }

        public void Play(string key, float volume, float pitch, float pan)
        {
            soundEffects[key].Play(volume, pitch, pan);
        }
    }
}
