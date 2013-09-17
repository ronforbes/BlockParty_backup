using BlockPartyWindowsStore.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace BlockPartyWindowsStore.Screens
{
    class StoreScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D titleTexture;
        Texture2D mouseTexture;
        Button bigUpsButton;
        Button backButton;
        LicenseInformation licenseInformation;

        public StoreScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);

            // setup the big ups button
            string bigUpsButtonText = "Send Big Ups to Omega Entertainment (FREE!)";
            int bigUpsButtonWidth = (int)(screenManager.Game.GraphicsManager.SpriteFont.MeasureString(bigUpsButtonText).X * 1.1f);
            int bigUpsButtonHeight = screenManager.Game.WorldViewport.Height / 10;
            bigUpsButton = new Button(this, bigUpsButtonText, Color.White, new Rectangle(screenManager.Game.WorldViewport.Width / 2 - bigUpsButtonWidth / 2, screenManager.Game.WorldViewport.Height / 2 - bigUpsButtonHeight / 2, bigUpsButtonWidth, bigUpsButtonHeight), Color.Red);
            bigUpsButton.Selected += bigUpsButton_Selected;

            // setup the back button
            string backButtonText = "Back";
            Vector2 backButtonSize = new Vector2(screenManager.Game.GraphicsManager.SpriteFont.MeasureString(backButtonText).X * 1.1f);
            backButton = new Button(this, backButtonText, Color.White, new Rectangle((int)(screenManager.Game.WorldViewport.Width * 0.025f), (int)(screenManager.Game.WorldViewport.Height * 0.025f), (int)backButtonSize.X, (int)backButtonSize.Y), Color.Orange);
            backButton.Selected += backButton_Selected;
            licenseInformation = CurrentAppSimulator.LicenseInformation;
        }        

        void bigUpsButton_Selected(object sender, EventArgs e)
        {
            Purchase();
        }

        async Task Purchase()
        {
            await CurrentAppSimulator.RequestProductPurchaseAsync("bigUps", false);
        }

        void backButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new MainMenuScreen(ScreenManager));
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("StoreBackground");
            titleTexture = ContentManager.Load<Texture2D>("StoreTitle");
            bigUpsButton.LoadContent();
            backButton.LoadContent();
            mouseTexture = ContentManager.Load<Texture2D>("Cursor");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bigUpsButton.Update(gameTime);
            backButton.Update(gameTime);
        }

        public override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);

            bigUpsButton.HandleInput(gameTime);
            backButton.HandleInput(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, ScreenManager.Game.GraphicsManager.WorldToScreenScaleMatrix);

            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.Game.WorldViewport.Bounds, Color.White);
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(titleTexture, ScreenManager.Game.WorldViewport.Bounds, Color.White);
            bigUpsButton.Draw(gameTime);
            backButton.Draw(gameTime);
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.Game.InputManager.WorldPosition.X, ScreenManager.Game.InputManager.WorldPosition.Y, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.Game.GraphicsManager.SpriteBatch.End();
        }
    }
}
