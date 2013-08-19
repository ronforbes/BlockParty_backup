using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class Block
    {
        public Board Board;

        public enum BlockState
        {
            Empty,
            Populating,
            Idle,
            Sliding,
            WaitingToFall,
            Falling,
            Matched,
            Flashing,
            WaitingToPop,
            Popping,
            WaitingToEmpty,
            Preview
        }
        public BlockState State = BlockState.Empty;

        public int Type = -1;
        public const int TypeCount = 6;

        public bool ChainEligible;      // Determines whether the block is capable of participating in a chain
        
        static Random random = new Random();

        public enum BlockSlideDirection
        {
            Left,
            Right
        }
        public BlockSlideDirection SlideDirection;
        BlockState slideTargetState;
        int slideTargetType;
        Vector2 slideTargetRendererScale;
        BlockRenderer.BlockAnimationState slideTargetRendererAnimationState;
        public TimeSpan SlideTimeElapsed;
        readonly public TimeSpan SlideDuration = TimeSpan.FromMilliseconds(50);

        public Block FallTarget;        // Next block down that will receive this block's state once falling completes
        TimeSpan fallDelayTimeElapsed;
        readonly TimeSpan fallDelayDuration = TimeSpan.FromSeconds(0.25);
        public TimeSpan FallTimeElapsed;
        readonly public TimeSpan FallDuration = TimeSpan.FromMilliseconds(50);
        public bool JustFell;           // Determines whether the block just fell and needs to be checked for its next state

        TimeSpan flashTimeElapsed;
        readonly TimeSpan flashDuration = TimeSpan.FromSeconds(1);

        TimeSpan popDelayTimeElapsed;
        readonly TimeSpan popDelayInterval = TimeSpan.FromSeconds(0.25);
        TimeSpan popDelayDuration;

        public TimeSpan PopTimeElapsed;
        readonly public TimeSpan PopDuration = TimeSpan.FromSeconds(0.25);

        public bool JustEmptied;        // Determines whether the block just popped and became empty, indicating that blocks above it are eligible to participate in a chain
        TimeSpan emptyDelayTimeElapsed;
        readonly TimeSpan emptyDelayInterval = TimeSpan.FromSeconds(0.25);
        TimeSpan emptyDelayDuration;

        public BlockRenderer Renderer;

        public TimeSpan PopulatingDelay = TimeSpan.Zero;
        readonly public TimeSpan PopulatingMaxDelay = TimeSpan.FromSeconds(0.25);

        // Construct the block
        public Block(Board board, Screen screen, int row, int column)
        {
            Board = board;
            Renderer = new BlockRenderer(this, row, column);
            PopulatingDelay = TimeSpan.FromMilliseconds(random.Next(0, (int)PopulatingMaxDelay.TotalMilliseconds));
        }

        public void LoadContent()
        {
            Renderer.LoadContent();
        }

        public void Create()
        {
            State = BlockState.Idle;
            Type = random.Next(Block.TypeCount);
        }

        // Set the block up to slide with the state it will need once the slide finishes
        public void SetupSlide(BlockSlideDirection direction, Block targetBlock)
        {
            SlideDirection = direction;
            slideTargetState = targetBlock.State;
            slideTargetType = targetBlock.Type;
            slideTargetRendererScale = targetBlock.Renderer.Scale;
            slideTargetRendererAnimationState = targetBlock.Renderer.AnimationState;
        }

        // Start sliding
        public void Slide()
        {
            State = BlockState.Sliding;
            SlideTimeElapsed = TimeSpan.Zero;
        }

        public void WaitToFall()
        {
            State = BlockState.WaitingToFall;
            fallDelayTimeElapsed = TimeSpan.Zero;
        }

        public void Fall()
        {
            State = BlockState.Falling;
            FallTimeElapsed = TimeSpan.Zero;
        }

        public void Land()
        {
            State = BlockState.Idle;
        }

        public void Match()
        {
            State = BlockState.Matched;
        }

        // Start flashing
        public void Flash(int matchingBlockCount, int delayCounter)
        {
            State = BlockState.Flashing;
            flashTimeElapsed = TimeSpan.Zero;
            popDelayDuration = TimeSpan.FromMilliseconds((matchingBlockCount - delayCounter) * popDelayInterval.TotalMilliseconds);
            emptyDelayDuration = TimeSpan.FromMilliseconds(delayCounter * emptyDelayInterval.TotalMilliseconds);
        }

        public void WaitToPop()
        {
            State = BlockState.WaitingToPop;
            popDelayTimeElapsed = TimeSpan.Zero;
        }

        public void Pop()
        {
            State = BlockState.Popping;
            PopTimeElapsed = TimeSpan.Zero;
            Board.Score += Board.ScoreBlockPop;
            Board.ParticleEmitters.Add(new ParticleEmitter(Board.Screen, 50, new Rectangle(Renderer.Rectangle.X + Board.Blocks[0, 0].Renderer.Width / 2, Renderer.Rectangle.Y + Board.Blocks[0, 0].Renderer.Height / 2, 20, 20), new Vector2(-0.1f, -0.1f), new Vector2(0.1f, 0.1f), Vector2.Zero, Renderer.Color, TimeSpan.FromSeconds(5)));
            Board.Screen.ScreenManager.AudioManager.Play("BlockPop", 1.0f, 0.0f, 0.0f);
        }

        public void WaitToEmpty()
        {
            State = BlockState.WaitingToEmpty;
            emptyDelayTimeElapsed = TimeSpan.Zero;
        }

        public void Empty()
        {
            State = BlockState.Empty;
            Type = -1;
            JustEmptied = true;
        }

        public void Raise(Block targetBlock)
        {
            State = targetBlock.State;
            Type = targetBlock.Type;
        }

        // Update the block depending on its state
        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                case BlockState.Empty: break;

                case BlockState.Idle: break;

                case BlockState.Sliding:
                    SlideTimeElapsed = SlideTimeElapsed.Add(gameTime.ElapsedGameTime);

                    if (SlideTimeElapsed >= SlideDuration)
                    {
                        State = slideTargetState;
                        Type = slideTargetType;
                        Renderer.Scale = slideTargetRendererScale;
                        Renderer.AnimationState = slideTargetRendererAnimationState;
                    }
                    break;

                case BlockState.WaitingToFall:
                    fallDelayTimeElapsed = fallDelayTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (fallDelayTimeElapsed >= fallDelayDuration)
                    {
                        Fall();
                    }
                    break;

                case BlockState.Falling:
                    FallTimeElapsed = FallTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (FallTimeElapsed >= FallDuration)
                    {
                        FallTarget.State = BlockState.Falling;
                        FallTarget.Type = Type;
                        FallTarget.ChainEligible = ChainEligible;
                        FallTarget.JustFell = true;
                        FallTarget.Renderer.Color = Renderer.Color;

                        State = BlockState.Empty;
                        Type = -1;
                        ChainEligible = false;
                    }
                    break;

                case BlockState.Matched: break;

                case BlockState.Flashing:
                    flashTimeElapsed = flashTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (flashTimeElapsed >= flashDuration)
                    {
                        WaitToPop();
                    }
                    break;

                case BlockState.WaitingToPop:
                    popDelayTimeElapsed = popDelayTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (popDelayTimeElapsed >= popDelayDuration)
                    {
                        Pop();
                    }
                    break;

                case BlockState.Popping:
                    PopTimeElapsed = PopTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (PopTimeElapsed >= PopDuration)
                    {
                        WaitToEmpty();
                    }
                    break;

                case BlockState.WaitingToEmpty:
                    emptyDelayTimeElapsed = emptyDelayTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (emptyDelayTimeElapsed >= emptyDelayDuration)
                    {
                        Empty();
                    }
                    break;
            }

            Renderer.Update(gameTime);
        }

        public void Press()
        {
            Renderer.Press();
        }

        public void Release()
        {
            Renderer.Release();
        }

        public void Draw(GameTime gameTime)
        {
            Renderer.Draw(gameTime);
        }
    }
}
