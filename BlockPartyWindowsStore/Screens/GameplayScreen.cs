﻿using BlockPartyWindowsStore.Gameplay;
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
        Board board;
        HUD hud;
        Button backButton;

        public GameplayScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            TransitionDuration = TimeSpan.FromSeconds(1);

            board = new Board(this);
            hud = new HUD(this, board);
            backButton = new Button(this, "Back", Color.White, new Rectangle(5, 5, 75, 75), Color.Red);
            backButton.Selected += backButton_Selected;
        }

        void backButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.LoadScreen(new MainMenuScreen(ScreenManager));
        }

        public override void LoadContent()
        {
            base.LoadContent();

            backgroundTexture = ContentManager.Load<Texture2D>("GameplayBackground");

            board.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (State == ScreenState.Active)
            {
                board.Update(gameTime);
            }

            backButton.Update(gameTime);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.HandleInput(gameTime);

            board.HandleInput(gameTime);
            backButton.HandleInput(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsManager.DrawFullscreenSprite(backgroundTexture, Color.White);

            board.Draw(gameTime);
            hud.Draw(gameTime);
            backButton.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
