using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class AudioManager
    {
        ScreenManager screenManager;

        Dictionary<string, SoundEffect> soundEffects;

        public AudioManager(ScreenManager screenManager)
        {
            this.screenManager = screenManager;

            soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadContent()
        {
            soundEffects.Add("BlockSlide", screenManager.Game.Content.Load<SoundEffect>("BlockSlide"));
            soundEffects.Add("BlockPop", screenManager.Game.Content.Load<SoundEffect>("BlockPop"));
            soundEffects.Add("BlockLand", screenManager.Game.Content.Load<SoundEffect>("BlockLand"));
            soundEffects.Add("Celebration", screenManager.Game.Content.Load<SoundEffect>("Celebration"));
        }

        public void Play(string key, float volume, float pitch, float pan)
        {
            soundEffects[key].Play(volume, pitch, pan);
        }
    }
}
