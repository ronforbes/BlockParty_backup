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
        enum BoardState
        {
            Populating,
            CountingDown,
            Playing,
            GameOver
        }

        BoardState state;

        /// <summary>
        /// Number of rows on the board
        /// </summary>
        const int rows = 10;

        /// <summary>
        /// Number of columns on the board
        /// </summary>
        const int columns = 6;

        /// <summary>
        /// Specifies how many rows of the board are empty at initial state
        /// </summary>
        const int initialEmptyRows = 5;

        /// <summary>
        /// The matrix of blocks
        /// </summary>
        Block[,] blocks;

        /// <summary>
        /// The next row of blocks to add
        /// </summary>
        Block[] nextRowBlocks;
        
        /// <summary>
        /// Random number generator
        /// </summary>
        Random random;

        /// <summary>
        /// The row of the currently selected block
        /// </summary>
        int selectedRow = -1;

        /// <summary>
        /// The column of the currently selected block
        /// </summary>
        int selectedColumn = -1;

        /// <summary>
        /// Count of the current chain
        /// </summary>
        int chainCount = 0;

        /// <summary>
        /// The amount of time that the board has raised (in milliseconds)
        /// </summary>
        double raiseTimeElapsed;

        /// <summary>
        /// Duration of time required to raise the board (in milliseconds)
        /// </summary>
        const double raiseDuration = 10000;

        /// <summary>
        /// Amount of time that the game over has been delayed (in milliseconds)
        /// </summary>
        double gameOverDelayTimeElapsed;

        /// <summary>
        /// Duration of time that game over is delayed (in milliseconds)
        /// </summary>
        const double gameOverDelayDuration = 10000;

        /// <summary>
        /// Amount of time that the board has been populating (in milliseconds)
        /// </summary>
        double populatingTimeElapsed;

        /// <summary>
        /// Duration of time to populate the board (in milliseconds)
        /// </summary>
        const double populatingDuration = 1000;

        /// <summary>
        /// Amount of time to delay each block's population on the board (in milliseconds)
        /// </summary>
        int[,] populatingDelays;

        /// <summary>
        /// Maximum delay applied to blocks populating the board (in milliseconds)
        /// </summary>
        const int populatingMaxDelay = 250;
        
        /// <summary>
        /// Amount of time that has been counted down before gameplay (in milliseconds)
        /// </summary>
        double countdownTimeElapsed;

        /// <summary>
        /// Duration of time to count down before gameplay (in milliseconds)
        /// </summary>
        const double countdownDuration = 3000;

        /// <summary>
        /// The player's score
        /// </summary>
        public int Score;

        public const int ScoreBlockPop = 10;
        const int scoreBlockComboInterval = 50;
        const int scoreBlockChainInterval = 100;

        double scoreDisplay;

        /// <summary>
        /// Reference to the sound manager, used to play board sound effects
        /// </summary>
        SoundManager soundManager;

        /// <summary>
        /// Timestamp of the most recent mouse press, used to detect double presses
        /// </summary>
        double previousClickTime;

        /// <summary>
        /// X position of the previous mouse click
        /// </summary>
        int previousClickX;

        /// <summary>
        /// Y position of the previous mouse click
        /// </summary>
        int previousClickY;

        /// <summary>
        /// Maximum amount of time in between clicks to constitute a double click (in milliseconds)
        /// </summary>
        const double doubleClickDuration = 1000;

        /// <summary>
        /// Maximum distance between clicks to constitute a double click
        /// </summary>
        const float doubleClickDistance = 5;

        /// <summary>
        /// Determines whether the board should be raised at the accelerated rate
        /// </summary>
        bool raiseRateAccelerated;

        /// <summary>
        /// Rate at which board should raise after a double click
        /// </summary>
        const double raiseRateAcceleration = 15;

        List<Celebration> celebrations;

        List<ParticleEmitter> particleEmitters;

        /// <summary>
        /// Construct the board
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public Board(GraphicsDevice graphicsDevice, SoundManager soundManager)
        {
            // Create the random number generator
            random = new Random();

            // Start the board in the populating state
            state = BoardState.Populating;
            populatingTimeElapsed = 0;
            populatingDelays = new int[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    populatingDelays[row, column] = random.Next(0, populatingMaxDelay);
                }
            }

            // Create the board
            blocks = new Block[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    blocks[row, column] = new Block(this, soundManager);

                    // Only fill the first bottom rows
                    if (row < initialEmptyRows)
                    {
                        blocks[row, column].Empty();
                    }
                    else
                    {
                        blocks[row, column].Create();
                    }
                }
            }
            
            // Create the next row of blocks
            nextRowBlocks = new Block[columns];
            for (int column = 0; column < columns; column++)
            {
                nextRowBlocks[column] = new Block(this, soundManager);
                nextRowBlocks[column].Create();
            }

            // Save the sound manager
            this.soundManager = soundManager;

            // Initialize celebrations
            celebrations = new List<Celebration>();

            particleEmitters = new List<ParticleEmitter>();
        }

        /// <summary>
        /// Process user input
        /// </summary>
        /// <param name="mouseManager"></param>
        public void HandleInput(GameTime gameTime, MouseManager mouseManager)
        {
            // If the mouse has been pressed over a valid block, select the block that it's hovering over
            if (mouseManager.LeftButtonPressed)
            {
                // Determine which block the mouse is hovering over
                int row = (mouseManager.Y + (int)(Block.Height * raiseTimeElapsed / raiseDuration)) / Block.Height;
                int column = (mouseManager.X - (ScreenManager.WorldWidth / 2 - columns * Block.Width / 2)) / Block.Width;

                if (row >= 0 && row < rows && column >= 0 && column < columns)
                {
                    if (blocks[row, column].State == BlockState.Idle)
                    {
                        selectedRow = row;
                        selectedColumn = column;
                        blocks[selectedRow, selectedColumn].Selected = true;
                    }
                }

                // Determine if the mouse was double pressed
                if (gameTime.TotalGameTime.TotalMilliseconds - previousClickTime < doubleClickDuration)
                {
                    if (Vector2.Distance(new Vector2(mouseManager.X, mouseManager.Y), new Vector2(previousClickX, previousClickY)) < doubleClickDistance)
                    {
                        raiseRateAccelerated = true;
                    }
                }

                previousClickTime = gameTime.TotalGameTime.TotalMilliseconds;
                previousClickX = mouseManager.X;
                previousClickY = mouseManager.Y;
            }

            // If a block is selected, swap it based on the mouse's position
            if (selectedRow != -1 && selectedColumn != -1)
            {
                // Swap the blocks if the mouse has moved to a different column
                if (mouseManager.X - (ScreenManager.WorldWidth / 2 - columns * Block.Width / 2) < selectedColumn * Block.Width &&
                    selectedColumn - 1 >= 0 &&
                    blocks[selectedRow, selectedColumn].State == BlockState.Idle &&
                    (blocks[selectedRow, selectedColumn - 1].State == BlockState.Idle ||
                    blocks[selectedRow, selectedColumn - 1].State == BlockState.Empty))
                {
                    // Swap the selected block with the one on its left
                    blocks[selectedRow, selectedColumn].SetupSlide(BlockSlideDirection.Left, blocks[selectedRow, selectedColumn - 1]);
                    blocks[selectedRow, selectedColumn - 1].SetupSlide(BlockSlideDirection.Right, blocks[selectedRow, selectedColumn]);

                    blocks[selectedRow, selectedColumn].Slide();
                    blocks[selectedRow, selectedColumn - 1].Slide();
                    soundManager.Play("BlockSlide", 1.0f, 0.0f, 0.0f);

                    selectedColumn--;
                }

                if (mouseManager.X - (ScreenManager.WorldWidth / 2 - columns * Block.Width / 2) > (selectedColumn + 1) * Block.Width &&
                    selectedColumn + 1 < columns &&
                    blocks[selectedRow, selectedColumn].State == BlockState.Idle &&
                    (blocks[selectedRow, selectedColumn + 1].State == BlockState.Idle ||
                    blocks[selectedRow, selectedColumn + 1].State == BlockState.Empty))
                {
                    // Swap the selected block with the one on its right
                    blocks[selectedRow, selectedColumn].SetupSlide(BlockSlideDirection.Right, blocks[selectedRow, selectedColumn + 1]);
                    blocks[selectedRow, selectedColumn + 1].SetupSlide(BlockSlideDirection.Left, blocks[selectedRow, selectedColumn]);

                    blocks[selectedRow, selectedColumn].Slide();
                    blocks[selectedRow, selectedColumn + 1].Slide();
                    soundManager.Play("BlockSlide", 1.0f, 0.0f, 0.0f);

                    selectedColumn++;
                }
            }

            // Deselect the block if the mouse button is no longer being held
            if (mouseManager.LeftButtonReleased && selectedRow != -1 && selectedColumn != -1)
            {
                blocks[selectedRow, selectedColumn].Selected = false;
                selectedRow = -1;
                selectedColumn = -1;
            }
        }

        /// <summary>
        /// Detect matching blocks and set their state appropriately
        /// </summary>
        private void DetectMatchingBlocks()
        {
            int matchingBlockCount = 0;
            bool incrementChain = false;
            int comboCelebrationRow = 0;
            int comboCelebrationColumn = 0;
            int chainCelebrationRow = 0;
            int chainCelebrationColumn = 0;

            // First look for horizontal matches
            for (int row = 0; row < rows; row++)
            {
                int horizontalMatchingBlocks = 0;

                for (int column = 0; column < columns; column++)
                {
                    // Skip non-idle blocks
                    if (blocks[row, column].State != BlockState.Idle)
                    {
                        continue;
                    }

                    // Search to the right for matching blocks
                    if (column + 1 < columns &&
                        blocks[row, column + 1].State == BlockState.Idle &&
                        blocks[row, column].Type == blocks[row, column + 1].Type)
                    {
                        horizontalMatchingBlocks++;
                    }
                    else
                    {
                        // Mark sequences of 2 or more matching blocks
                        if (horizontalMatchingBlocks >= 2)
                        {
                            for (int matchingColumn = horizontalMatchingBlocks; matchingColumn >= 0; matchingColumn--)
                            {
                                blocks[row, column - matchingColumn].Match();
                                matchingBlockCount++;
                                comboCelebrationRow = row;
                                comboCelebrationColumn = column - matchingColumn;
                                if (blocks[row, column - matchingColumn].ChainEligible)
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
            for (int column = 0; column < columns; column++)
            {
                int verticalMatchingBlocks = 0;

                for (int row = 0; row < rows; row++)
                {
                    // Skip non-idle blocks
                    if (blocks[row, column].State != BlockState.Idle)
                    {
                        continue;
                    }

                    // Search down for matching blocks
                    if (row + 1 < rows &&
                        blocks[row + 1, column].State == BlockState.Idle &&
                        blocks[row, column].Type == blocks[row + 1, column].Type)
                    {
                        verticalMatchingBlocks++;
                    }
                    else
                    {
                        // Mark sequences of 2 or more matching blocks
                        if (verticalMatchingBlocks >= 2)
                        {
                            for (int matchingRow = verticalMatchingBlocks; matchingRow >= 0; matchingRow--)
                            {
                                blocks[row - matchingRow, column].State = BlockState.Matched;
                                matchingBlockCount++;
                                comboCelebrationRow = row - matchingRow;
                                comboCelebrationColumn = column;
                                if (blocks[row - matchingRow, column].ChainEligible)
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
                Score += matchingBlockCount * scoreBlockComboInterval;
                celebrations.Add(new Celebration(matchingBlockCount.ToString(), comboCelebrationRow, comboCelebrationColumn));
                particleEmitters.Add(new ParticleEmitter(50, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)) + new Vector2(comboCelebrationColumn * Block.Width, comboCelebrationRow * Block.Width), new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f), Vector2.Zero, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()), 10, 1000));
                float pitch = 0.1f * (matchingBlockCount - 3);
                
                // Only play the combo sound if a chain sound isn't going to play (avoid cacophany!)
                if (!incrementChain)
                {
                    soundManager.Play("Celebration", 1.0f, pitch, 0.0f);
                }
            }

            // Handle chain continuation
            if (incrementChain)
            {
                chainCount++;
                Score += chainCount * scoreBlockChainInterval;
                celebrations.Add(new Celebration(chainCount.ToString() + "x", chainCelebrationRow, chainCelebrationColumn));
                particleEmitters.Add(new ParticleEmitter(50, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)) + new Vector2(chainCelebrationColumn * Block.Width, chainCelebrationRow * Block.Width), new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f), Vector2.Zero, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()), 10, 1000));
                float pitch = 0.1f * chainCount;
                soundManager.Play("Celebration", 1.0f, pitch, 0.0f);
            }

            // Setup matching blocks to pop
            int delayCounter = matchingBlockCount;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State == BlockState.Matched)
                    {
                        blocks[row, column].Flash(matchingBlockCount, delayCounter);
                        delayCounter--;
                    }
                }
            }
        }

        /// <summary>
        /// Detects blocks that are eligible to participage in chains based on blocks below
        /// that have finished popping
        /// </summary>
        private void DetectChain()
        {
            bool stopChain = true;

            // Detect blocks that are eligible to participate in chains
            for (int column = 0; column < columns; column++)
            {
                for (int row = rows - 1; row >= 0; row--)
                {
                    if (blocks[row, column].JustEmptied)
                    {
                        for (int chainEligibleRow = row - 1; chainEligibleRow >= 0; chainEligibleRow--)
                        {
                            if (blocks[chainEligibleRow, column].State == BlockState.Idle)
                            {
                                blocks[chainEligibleRow, column].ChainEligible = true;
                                stopChain = false;
                            }
                        }
                    }

                    blocks[row, column].JustEmptied = false;
                }
            }

            // Stop the current chain if all of the blocks are idle or empty
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State != BlockState.Idle && blocks[row, column].State != BlockState.Empty && blocks[row, column].State != BlockState.Sliding)
                    {
                        stopChain = false;
                    }
                }
            }

            if (stopChain)
            {
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        blocks[row, column].ChainEligible = false;
                    }
                }

                chainCount = 1;
            }
        }

        /// <summary>
        /// Applies gravity to blocks, making them fall when there are empty ones below them
        /// </summary>
        /// <param name="gameTime"></param>
        private void ApplyGravity(GameTime gameTime)
        {
            bool playLandSound = false;
            for (int column = 0; column < columns; column++)
            {
                bool emptyBlockDetected = false;

                for (int row = rows - 1; row >= 0; row--)
                {
                    if (blocks[row, column].State == BlockState.Empty)
                    {
                        emptyBlockDetected = true;
                    }

                    if (blocks[row, column].State == BlockState.Idle && emptyBlockDetected)
                    {
                        blocks[row, column].FallTarget = blocks[row + 1, column];
                        blocks[row, column].WaitToFall();
                    }

                    if (blocks[row, column].JustFell)
                    {
                        //If there are no more empty or falling blocks below, stop falling
                        if (row + 1 < rows && (blocks[row + 1, column].State == BlockState.Empty || blocks[row + 1, column].State == BlockState.Falling))
                        {
                            blocks[row, column].Fall();
                            blocks[row, column].FallTarget = blocks[row + 1, column];
                        }
                        else
                        {
                            blocks[row, column].Land();
                            playLandSound = true;
                            particleEmitters.Add(new ParticleEmitter(5, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)) + new Vector2(column * Block.Width, row * Block.Height + Block.Height), new Vector2(-0.1f, -0.1f), new Vector2(0.0f, 0.0f), Vector2.Zero, Color.White, 5, 1000));
                            particleEmitters.Add(new ParticleEmitter(5, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)) + new Vector2(column * Block.Width + Block.Width, row * Block.Height + Block.Height), new Vector2(0.0f, -0.1f), new Vector2(0.1f, 0.0f), Vector2.Zero, Color.White, 5, 1000));
                        }

                        blocks[row, column].JustFell = false;
                    }
                }
            }

            if (playLandSound)
            {
                soundManager.Play("BlockLand", 1.0f, 0.0f, 0.0f);
            }
        }

        private void Raise(GameTime gameTime)
        {
            // Determine whether the board should raise based on whether there are any non-empty/idle blocks
            bool raiseBoard = true;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State != BlockState.Empty &&
                        blocks[row, column].State != BlockState.Idle)
                    {
                        raiseBoard = false;
                    }
                }
            }

            // If the board should raise, increment the rise time elapsed, and after it exceeds the duration,
            // add the new row to the bottom
            if (raiseBoard)
            {
                if (raiseRateAccelerated)
                {
                    raiseTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds * raiseRateAcceleration;
                }
                else
                {
                    raiseTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (raiseTimeElapsed >= raiseDuration)
                {
                    raiseTimeElapsed = 0;
                    raiseRateAccelerated = false;

                    for (int row = 0; row < rows - 1; row++)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            blocks[row, column].Raise(blocks[row + 1, column]);
                        }
                    }

                    for(int column = 0; column < columns; column++)
                    {
                        blocks[rows - 1, column].Raise(nextRowBlocks[column]);

                        nextRowBlocks[column].Create();
                    }
                }
            }
        }

        /// <summary>
        /// Detects game over conditions
        /// </summary>
        private void DetectGameOver(GameTime gameTime)
        {
            // Determine if we're about to lose based on idle blocks in the top row
            bool pendingGameOver = false;
            for (int column = 0; column < columns; column++)
            {
                if(blocks[0, column].State == BlockState.Idle)
                {
                    pendingGameOver = true;
                }
            }

            // Determine whether there are any non-empty / non-idle blocks
            bool activeBlocks = false;
            for (int row = 0; row > rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State != BlockState.Idle &&
                        blocks[row, column].State != BlockState.Empty)
                    {
                        activeBlocks = true;
                    }
                }
            }

            if (pendingGameOver && !activeBlocks)
            {
                gameOverDelayTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (gameOverDelayTimeElapsed >= gameOverDelayDuration)
                {
                    state = BoardState.GameOver;
                }
            }
            else
            {
                gameOverDelayTimeElapsed = 0;
            }
        }

        /// <summary>
        /// Update the board
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="mouseManager"></param>
        public void Update(GameTime gameTime, MouseManager mouseManager, SoundManager soundManager)
        {
            switch (state)
            {
                case BoardState.Populating:
                    populatingTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (populatingTimeElapsed >= populatingDuration)
                    {
                        state = BoardState.CountingDown;
                        countdownTimeElapsed = 0;
                    }
                    break;
                
                case BoardState.CountingDown:
                    countdownTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (countdownTimeElapsed >= countdownDuration)
                    {
                        state = BoardState.Playing;
                    }
                    break;
                
                case BoardState.Playing:
                    DetectMatchingBlocks();

                    DetectChain();

                    ApplyGravity(gameTime);

                    HandleInput(gameTime, mouseManager);

                    Raise(gameTime);

                    DetectGameOver(gameTime);

                    // Updating blocks from the bottom up to account for falling logic
                    for (int row = rows - 1; row >= 0; row--)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            blocks[row, column].Update(gameTime);
                        }
                    }

                    // Update celebrations
                    foreach (Celebration celebration in celebrations)
                    {
                        celebration.Update(gameTime);
                    }

                    // Update particle emitters
                    foreach (ParticleEmitter pe in particleEmitters)
                    {
                        pe.Update(gameTime);
                    }
                    break;
                
                case BoardState.GameOver: break;
            }

            
        }

        /// <summary>
        /// Draw the board
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphicsManager"></param>
        public void Draw(GameTime gameTime, GraphicsManager graphicsManager)
        {
            switch (state)
            {
                case BoardState.Populating:
                    // Draw a black background
                    graphicsManager.DrawRectangle("Blank", new Rectangle(ScreenManager.WorldWidth / 2, ScreenManager.WorldHeight / 2, columns * Block.Width, rows * Block.Height), Color.Black, 0.0f, 1.0f);

                    // Draw the blocks populating the board
                    int yOffset = 0;
                    for (int row = 0; row < rows; row++)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            yOffset = (int)Tween.ElasticEaseOut(Math.Max(populatingTimeElapsed - populatingDelays[row, column], 0), -1 * rows * Block.Height, rows * Block.Height, populatingDuration);
                            blocks[row, column].Draw(gameTime, graphicsManager, row, column, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, yOffset), false);
                        }
                    }
                    break;
                
                case BoardState.CountingDown:
                    // Draw a black background
                    graphicsManager.DrawRectangle("Blank", new Rectangle(ScreenManager.WorldWidth / 2, ScreenManager.WorldHeight / 2, columns * Block.Width, rows * Block.Height), Color.Black, 0.0f, 1.0f);

                    // Draw the matrix of blocks
                    for (int row = 0; row < rows; row++)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            blocks[row, column].Draw(gameTime, graphicsManager, row, column, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)), false);
                        }
                    }

                    graphicsManager.DrawText(Math.Ceiling(3 - countdownTimeElapsed / 1000).ToString(), new Vector2(ScreenManager.WorldWidth / 2, ScreenManager.WorldHeight / 2), Color.White, true);
                    break;
                
                case BoardState.Playing:
                    // Draw a black background
                    graphicsManager.DrawRectangle("Blank", new Rectangle(ScreenManager.WorldWidth / 2, ScreenManager.WorldHeight / 2, columns * Block.Width, rows * Block.Height), Color.Black, 0.0f, 1.0f);

                    // Draw the matrix of blocks
                    for (int row = 0; row < rows; row++)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            blocks[row, column].Draw(gameTime, graphicsManager, row, column, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)), false);
                        }
                    }

                    // Draw the next row of blocks
                    for (int column = 0; column < columns; column++)
                    {
                        nextRowBlocks[column].Draw(gameTime, graphicsManager, rows, column, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)), true);
                    }

                    // Draw the score display
                    scoreDisplay += (Score - scoreDisplay) * 0.1;
                    if (Math.Abs(Score - scoreDisplay) < 1)
                        scoreDisplay = Score;
                    graphicsManager.DrawText(Math.Floor(scoreDisplay).ToString(), new Vector2(50, 50), Color.White, false);

                    // Draw celebrations
                    foreach (Celebration celebration in celebrations)
                    {
                        celebration.Draw(gameTime, graphicsManager, new Vector2(ScreenManager.WorldWidth / 2 - columns * Block.Width / 2, (int)(-1 * Block.Height * raiseTimeElapsed / raiseDuration)));
                    }

                    // Draw particles
                    foreach (ParticleEmitter pe in particleEmitters)
                    {
                        pe.Draw(gameTime, graphicsManager);
                    }
                    break;
                
                case BoardState.GameOver:
                    graphicsManager.DrawText("GAME OVER", new Vector2(ScreenManager.WorldWidth / 2, ScreenManager.WorldHeight / 2), Color.White, true);
                    break;
            }
        }
    }
}
