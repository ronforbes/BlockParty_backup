using BlockPartyWindowsStore.Gameplay;
using BlockPartyWindowsStore.ScreenManagement;
using BlockPartyWindowsStore.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace BlockPartyWindowsStore
{
    class MainMenuScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D titleTexture;
        Texture2D mouseTexture;
        Button playButton;
        Button storeButton;
        Button audioButton;
        BlockRain blockRain;

        public MainMenuScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);

            // setup the play button
            int playButtonWidth = screenManager.Game.WorldViewport.Width / 5;
            int playButtonHeight = screenManager.Game.WorldViewport.Height / 10;
            playButton = new Button(this, "Play!", Color.White, new Rectangle(screenManager.Game.WorldViewport.Width / 2 - playButtonWidth / 2, screenManager.Game.WorldViewport.Height / 2 - playButtonHeight / 2, playButtonWidth, playButtonHeight), Color.Green);
            playButton.Selected += playButton_Selected;

            // setup the store button
            int storeButtonWidth = screenManager.Game.WorldViewport.Width / 5;
            int storeButtonHeight = screenManager.Game.WorldViewport.Height / 10;
            storeButton = new Button(this, "Store", Color.White, new Rectangle(screenManager.Game.WorldViewport.Width / 2 - storeButtonWidth / 2, screenManager.Game.WorldViewport.Height / 2 - storeButtonHeight / 2 + (int)(playButtonHeight * 1.1f), storeButtonWidth, storeButtonHeight), Color.Green);
            storeButton.Selected += storeButton_Selected;

            // Setup the audio mute button
            string audioButtonText = ScreenManager.Game.AudioManager.Muted ? "Audio: Off" : "Audio: On";
            Vector2 audioButtonSize = new Vector2(screenManager.Game.GraphicsManager.SpriteFont.MeasureString(audioButtonText).X * 1.1f);
            audioButton = new Button(this, audioButtonText, Color.White, new Rectangle((int)(screenManager.Game.WorldViewport.Width * 0.025f), (int)(screenManager.Game.WorldViewport.Height * 0.975f - audioButtonSize.Y), (int)audioButtonSize.X, (int)audioButtonSize.Y), Color.Red);
            audioButton.Selected += audioButton_Selected;

            blockRain = new BlockRain(this);
        }

        void playButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new GameplayScreen(ScreenManager));
        }

        void storeButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new StoreScreen(ScreenManager));
        }

        void audioButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.Game.AudioManager.Muted = !ScreenManager.Game.AudioManager.Muted;

            audioButton.Text = ScreenManager.Game.AudioManager.Muted ? "Audio: Off" : "Audio: On";
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("BlockPartyMainMenuBackground");
            titleTexture = ContentManager.Load<Texture2D>("BlockPartyMainMenuTitle");
            playButton.LoadContent();
            storeButton.LoadContent();
            audioButton.LoadContent();
            mouseTexture = ContentManager.Load<Texture2D>("Cursor");

            blockRain.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            playButton.Update(gameTime);
            storeButton.Update(gameTime);
            audioButton.Update(gameTime);
            blockRain.Update(gameTime);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.HandleInput(gameTime);

            playButton.HandleInput(gameTime);
            storeButton.HandleInput(gameTime);
            audioButton.HandleInput(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.Game.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, ScreenManager.Game.GraphicsManager.WorldToScreenScaleMatrix);
            
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.Game.WorldViewport.Bounds, Color.White);
            blockRain.Draw(gameTime);
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(titleTexture, ScreenManager.Game.WorldViewport.Bounds, Color.White);
            playButton.Draw(gameTime);
            storeButton.Draw(gameTime);
            audioButton.Draw(gameTime);
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.Game.InputManager.WorldPosition.X, ScreenManager.Game.InputManager.WorldPosition.Y, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.Game.GraphicsManager.SpriteBatch.End();
        }
    }
}
