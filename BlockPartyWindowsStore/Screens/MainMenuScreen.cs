using BlockPartyWindowsStore.Gameplay;
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
        Texture2D titleTexture;
        Texture2D mouseTexture;
        Button playButton;
        Button audioButton;
        BlockRain blockRain;

        public MainMenuScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);

            // setup the play button
            int playButtonWidth = screenManager.World.Width / 5;
            int playButtonHeight = screenManager.World.Height / 10;
            playButton = new Button(this, "Play!", Color.White, new Rectangle(screenManager.World.Width / 2 - playButtonWidth / 2, screenManager.World.Height / 2 - playButtonHeight / 2, playButtonWidth, playButtonHeight), Color.Green);
            playButton.Selected += playButton_Selected;

            // Setup the audio mute button
            string audioButtonText = ScreenManager.AudioManager.Muted ? "Audio: Off" : "Audio: On";
            Vector2 audioButtonSize = new Vector2(screenManager.GraphicsManager.SpriteFont.MeasureString(audioButtonText).X * 1.1f);
            audioButton = new Button(this, audioButtonText, Color.White, new Rectangle((int)(screenManager.World.Width * 0.025f), (int)(screenManager.World.Height * 0.975f - audioButtonSize.Y), (int)audioButtonSize.X, (int)audioButtonSize.Y), Color.Red);
            audioButton.Selected += audioButton_Selected;

            blockRain = new BlockRain(this);
        }

        void playButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new GameplayScreen(ScreenManager));
        }

        void audioButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.AudioManager.Muted = !ScreenManager.AudioManager.Muted;

            audioButton.Text = ScreenManager.AudioManager.Muted ? "Audio: Off" : "Audio: On";
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("BlockPartyMainMenuBackground");
            titleTexture = ContentManager.Load<Texture2D>("BlockPartyMainMenuTitle");
            mouseTexture = ContentManager.Load<Texture2D>("Cursor");

            blockRain.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            playButton.Update(gameTime);
            audioButton.Update(gameTime);
            blockRain.Update(gameTime);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.HandleInput(gameTime);

            playButton.HandleInput(gameTime);
            audioButton.HandleInput(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);
            
            ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.World.Bounds, Color.White);
            blockRain.Draw(gameTime);
            ScreenManager.GraphicsManager.SpriteBatch.Draw(titleTexture, ScreenManager.World.Bounds, Color.White);
            playButton.Draw(gameTime);
            audioButton.Draw(gameTime);
            ScreenManager.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.InputManager.WorldX, ScreenManager.InputManager.WorldY, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.GraphicsManager.SpriteBatch.End();
        }
    }
}
