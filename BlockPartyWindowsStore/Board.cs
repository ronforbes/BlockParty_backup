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
        const int initialEmptyRows = 0;

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
        /// Duration of time that game over is delayed
        /// </summary>
        const double gameOverDelayDuration = 10000;

        /// <summary>
        /// Determines whether game over conditions have been met
        /// </summary>
        bool gameOver = false;

        /// <summary>
        /// Construct the board
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public Board(GraphicsDevice graphicsDevice)
        {
            // Create the random number generator
            random = new Random();

            // Create the board
            blocks = new Block[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    blocks[row, column] = new Block(graphicsDevice);

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
                nextRowBlocks[column] = new Block(graphicsDevice);
                nextRowBlocks[column].Create();
            }
        }

        /// <summary>
        /// Process user input
        /// </summary>
        /// <param name="mouseManager"></param>
        public void HandleInput(MouseManager mouseManager)
        {
            // If the mouse has been pressed over a valid block, select the block that it's hovering over
            if (mouseManager.LeftButtonPressed)
            {
                // Determine which block the mouse is hovering over
                int row = mouseManager.Y / Block.Height;
                int column = mouseManager.X / Block.Width;

                if (row >= 0 && row < rows && column >= 0 && column < columns)
                {
                    if (blocks[row, column].State == BlockState.Idle)
                    {
                        selectedRow = row;
                        selectedColumn = column;
                        blocks[selectedRow, selectedColumn].Selected = true;
                    }
                }
            }

            // If a block is selected, swap it based on the mouse's position
            if (selectedRow != -1 && selectedColumn != -1)
            {
                // Swap the blocks if the mouse has moved to a different column
                if (mouseManager.X < selectedColumn * Block.Width &&
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

                    selectedColumn--;
                }

                if (mouseManager.X > (selectedColumn + 1) * Block.Width &&
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
                                if (blocks[row, column - matchingColumn].ChainEligible)
                                {
                                    incrementChain = true;
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
                                if (blocks[row - matchingRow, column].ChainEligible)
                                {
                                    incrementChain = true;
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
                // TODO: handle combos
            }

            // Handle chain continuation
            if (incrementChain)
            {
                chainCount++;
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
                            }
                        }

                        break;
                    }

                    blocks[row, column].JustEmptied = false;
                }
            }

            // Stop the current chain if all of the blocks are idle or empty
            bool stopChain = true;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State != BlockState.Idle && blocks[row, column].State != BlockState.Empty)
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
                            blocks[row, column].State = BlockState.Idle;
                        }

                        blocks[row, column].JustFell = false;
                    }
                }
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
                raiseTimeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (raiseTimeElapsed >= raiseDuration)
                {
                    raiseTimeElapsed = 0;

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
                    gameOver = true;
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
        public void Update(GameTime gameTime, MouseManager mouseManager)
        {
            if (!gameOver)
            {
                HandleInput(mouseManager);

                DetectMatchingBlocks();

                DetectChain();

                ApplyGravity(gameTime);

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
            }
        }

        /// <summary>
        /// Draw the board
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphicsManager"></param>
        public void Draw(GameTime gameTime, GraphicsManager graphicsManager)
        {
            // Draw the matrix of blocks
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    blocks[row, column].Draw(gameTime, graphicsManager, row, column);
                }
            }

            if (gameOver)
            {
                graphicsManager.DrawText("GAME OVER", Vector2.Zero, Color.White);
            }
        }
    }
}
