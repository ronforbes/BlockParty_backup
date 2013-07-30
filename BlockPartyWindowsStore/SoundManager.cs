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
        }

        public void Play(string key)
        {
            soundEffects[key].Play();
        }
    }
}
