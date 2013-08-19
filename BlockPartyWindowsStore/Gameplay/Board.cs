using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore
{
    class Board
    {
        public Screen Screen;

        public enum BoardState
        {
            Populating,
            CountingDown,
            Playing,
            GameOver
        }
        public BoardState State = BoardState.Populating;

        public readonly int Rows = 10, Columns = 6;
        const int initialEmptyRows = 5;

        public Block[,] Blocks;
        public Block[] NextBlocks;

        Random random = new Random();

        int chainCount = 0;

        public TimeSpan RaiseTimeElapsed = TimeSpan.Zero;
        public readonly TimeSpan RaiseDuration = TimeSpan.FromSeconds(10);
        int raiseRateNormal = 1;
        public readonly int RaiseRateAccelerated = 33;
        public int RaiseRate = 1;

        public TimeSpan GameOverDelayTimeElapsed;
        public readonly TimeSpan GameOverDelayDuration = TimeSpan.FromSeconds(10);

        public TimeSpan PopulatingTimeElapsed = TimeSpan.Zero;
        public readonly TimeSpan PopulatingDuration = TimeSpan.FromSeconds(1);

        public TimeSpan GoTimeElapsed = TimeSpan.Zero;
        public readonly TimeSpan GoDuration = TimeSpan.FromSeconds(1);

        public TimeSpan CountdownTimeElapsed = TimeSpan.Zero;
        public readonly TimeSpan CountdownDuration = TimeSpan.FromSeconds(3);

        public TimeSpan GameOverTimeElapsed = TimeSpan.Zero;
        public readonly TimeSpan GameOverDuration = TimeSpan.FromSeconds(1);

        public int Score = 0;
        public const int ScoreBlockPop = 10;
        const int ScoreBlockComboInterval = 50;
        const int ScoreBlockChainInterval = 100;

        public int Level = 1;

        public List<Celebration> Celebrations = new List<Celebration>();
        public List<ParticleEmitter> ParticleEmitters = new List<ParticleEmitter>();

        public BoardRenderer Renderer;
        BoardController controller;

        /// <summary>
        /// Construct the board
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public Board(Screen screen)
        {
            Screen = screen;   

            // Create the board
            Blocks = new Block[Rows, Columns];
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Blocks[row, column] = new Block(this, Screen, row, column);

                    // Only fill the first bottom Rows
                    if (row < initialEmptyRows)
                    {
                        Blocks[row, column].Empty();
                    }
                    else
                    {
                        Blocks[row, column].Create();
                    }
                }
            }

            // Create the next row of Blocks
            NextBlocks = new Block[Columns];
            for (int column = 0; column < Columns; column++)
            {
                NextBlocks[column] = new Block(this, Screen, Rows, column);
                NextBlocks[column].Create();
                NextBlocks[column].State = Block.BlockState.Preview;
            }

            Renderer = new BoardRenderer(this);
            controller = new BoardController(this);
        }

        public void LoadContent()
        {
            Renderer.LoadContent();

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Blocks[row, column].LoadContent();
                }
            }
        }

        /// <summary>
        /// Update the board
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="mouseManager"></param>
        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                case BoardState.Populating:
                    PopulatingTimeElapsed += gameTime.ElapsedGameTime;

                    if (PopulatingTimeElapsed >= PopulatingDuration)
                    {
                        State = BoardState.CountingDown;
                        CountdownTimeElapsed = TimeSpan.Zero;
                    }

                    UpdateBlocks(gameTime);
                    break;

                case BoardState.CountingDown:
                    CountdownTimeElapsed += gameTime.ElapsedGameTime;

                    if (CountdownTimeElapsed >= CountdownDuration)
                    {
                        State = BoardState.Playing;
                    }
                    break;

                case BoardState.Playing:
                    DetectMatchingBlocks();

                    Raise(gameTime);

                    DetectGameOver(gameTime);

                    UpdateBlocks(gameTime);

                    DetectChain();

                    ApplyGravity(gameTime);

                    UpdateGo(gameTime);

                    // Update celebrations
                    foreach (Celebration celebration in Celebrations)
                    {
                        celebration.Update(gameTime);
                    }

                    // Update particle emitters
                    foreach (ParticleEmitter pe in ParticleEmitters)
                    {
                        pe.Update(gameTime);
                    }

                    UpdateLevel();
                    break;

                case BoardState.GameOver:
                    GameOverTimeElapsed += gameTime.ElapsedGameTime;

                    if (GameOverTimeElapsed >= GameOverDuration)
                    {
                        GameOverTimeElapsed = GameOverDuration;
                    }

                    UpdateBlocks(gameTime);

                    break;
            }
        }

        /// <summary>
        /// Detect matching Blocks and set their State appropriately
        /// </summary>
        void DetectMatchingBlocks()
        {
            int matchingBlockCount = 0;
            bool incrementChain = false;
            int comboCelebrationRow = 0;
            int comboCelebrationColumn = 0;
            int chainCelebrationRow = 0;
            int chainCelebrationColumn = 0;

            // First look for horizontal matches
            for (int row = 0; row < Rows; row++)
            {
                int horizontalMatchingBlocks = 0;

                for (int column = 0; column < Columns; column++)
                {
                    // Skip non-idle Blocks
                    if (Blocks[row, column].State != Block.BlockState.Idle)
                    {
                        continue;
                    }

                    // Search to the right for matching Blocks
                    if (column + 1 < Columns &&
                        Blocks[row, column + 1].State == Block.BlockState.Idle &&
                        Blocks[row, column].Type == Blocks[row, column + 1].Type)
                    {
                        horizontalMatchingBlocks++;
                    }
                    else
                    {
                        // Mark sequences of 2 or more matching Blocks
                        if (horizontalMatchingBlocks >= 2)
                        {
                            for (int matchingColumn = horizontalMatchingBlocks; matchingColumn >= 0; matchingColumn--)
                            {
                                Blocks[row, column - matchingColumn].Match();
                                matchingBlockCount++;
                                comboCelebrationRow = row;
                                comboCelebrationColumn = column - matchingColumn;
                                if (Blocks[row, column - matchingColumn].ChainEligible)
                                {
                                    incrementChain = true;
                                    chainCelebrationRow = row;
                                    chainCelebrationColumn = column - matchingColumn;
                                }
                            }
                        }

                        // Reset the matching block count
                        horizontalMatchingBlocks = 0;
                    }
                }
            }

            // Now look for vertical matches
            for (int column = 0; column < Columns; column++)
            {
                int verticalMatchingBlocks = 0;

                for (int row = 0; row < Rows; row++)
                {
                    // Skip non-idle Blocks
                    if (Blocks[row, column].State != Block.BlockState.Idle)
                    {
                        continue;
                    }

                    // Search down for matching Blocks
                    if (row + 1 < Rows &&
                        Blocks[row + 1, column].State == Block.BlockState.Idle &&
                        Blocks[row, column].Type == Blocks[row + 1, column].Type)
                    {
                        verticalMatchingBlocks++;
                    }
                    else
                    {
                        // Mark sequences of 2 or more matching Blocks
                        if (verticalMatchingBlocks >= 2)
                        {
                            for (int matchingRow = verticalMatchingBlocks; matchingRow >= 0; matchingRow--)
                            {
                                Blocks[row - matchingRow, column].State = Block.BlockState.Matched;
                                matchingBlockCount++;
                                comboCelebrationRow = row - matchingRow;
                                comboCelebrationColumn = column;
                                if (Blocks[row - matchingRow, column].ChainEligible)
                                {
                                    incrementChain = true;
                                    chainCelebrationRow = row - matchingRow;
                                    chainCelebrationColumn = column;
                                }
                            }
                        }

                        // Reset the matching block count
                        verticalMatchingBlocks = 0;
                    }
                }
            }

            // Handle combos
            if (matchingBlockCount > 3)
            {
                Score += matchingBlockCount * ScoreBlockComboInterval;
                Celebrations.Add(new Celebration(this, matchingBlockCount.ToString(), comboCelebrationRow, comboCelebrationColumn));
                //ParticleEmitters.Add(new ParticleEmitter(Screen, 50, new Vector2(Screen.ScreenManager.WorldViewport.Width / 2 - Columns * BlockRenderer.Width / 2, (int)(-1 * BlockRenderer.Height * raiseTimeElapsed.TotalMilliseconds / raiseDuration.TotalMilliseconds)) + new Vector2(comboCelebrationColumn * BlockRenderer.Width, comboCelebrationRow * BlockRenderer.Width), new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f), Vector2.Zero, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()), new Vector2(10, 10), TimeSpan.FromMilliseconds(1000)));
                float pitch = 0.1f * (matchingBlockCount - 3);

                // Only play the combo sound if a chain sound isn't going to play (avoid cacophany!)
                if (!incrementChain)
                {
                    Screen.ScreenManager.AudioManager.Play("Celebration", 1.0f, pitch, 0.0f);
                }
            }

            // Handle chain continuation
            if (incrementChain)
            {
                chainCount++;
                Score += chainCount * ScoreBlockChainInterval;
                Celebrations.Add(new Celebration(this, chainCount.ToString() + "x", chainCelebrationRow, chainCelebrationColumn));
                //ParticleEmitters.Add(new ParticleEmitter(Screen, 50, new Vector2(Screen.ScreenManager.WorldViewport.Width / 2 - Columns * BlockRenderer.Width / 2, (int)(-1 * BlockRenderer.Height * raiseTimeElapsed.TotalMilliseconds / raiseDuration.TotalMilliseconds)) + new Vector2(chainCelebrationColumn * BlockRenderer.Width, chainCelebrationRow * BlockRenderer.Width), new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f), Vector2.Zero, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()), new Vector2(10, 10), TimeSpan.FromMilliseconds(1000)));
                float pitch = 0.1f * chainCount;
                Screen.ScreenManager.AudioManager.Play("Celebration", 1.0f, pitch, 0.0f);
            }

            // Setup matching Blocks to pop
            int delayCounter = matchingBlockCount;
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (Blocks[row, column].State == Block.BlockState.Matched)
                    {
                        Blocks[row, column].Flash(matchingBlockCount, delayCounter);
                        delayCounter--;
                    }
                }
            }
        }

        /// <summary>
        /// Detects Blocks that are eligible to participage in chains based on Blocks below
        /// that have finished popping
        /// </summary>
        void DetectChain()
        {
            bool stopChain = true;

            // Detect Blocks that are eligible to participate in chains
            for (int column = 0; column < Columns; column++)
            {
                for (int row = Rows - 1; row >= 0; row--)
                {
                    if (Blocks[row, column].JustEmptied)
                    {
                        for (int chainEligibleRow = row - 1; chainEligibleRow >= 0; chainEligibleRow--)
                        {
                            if (Blocks[chainEligibleRow, column].State == Block.BlockState.Idle)
                            {
                                Blocks[chainEligibleRow, column].ChainEligible = true;
                                stopChain = false;
                            }
                        }
                    }

                    Blocks[row, column].JustEmptied = false;
                }
            }

            // Stop the current chain if all of the Blocks are idle or empty
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (Blocks[row, column].State != Block.BlockState.Idle && Blocks[row, column].State != Block.BlockState.Empty && Blocks[row, column].State != Block.BlockState.Sliding)
                    {
                        stopChain = false;
                    }
                }
            }

            if (stopChain)
            {
                for (int row = 0; row < Rows; row++)
                {
                    for (int column = 0; column < Columns; column++)
                    {
                        Blocks[row, column].ChainEligible = false;
                    }
                }

                chainCount = 1;
            }
        }

        /// <summary>
        /// Applies gravity to Blocks, making them fall when there are empty ones below them
        /// </summary>
        /// <param name="gameTime"></param>
        void ApplyGravity(GameTime gameTime)
        {
            bool playLandSound = false;
            for (int column = 0; column < Columns; column++)
            {
                bool emptyBlockDetected = false;

                for (int row = Rows - 1; row >= 0; row--)
                {
                    if (Blocks[row, column].State == Block.BlockState.Empty)
                    {
                        emptyBlockDetected = true;
                    }

                    if (Blocks[row, column].State == Block.BlockState.Idle && emptyBlockDetected)
                    {
                        Blocks[row, column].FallTarget = Blocks[row + 1, column];
                        Blocks[row, column].WaitToFall();
                    }

                    if (Blocks[row, column].JustFell)
                    {
                        //If there are no more empty or falling Blocks below, stop falling
                        if (row + 1 < Rows && (Blocks[row + 1, column].State == Block.BlockState.Empty || Blocks[row + 1, column].State == Block.BlockState.Falling))
                        {
                            Blocks[row, column].Fall();
                            Blocks[row, column].FallTarget = Blocks[row + 1, column];
                        }
                        else
                        {
                            Blocks[row, column].Land();
                            playLandSound = true;
                            ParticleEmitters.Add(new ParticleEmitter(Screen, 3, new Rectangle(Blocks[row, column].Renderer.Rectangle.X, Blocks[row, column].Renderer.Rectangle.Y + Blocks[0, 0].Renderer.Height, 5, 5), new Vector2(-0.05f, -0.05f), new Vector2(0.0f, 0.0f), Vector2.Zero, Color.White, TimeSpan.FromMilliseconds(1000)));
                            ParticleEmitters.Add(new ParticleEmitter(Screen, 3, new Rectangle(Blocks[row, column].Renderer.Rectangle.X + Blocks[0, 0].Renderer.Width, Blocks[row, column].Renderer.Rectangle.Y + Blocks[0, 0].Renderer.Height, 5, 5), new Vector2(0.0f, -0.05f), new Vector2(0.05f, 0.0f), Vector2.Zero, Color.White, TimeSpan.FromMilliseconds(1000)));
                        }

                        Blocks[row, column].JustFell = false;
                    }
                }
            }

            if (playLandSound)
            {
                Screen.ScreenManager.AudioManager.Play("BlockLand", 1.0f, 0.0f, 0.0f);
            }
        }

        void Raise(GameTime gameTime)
        {
            int rate = RaiseRate;
            // Determine whether the board should raise based on whether there are any non-empty/idle Blocks
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (Blocks[row, column].State != Block.BlockState.Empty &&
                        Blocks[row, column].State != Block.BlockState.Idle &&
                        Blocks[row, column].State != Block.BlockState.Sliding)
                    {
                        if (rate == raiseRateNormal)
                        {
                            rate = 0;
                        }
                    }

                    // Stop raising if any cells in the top row are occupied
                    if (row == 0)
                    {
                        if (Blocks[row, column].State != Block.BlockState.Empty)
                        {
                            if (rate == raiseRateNormal)
                            {
                                rate = 0;
                            }
                        }
                    }
                }
            }

            RaiseTimeElapsed = RaiseTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * rate));

            if (RaiseTimeElapsed >= RaiseDuration)
            {
                RaiseTimeElapsed = TimeSpan.Zero;
                RaiseRate = raiseRateNormal;

                for (int row = 0; row < Rows - 1; row++)
                {
                    for (int column = 0; column < Columns; column++)
                    {
                        Blocks[row, column].Raise(Blocks[row + 1, column]);
                    }
                }

                for (int column = 0; column < Columns; column++)
                {
                    Blocks[Rows - 1, column].Raise(NextBlocks[column]);
                    Blocks[Rows - 1, column].State = Block.BlockState.Idle;

                    NextBlocks[column].Create();
                    NextBlocks[column].State = Block.BlockState.Preview;
                }
            }
        }

        /// <summary>
        /// Detects game over conditions
        /// </summary>
        void DetectGameOver(GameTime gameTime)
        {
            // Determine if we're about to lose based on idle Blocks in the top row
            bool pendingGameOver = false;
            for (int column = 0; column < Columns; column++)
            {
                if (Blocks[0, column].State == Block.BlockState.Idle)
                {
                    pendingGameOver = true;
                }
            }

            // Determine whether there are any non-empty / non-idle Blocks
            bool activeBlocks = false;
            for (int row = 0; row > Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (Blocks[row, column].State != Block.BlockState.Idle &&
                        Blocks[row, column].State != Block.BlockState.Empty)
                    {
                        activeBlocks = true;
                    }
                }
            }

            if (pendingGameOver && !activeBlocks)
            {
                GameOverDelayTimeElapsed += gameTime.ElapsedGameTime;

                if (GameOverDelayTimeElapsed >= GameOverDelayDuration)
                {
                    State = BoardState.GameOver;
                }
            }
            else
            {
                GameOverDelayTimeElapsed = TimeSpan.Zero;
            }
        }

        void UpdateGo(GameTime gameTime)
        {
            GoTimeElapsed += gameTime.ElapsedGameTime;

            if (GoTimeElapsed >= GoDuration)
            {
                GoTimeElapsed = GoDuration;
            }
        }

        void UpdateBlocks(GameTime gameTime)
        {
            // Updating Blocks from the bottom up to account for falling logic
            for (int row = Rows - 1; row >= 0; row--)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Blocks[row, column].Update(gameTime);
                }
            }

            for (int column = 0; column < Columns; column++)
            {
                NextBlocks[column].Update(gameTime);
            }
        }

        void UpdateLevel()
        {
            Level = (int)MathHelper.Clamp(Score / 1000, 1, 99);
            raiseRateNormal = Math.Max(Level / 3, 1);
        }

        public void HandleInput(GameTime gameTime)
        {
            controller.HandleInput(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Renderer.Draw(gameTime);
        }
    }
}
