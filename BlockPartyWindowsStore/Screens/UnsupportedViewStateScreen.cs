using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Screens
{
    class UnsupportedViewStateScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D mouseTexture;

        public UnsupportedViewStateScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("GameplayBackground");
            mouseTexture = ContentManager.Load<Texture2D>("Cursor");
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);

            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.Game.GraphicsManager.ScreenViewport.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            Vector2 stringOrigin = ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString("Go fullscreen to play!") / 2;

            ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(ScreenManager.Game.GraphicsManager.SpriteFont, "Go fullscreen to play!", new Vector2(ScreenManager.Game.GraphicsManager.ScreenViewport.Width / 2, ScreenManager.Game.GraphicsManager.ScreenViewport.Height / 2), Color.White, 0f, stringOrigin, Vector2.One, SpriteEffects.None, 0f);

            base.Draw(gameTime);

            ScreenManager.Game.GraphicsManager.SpriteBatch.End();
        }
    }
}
