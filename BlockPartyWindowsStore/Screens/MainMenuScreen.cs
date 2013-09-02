using BlockPartyWindowsStore.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class MainMenuScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D mouseTexture;
        Button playButton;

        public MainMenuScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);

            int playButtonWidth = screenManager.World.Width / 5;
            int playButtonHeight = screenManager.World.Height / 10;

            playButton = new Button(this, "Play!", Color.White, new Rectangle(screenManager.World.Width / 2 - playButtonWidth / 2, screenManager.World.Height / 2 - playButtonHeight / 2, playButtonWidth, playButtonHeight), Color.Green);
            playButton.Selected += playButton_Selected;
        }

        void playButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new GameplayScreen(ScreenManager));
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("BlockPartyMainMenuBackground");
            mouseTexture = ContentManager.Load<Texture2D>("Cursor");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            playButton.Update(gameTime);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.HandleInput(gameTime);

            playButton.HandleInput(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);
            
            ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.World.Bounds, Color.White);
            
            playButton.Draw(gameTime);

            ScreenManager.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.InputManager.WorldX, ScreenManager.InputManager.WorldY, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.GraphicsManager.SpriteBatch.End();
        }
    }
}
