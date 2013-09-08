using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Screens
{
    class SnapFilledScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D mouseTexture;

        public SnapFilledScreen(ScreenManager screenManager)
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
            ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);

            ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.Screen.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            Vector2 stringOrigin = ScreenManager.GraphicsManager.SpriteFont.MeasureString("Go fullscreen to play!") / 2;

            ScreenManager.GraphicsManager.SpriteBatch.DrawString(ScreenManager.GraphicsManager.SpriteFont, "Go fullscreen to play!", new Vector2(ScreenManager.Screen.Width / 2, ScreenManager.Screen.Height / 2), Color.White, 0f, stringOrigin, Vector2.One, SpriteEffects.None, 0f);

            //ScreenManager.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.InputManager.ScreenX, ScreenManager.InputManager.ScreenY, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.GraphicsManager.SpriteBatch.End();
        }
    }
}
