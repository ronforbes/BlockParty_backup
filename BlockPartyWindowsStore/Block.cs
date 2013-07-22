using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    /// <summary>
    /// Enumeration of the possible states a block may be in
    /// </summary>
    enum BlockState
    {
        Empty,
        Idle,
        Sliding,
        WaitingToFall,
        Falling,
        Matched,
        Flashing,
        WaitingToPop,
        Popping,
        WaitingToEmpty
    }

    /// <summary>
    /// Enumeration of the directions in which a block can slide
    /// </summary>
    enum BlockSlideDirection
    {
        Left,
        Right
    }

    /// <summary>
    /// A block on the game board
    /// </summary>
    class Block
    {
        /// <summary>
        /// Number of different types of blocks
        /// </summary>
        public const int TypeCount = 6;

        /// <summary>
        /// Width of a block, relative to aspect ratio
        /// </summary>
        public const int Width = 90;

        /// <summary>
        /// Height of a block, relative to aspect ratio
        /// </summary>
        public const int Height = 90;

        /// <summary>
        /// Current state of the block
        /// </summary>
        public BlockState State;

        /// <summary>
        /// Type of block used for matching
        /// </summary>
        public int Type;

        /// <summary>
        /// Determines whether the block is currently selected
        /// </summary>
        public bool Selected;

        /// <summary>
        /// Determines whether the block is eligible to participate in a chain
        /// </summary>
        public bool ChainEligible;

        /// <summary>
        /// Next block down that will receive this block's state once falling completes
        /// </summary>
        public Block FallTarget;

        /// <summary>
        /// Determines whether the block just popped and became empty, indicating that blocks
        /// above it are eligible to participate in a chain
        /// </summary>
        public bool JustEmptied;

        /// <summary>
        /// Determines whether the block just fell and needs to be checked for its next state
        /// </summary>
        public bool JustFell;

        /// <summary>
        /// Random number generator used to determine block types
        /// </summary>
        static Random random = new Random();

        /// <summary>
        /// Direction in which the block is sliding
        /// </summary>
        BlockSlideDirection slideDirection;

        /// <summary>
        /// Ending state after a slide
        /// </summary>
        BlockState slideTargetState;

        /// <summary>
        /// Ending type after a slide
        /// </summary>
        int slideTargetType;

        /// <summary>
        /// Ending selection state after a slide
        /// </summary>
        bool slideTargetSelected;

        /// <summary>
        /// Amount of time the block has been sliding
        /// </summary>
        double slideTimeElapsed;

        /// <summary>
        /// Duration of block sliding (in milliseconds)
        /// </summary>
        const double slideDuration = 50;

        /// <summary>
        /// Amount of time the block has waited before starting to fall (in milliseconds)
        /// </summary>
        double fallDelayTimeElapsed;

        /// <summary>
        /// Duration of time that blocks wait before falling (in milliseconds)
        /// </summary>
        const double fallDelayDuration = 250;

        /// <summary>
        /// The amount of time the block has been falling (in milliseconds)
        /// </summary>
        double fallTimeElapsed;

        /// <summary>
        /// Duration of time that blocks fall (in milliseconds)
        /// </summary>
        const double fallDuration = 50;

        /// <summary>
        /// The amount of time the block has been flashing (in milliseconds)
        /// </summary>
        double flashTimeElapsed;

        /// <summary>
        /// Duration of time that blocks flash before starting to pop
        /// </summary>
        const double flashDuration = 1000;

        /// <summary>
        /// The frequency at which flashing blocks should blink
        /// </summary>
        const int flashFrequency = 100;

        /// <summary>
        /// The amount of time the block has been waiting to pop (in milliseconds)
        /// </summary>
        double popDelayTimeElapsed;

        /// <summary>
        /// Interval of time delay between blocks starting to pop (in milliseconds)
        /// </summary>
        const double popDelayInterval = 250;

        /// <summary>
        /// Duration of time to wait before starting to pop (in milliseconds)
        /// </summary>
        double popDelayDuration;

        /// <summary>
        /// The amount of time the block has been popping (in milliseconds)
        /// </summary>
        double popTimeElapsed;

        /// <summary>
        /// Duration of time that blocks pop (in milliseconds)
        /// </summary>
        const double popDuration = 250;

        /// <summary>
        /// The amount of time the block has been waiting to empty
        /// </summary>
        double emptyDelayTimeElapsed;

        /// <summary>
        /// Interval of time delay between blocks starting to empty (in milliseconds)
        /// </summary>
        const double emptyDelayInterval = 250;

        /// <summary>
        /// Duration of time to wait before emptying (in milliseconds)
        /// </summary>
        double emptyDelayDuration;

        // Construct the block
        public Block(GraphicsDevice graphicsDevice)
        {
            State = BlockState.Empty;
            Type = -1;
        }

        public void Create()
        {
            State = BlockState.Idle;
            Type = random.Next(Block.TypeCount);
        }

        // Set the block up to slide with the state it will need once the slide finishes
        public void SetupSlide(BlockSlideDirection direction, Block targetBlock)
        {
            slideDirection = direction;
            slideTargetState = targetBlock.State;
            slideTargetType = targetBlock.Type;
            slideTargetSelected = targetBlock.Selected;
        }

        // Start sliding
        public void Slide()
        {
            State = BlockState.Sliding;
            slideTimeElapsed = 0;
        }

        public void WaitToFall()
        {
            State = BlockState.WaitingToFall;
            fallDelayTimeElapsed = 0;
            Selected = false;
        }

        public void Fall()
        {
            State = BlockState.Falling;
            fallTimeElapsed = 0;
        }

        public void Match()
        {
            State = BlockState.Matched;
            Selected = false;
        }

        // Start flashing
        public void Flash(int matchingBlockCount, int delayCounter)
        {
            State = BlockState.Flashing;
            flashTimeElapsed = 0;
            popDelayDuration = (matchingBlockCount - delayCounter) * popDelayInterval;
            emptyDelayDuration = delayCounter * emptyDelayInterval;
        }

        public void WaitToPop()
        {
            State = BlockState.WaitingToPop;
            popDelayTimeElapsed = 0;
        }

        public void Pop()
        {
            State = BlockState.Popping;
            popTimeElapsed = 0;
        }

        public void WaitToEmpty()
        {
            State = BlockState.WaitingToEmpty;
            emptyDelayTimeElapsed = 0;
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
                    slideTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (slideTimeElapsed >= slideDuration)
                    {
                        State = slideTargetState;
                        Type = slideTargetType;
                        Selected = slideTargetSelected;
                    }
                    break;
                
                case BlockState.WaitingToFall:
                    fallDelayTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (fallDelayTimeElapsed >= fallDelayDuration)
                    {
                        Fall();
                    }
                    break;
                
                case BlockState.Falling:
                    fallTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (fallTimeElapsed >= fallDuration)
                    {
                        FallTarget.Type = Type;
                        FallTarget.ChainEligible = ChainEligible;
                        FallTarget.JustFell = true;
                        
                        State = BlockState.Empty;
                        ChainEligible = false;
                    }
                    break;
                
                case BlockState.Matched: break;
                
                case BlockState.Flashing:
                    flashTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (flashTimeElapsed >= flashDuration)
                    {
                        WaitToPop();
                    }
                    break;
                
                case BlockState.WaitingToPop:
                    popDelayTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (popDelayTimeElapsed >= popDelayDuration)
                    {
                        Pop();
                    }
                    break;
                
                case BlockState.Popping:
                    popTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (popTimeElapsed >= popDuration)
                    {
                        WaitToEmpty();
                    }
                    break;
                
                case BlockState.WaitingToEmpty:
                    emptyDelayTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (emptyDelayTimeElapsed >= emptyDelayDuration)
                    {
                        Empty();
                    }
                    break;
            }
        }

        public void Draw(GameTime gameTime, GraphicsManager graphicsManager, int row, int column)
        {
            Color color = Color.Black;

            switch (Type)
            {
                case 0: color = Color.Red; break;
                case 1: color = Color.Green; break;
                case 2: color = Color.Blue; break;
                case 3: color = Color.Cyan; break;
                case 4: color = Color.Magenta; break;
                case 5: color = Color.Yellow; break;
            }

            switch (State)
            {
                case BlockState.Empty: color = Color.Black; break;
                case BlockState.Idle: break;
                case BlockState.WaitingToFall: break;
                case BlockState.Falling: break;
                case BlockState.Matched: break;
                case BlockState.Flashing:
                    if (gameTime.TotalGameTime.TotalMilliseconds % flashFrequency > flashFrequency / 2)
                    {
                        color = Color.White;
                    }
                    break;
                case BlockState.WaitingToPop: break;
                case BlockState.Popping: break;
                case BlockState.WaitingToEmpty: color = Color.Black; break;
            }

            if(Selected)
            {
                color = new Color(color.ToVector3() + new Vector3(0.5f, 0.5f, 0.5f));
            }

            graphicsManager.DrawRectangle(new Rectangle(column * Width, row * Height, Width, Height), color);
        }
    }
}
