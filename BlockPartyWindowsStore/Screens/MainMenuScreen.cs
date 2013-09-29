using BlockPartyWindowsStore.Gameplay;
using BlockPartyWindowsStore.ScreenManagement;
using BlockPartyWindowsStore.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;

namespace BlockPartyWindowsStore
{
    class MainMenuScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D titleTexture;
        Texture2D mouseTexture;
        Texture2D playerPictureTexture;
        Rectangle playerPictureRectangle;
        string playerName = "";
        Button playButton;
        Button storeButton;
        Button audioButton;
        Button logInButton;
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

            // setup the log in button
            string logInButtonText = !screenManager.Game.IsLoggedIn ? "Log In" : "Log Out";
            Vector2 logInButtonSize = new Vector2(screenManager.Game.GraphicsManager.SpriteFont.MeasureString(logInButtonText).X * 1.1f);
            logInButton = new Button(this, logInButtonText, Color.White, new Rectangle((int)(screenManager.Game.WorldViewport.Width * 0.975f - logInButtonSize.X), (int)(screenManager.Game.WorldViewport.Height * 0.025f), (int)logInButtonSize.X, (int)logInButtonSize.Y), new Color(59, 89, 152));
            logInButton.Selected += logInButton_Selected;

            // Setup the audio mute button
            string audioButtonText = ScreenManager.Game.AudioManager.Muted ? "Audio: Off" : "Audio: On";
            Vector2 audioButtonSize = new Vector2(screenManager.Game.GraphicsManager.SpriteFont.MeasureString(audioButtonText).X * 1.1f);
            audioButton = new Button(this, audioButtonText, Color.White, new Rectangle((int)(screenManager.Game.WorldViewport.Width * 0.025f), (int)(screenManager.Game.WorldViewport.Height * 0.975f - audioButtonSize.Y), (int)audioButtonSize.X, (int)audioButtonSize.Y), Color.Red);
            audioButton.Selected += audioButton_Selected;

            playerPictureRectangle = new Rectangle((int)(screenManager.Game.WorldViewport.Width * 0.975f - 2 * logInButtonSize.X), (int)(screenManager.Game.WorldViewport.Height * 0.025f), (int)logInButtonSize.X, (int)logInButtonSize.Y);

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

        async void logInButton_Selected(object sender, EventArgs e)
        {
            if (!ScreenManager.Game.IsLoggedIn)
            {
                await LogIn();
            }
        }

        async Task LogIn()
        {
            try
            {
                ScreenManager.Game.FacebookSession = await ScreenManager.Game.FacebookSessionClient.LoginAsync();
                ScreenManager.Game.FacebookAccessToken = ScreenManager.Game.FacebookSession.AccessToken;
                ScreenManager.Game.FacebookId = ScreenManager.Game.FacebookSession.FacebookId;

                ScreenManager.Game.FacebookClient = new Facebook.FacebookClient(ScreenManager.Game.FacebookAccessToken);

                dynamic parameters = new ExpandoObject();
                parameters.access_token = ScreenManager.Game.FacebookAccessToken;
                parameters.fields = "first_name";

                dynamic result = await ScreenManager.Game.FacebookClient.GetTaskAsync("me", parameters);

                WebRequest webRequest = HttpWebRequest.Create(new Uri("https://graph.facebook.com/" + ScreenManager.Game.FacebookId + "/picture?type=large&access_token=" + ScreenManager.Game.FacebookAccessToken));
                WebResponse webResponse = webRequest.GetResponseAsync().Result;
                Stream stream = webResponse.GetResponseStream();
                MemoryStream memoryStream = new MemoryStream();
                int count = 0;
                do
                {
                    byte[] buffer = new byte[1024];
                    count = stream.Read(buffer, 0, 1024);
                    memoryStream.Write(buffer, 0, count);
                } while (stream.CanRead && count > 0);
                memoryStream.Seek(0, SeekOrigin.Begin);
                playerPictureTexture = Texture2D.FromStream(ScreenManager.Game.GraphicsDevice, memoryStream);

                playerName = result.first_name;
            }
            catch (Exception e)
            {
                MessageDialog messageDialog = new MessageDialog(e.Message, "Facebook Log In Failed!");
                messageDialog.ShowAsync();
            }
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
            logInButton.LoadContent();
            audioButton.LoadContent();
            mouseTexture = ContentManager.Load<Texture2D>("Cursor");
            playerPictureTexture = new Texture2D(ScreenManager.Game.GraphicsDevice, 1, 1);

            blockRain.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            playButton.Update(gameTime);
            storeButton.Update(gameTime);
            logInButton.Update(gameTime);
            audioButton.Update(gameTime);
            blockRain.Update(gameTime);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.HandleInput(gameTime);

            playButton.HandleInput(gameTime);
            storeButton.HandleInput(gameTime);
            logInButton.HandleInput(gameTime);
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
            logInButton.Draw(gameTime);
            audioButton.Draw(gameTime);
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(playerPictureTexture, playerPictureRectangle, Color.White);
            Vector2 playerNameSize = ScreenManager.Game.GraphicsManager.SpriteFont.MeasureString(playerName);
            ScreenManager.Game.GraphicsManager.SpriteBatch.DrawString(ScreenManager.Game.GraphicsManager.SpriteFont, playerName, new Vector2(playerPictureRectangle.X + playerPictureRectangle.Width / 2 - playerNameSize.X / 2, playerPictureRectangle.Y + playerPictureRectangle.Height), Color.White);
            ScreenManager.Game.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.Game.InputManager.WorldPosition.X, ScreenManager.Game.InputManager.WorldPosition.Y, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.Game.GraphicsManager.SpriteBatch.End();
        }
    }
}
