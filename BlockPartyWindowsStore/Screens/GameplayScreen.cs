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
    class GameplayScreen : Screen
    {
        Texture2D backgroundTexture;
        Texture2D mouseTexture;
        Board board;
        HUD hud;
        Button menuButton;

        public GameplayScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);

            board = new Board(this);
            hud = new HUD(this, board);
            menuButton = new Button(this, "Menu", Color.White, new Rectangle(45, 45, 75, 75), Color.Red);
            menuButton.Selected += menuButton_Selected;
        }

        void menuButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new MainMenuScreen(ScreenManager));
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("GameplayBackground");

            mouseTexture = ContentManager.Load<Texture2D>("Cursor");

            board.LoadContent();

            hud.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (State == ScreenState.Active)
            {
                board.Update(gameTime);
            }

            menuButton.Update(gameTime);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.HandleInput(gameTime);

            board.HandleInput(gameTime);
            menuButton.HandleInput(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ScreenManager.GraphicsManager.WorldToScreenScaleMatrix);

            ScreenManager.GraphicsManager.SpriteBatch.Draw(backgroundTexture, ScreenManager.World.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            board.Draw(gameTime);
            hud.Draw(gameTime);
            menuButton.Draw(gameTime);

            ScreenManager.GraphicsManager.SpriteBatch.Draw(mouseTexture, new Rectangle(ScreenManager.InputManager.WorldX, ScreenManager.InputManager.WorldY, 25, 50), Color.White);

            base.Draw(gameTime);

            ScreenManager.GraphicsManager.SpriteBatch.End();
        }
    }
}
