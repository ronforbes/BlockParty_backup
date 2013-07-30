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
        public const int Width = 30;

        /// <summary>
        /// Height of a block, relative to aspect ratio
        /// </summary>
        public const int Height = 30;

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

        /// <summary>
        /// Scale at which the block is rendered
        /// </summary>
        float scale = 1.0f;

        /// <summary>
        /// Color with which the block is rendered
        /// </summary>
        Color color;

        /// <summary>
        /// Reference to the parent board
        /// </summary>
        Board board;

        /// <summary>
        /// Reference to the sound manager for playing block sound effects
        /// </summary>
        SoundManager soundManager;

        // Construct the block
        public Block(Board board, SoundManager soundManager)
        {
            this.board = board;
            this.soundManager = soundManager;
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

        public void Land()
        {
            State = BlockState.Idle;
            scale = 1.25f;
            color = new Color(color.R + 0.5f, color.G + 0.5f, color.B + 0.5f, color.A + 0.5f);
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
            board.Score += Board.ScoreBlockPop;
            soundManager.Play("BlockPop");
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
                        FallTarget.State = BlockState.Falling;
                        FallTarget.Type = Type;
                        FallTarget.ChainEligible = ChainEligible;
                        FallTarget.JustFell = true;
                        
                        State = BlockState.Empty;
                        Type = -1;
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

        public void Draw(GameTime gameTime, GraphicsManager graphicsManager, int row, int column, Vector2 boardPosition, bool nextRow)
        {
            Vector2 position = new Vector2();

            switch (Type)
            {
                default: color = new Color(0.0f, 0.0f, 0.0f, 0.0f); break;
                case 0: color = new Color(1.0f, 0.0f, 0.0f, 1.0f); break;
                case 1: color = new Color(0.0f, 1.0f, 0.0f, 1.0f); break;
                case 2: color = new Color(0.0f, 0.0f, 1.0f, 1.0f); break;
                case 3: color = new Color(0.0f, 1.0f, 1.0f, 1.0f); break;
                case 4: color = new Color(1.0f, 1.0f, 0.0f, 1.0f); break;
                case 5: color = new Color(1.0f, 0.0f, 1.0f, 1.0f); break;
            }

            switch (State)
            {
                case BlockState.Empty: break;
                
                case BlockState.Idle:
                    break;
                
                case BlockState.Sliding:
                    int direction = slideDirection == BlockSlideDirection.Left ? -1 : 1;
                    position.X = (float)Tween.Linear(slideTimeElapsed, 0, direction * Width, slideDuration);
                    break;
                
                case BlockState.WaitingToFall: break;
                
                case BlockState.Falling:
                    position.Y += (float)Tween.Linear(fallTimeElapsed, 0, Height, fallDuration);
                    break;
                
                case BlockState.Matched: break;
                
                case BlockState.Flashing:
                    if (gameTime.TotalGameTime.TotalMilliseconds % flashFrequency > flashFrequency / 2)
                    {
                        color = Color.White;
                    }
                    break;
                
                case BlockState.WaitingToPop: break;
                
                case BlockState.Popping:
                    scale = (float)Tween.Linear(popTimeElapsed, 1.0, 1.0, popDuration);
                    color.R = (byte)Tween.Linear(popTimeElapsed, color.R, -1 * color.R, popDuration);
                    color.G = (byte)Tween.Linear(popTimeElapsed, color.G, -1 * color.G, popDuration);
                    color.B = (byte)Tween.Linear(popTimeElapsed, color.B, -1 * color.B, popDuration);
                    color.A = (byte)Tween.Linear(popTimeElapsed, color.A, -1 * color.A, popDuration);
                    break;
                
                case BlockState.WaitingToEmpty:
                    scale = 1.0f;
                    color = Color.Black; 
                    break;
            }

            if(Selected && State == BlockState.Idle)
            {
                scale = 1.25f;
                color = new Color(color.ToVector3() + new Vector3(0.5f, 0.5f, 0.5f));
            }

            if (nextRow)
            {
                color = new Color(color.ToVector3() - new Vector3(0.5f, 0.5f, 0.5f));
            }

            // Apply dampening
            scale += (1.0f - scale) * 0.15f;
            if (Math.Abs(scale - 1.0f) < 0.01)
                scale = 1.0f;

            // dampen color
            switch (Type)
            {
                case 0:
                    color.R += (byte)((Color.Red.R - color.R) * 0.15f);
                    color.G += (byte)((Color.Red.G - color.G) * 0.15f);
                    color.B += (byte)((Color.Red.B - color.B) * 0.15f);
                    break;
                case 1:
                    color.R += (byte)((Color.Green.R - color.R) * 0.15f);
                    color.G += (byte)((Color.Green.G - color.G) * 0.15f);
                    color.B += (byte)((Color.Green.B - color.B) * 0.15f);
                    break;
                case 2:
                    color.R += (byte)((Color.Blue.R - color.R) * 0.15f);
                    color.G += (byte)((Color.Blue.G - color.G) * 0.15f);
                    color.B += (byte)((Color.Blue.B - color.B) * 0.15f);
                    break;
                case 3:
                    color.R += (byte)((Color.Cyan.R - color.R) * 0.15f);
                    color.G += (byte)((Color.Cyan.G - color.G) * 0.15f);
                    color.B += (byte)((Color.Cyan.B - color.B) * 0.15f);
                    break;
                case 4:
                    color.R += (byte)((Color.Magenta.R - color.R) * 0.15f);
                    color.G += (byte)((Color.Magenta.G - color.G) * 0.15f);
                    color.B += (byte)((Color.Magenta.B - color.B) * 0.15f);
                    break;
                case 5:
                    color.R += (byte)((Color.Yellow.R - color.R) * 0.15f);
                    color.G += (byte)((Color.Yellow.G - color.G) * 0.15f);
                    color.B += (byte)((Color.Yellow.B - color.B) * 0.15f);
                    break;
            }
            
            if (State != BlockState.Empty)
            {
                graphicsManager.DrawRectangle("Blank", new Rectangle((int)boardPosition.X + column * Width + Width / 2 + (int)position.X, row * Height + Height / 2 + (int)position.Y + (int)boardPosition.Y, (int)(Width * 0.95), (int)(Height * 0.95)), color, 0.0f, scale);
            }
        }
    }
}
