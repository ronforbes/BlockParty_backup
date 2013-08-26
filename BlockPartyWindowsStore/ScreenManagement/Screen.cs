using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    abstract class Screen
    {
        public ScreenManager ScreenManager;
        public ContentManager ContentManager;

        public enum ScreenState
        {
            TransitioningOn,
            Active,
            TransitioningOff
        }
        public ScreenState State = ScreenState.TransitioningOn;

        public TimeSpan TransitionTimeElapsed = TimeSpan.Zero;
        public TimeSpan TransitionDuration = TimeSpan.Zero;

        public Screen(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }

        public virtual void LoadContent()
        {
            ContentManager = new ContentManager(ScreenManager.Game.Services, "Content");
        }

        public virtual void UnloadContent()
        {
            ContentManager.Unload();
        }

        public void TransitionOff()
        {
            State = ScreenState.TransitioningOff;
            TransitionTimeElapsed = TimeSpan.Zero;
        }

        public virtual void Update(GameTime gameTime) 
        {
            if (State == ScreenState.TransitioningOn ||
                State == ScreenState.TransitioningOff)
            {
                UpdateTransition(gameTime);
            }
        }

        void UpdateTransition(GameTime gameTime)
        {
            TransitionTimeElapsed += gameTime.ElapsedGameTime;

            if (State == ScreenState.TransitioningOn)
            {
                if (TransitionTimeElapsed >= TransitionDuration)
                {
                    State = ScreenState.Active;
                }
            }

            if (State == ScreenState.TransitioningOff)
            {
                if (TransitionTimeElapsed >= TransitionDuration)
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
        }

        public virtual void HandleInput(GameTime gameTime) { }
        
        public virtual void Draw(GameTime gameTime) 
        {
            if (State == ScreenState.TransitioningOn ||
                State == ScreenState.TransitioningOff)
            {
                DrawTransition();
            }
        }

        public void DrawTransition()
        {
            Color color = Color.Black * (float)(State == ScreenState.TransitioningOn ? 
                Tween.Linear(TransitionTimeElapsed.TotalMilliseconds, 1, -1, TransitionDuration.TotalMilliseconds) : 
                Tween.Linear(TransitionTimeElapsed.TotalMilliseconds, 0, 1, TransitionDuration.TotalMilliseconds));
            ScreenManager.GraphicsManager.SpriteBatch.Draw(ScreenManager.GraphicsManager.BlankTexture, ScreenManager.World.Bounds, color);
        }
    }
}
