using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class SplashScreen : Screen
    {
        TimeSpan timeElapsed;
        readonly TimeSpan duration = TimeSpan.FromSeconds(1);

        Texture2D logoTexture;

        public SplashScreen(ScreenManager screenManager) : base(screenManager) 
        {
            TransitionDuration = TimeSpan.FromSeconds(1);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            logoTexture = ContentManager.Load<Texture2D>("Logo");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (State == Screen.ScreenState.Active)
            {
                timeElapsed += gameTime.ElapsedGameTime;

                if (timeElapsed >= duration)
                {
                    ScreenManager.LoadScreen(new MainMenuScreen(ScreenManager));
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsManager.DrawFullscreenSprite(logoTexture, Color.White);

            base.Draw(gameTime);
        }
    }
}
