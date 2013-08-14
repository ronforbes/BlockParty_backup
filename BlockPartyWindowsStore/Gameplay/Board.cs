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
        Screen screen;
        public Screen Screen
        {
            get { return screen; }
        }

        public enum BoardState
        {
            Populating,
            CountingDown,
            Playing,
            GameOver
        }

        BoardState state = BoardState.Populating;
        public BoardState State
        {
            get { return state; }
        }

        /// <summary>
        /// Number of rows and columns on the board
        /// </summary>
        const int rows = 10;
        public int Rows
        {
            get { return rows; }
        }

        const int columns = 10;
        public int Columns
        {
            get { return columns; }
        }

        /// <summary>
        /// Specifies how many rows of the board are empty at initial state
        /// </summary>
        const int initialEmptyRows = 5;

        /// <summary>
        /// The matrix of blocks
        /// </summary>
        Block[,] blocks;
        public Block[,] Blocks
        {
            get { return blocks; }
        }

        /// <summary>
        /// The next row of blocks to add
        /// </summary>
        Block[] nextRowBlocks;

        /// <summary>
        /// Random number generator
        /// </summary>
        Random random = new Random();

        

        /// <summary>
        /// Count of the current chain
        /// </summary>
        int chainCount = 0;

        /// <summary>
        /// The amount of time that the board has raised (in milliseconds)
        /// </summary>
        TimeSpan raiseTimeElapsed;
        public TimeSpan RaiseTimeElapsed
        {
            get { return raiseTimeElapsed; }
        }

        /// <summary>
        /// Duration of time required to raise the board (in milliseconds)
        /// </summary>
        readonly TimeSpan raiseDuration = TimeSpan.FromSeconds(10);
        public TimeSpan RaiseDuration
        {
            get { return raiseDuration; }
        }

        /// <summary>
        /// Amount of time that the game over has been delayed (in milliseconds)
        /// </summary>
        TimeSpan gameOverDelayTimeElapsed;

        /// <summary>
        /// Duration of time that game over is delayed (in milliseconds)
        /// </summary>
        readonly TimeSpan gameOverDelayDuration = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Amount of time that the board has been populating (in milliseconds)
        /// </summary>
        TimeSpan populatingTimeElapsed = TimeSpan.Zero;
        public TimeSpan PopulatingTimeElapsed
        {
            get { return populatingTimeElapsed; }
        }

        /// <summary>
        /// Duration of time to populate the board (in milliseconds)
        /// </summary>
        readonly TimeSpan populatingDuration = TimeSpan.FromSeconds(1);
        public TimeSpan PopulatingDuration
        {
            get { return populatingDuration; }
        }

        /// <summary>
        /// Amount of time to delay each block's population on the board (in milliseconds)
        /// </summary>
        TimeSpan[,] populatingDelays = new TimeSpan[rows, columns];

        /// <summary>
        /// Amount of time that has been counted down before gameplay (in milliseconds)
        /// </summary>
        TimeSpan countdownTimeElapsed;
        public TimeSpan CountdownTimeElapsed
        {
            get { return countdownTimeElapsed; }
        }

        /// <summary>
        /// Duration of time to count down before gameplay (in milliseconds)
        /// </summary>
        readonly TimeSpan countdownDuration = TimeSpan.FromSeconds(3);
        public TimeSpan CountdownDuration
        {
            get { return countdownDuration; }
        }

        /// <summary>
        /// The player's score
        /// </summary>
        int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public const int ScoreBlockPop = 10;
        const int scoreBlockComboInterval = 50;
        const int scoreBlockChainInterval = 100;

        int level = 1;
        public int Level
        {
            get { return level; }
        }

        int raiseRateNormal = 1;
        const int raiseRateAccelerated = 33;
        public int RaiseRateAccelerated
        {
            get { return raiseRateAccelerated; }
        }

        int raiseRate = 1;
        public int RaiseRate
        {
            get { return raiseRate; }
            set { raiseRate = value; }
        }

        List<Celebration> celebrations;

        List<ParticleEmitter> particleEmitters;
        public List<ParticleEmitter> ParticleEmitters
        {
            get { return particleEmitters; }
        }

        BoardRenderer renderer;
        public BoardRenderer Renderer
        {
            get { return renderer; }
        }

        BoardController controller;

        /// <summary>
        /// Construct the board
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public Board(Screen screen)
        {
            this.screen = screen;   

            // Create the board
            blocks = new Block[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    blocks[row, column] = new Block(this, screen, row, column);

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
                nextRowBlocks[column] = new Block(this, screen, rows, column);
                nextRowBlocks[column].Create();
                nextRowBlocks[column].State = Block.BlockState.Preview;
            }

            // Initialize celebrations
            celebrations = new List<Celebration>();

            particleEmitters = new List<ParticleEmitter>();

            renderer = new BoardRenderer(this);
            controller = new BoardController(this);
        }

        public void LoadContent()
        {
            renderer.LoadContent();

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    blocks[row, column].LoadContent();
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
            switch (state)
            {
                case BoardState.Populating:
                    populatingTimeElapsed = populatingTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (populatingTimeElapsed >= populatingDuration)
                    {
                        state = BoardState.CountingDown;
                        countdownTimeElapsed = TimeSpan.Zero;
                    }
                    break;

                case BoardState.CountingDown:
                    countdownTimeElapsed = countdownTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                    if (countdownTimeElapsed >= countdownDuration)
                    {
                        state = BoardState.Playing;
                    }
                    break;

                case BoardState.Playing:
                    DetectMatchingBlocks();

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

                    DetectChain();

                    ApplyGravity(gameTime);

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

                    UpdateLevel();
                    break;

                case BoardState.GameOver: break;
            }
        }

        /// <summary>
        /// Detect matching blocks and set their state appropriately
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
            for (int row = 0; row < rows; row++)
            {
                int horizontalMatchingBlocks = 0;

                for (int column = 0; column < columns; column++)
                {
                    // Skip non-idle blocks
                    if (blocks[row, column].State != Block.BlockState.Idle)
                    {
                        continue;
                    }

                    // Search to the right for matching blocks
                    if (column + 1 < columns &&
                        blocks[row, column + 1].State == Block.BlockState.Idle &&
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
                    if (blocks[row, column].State != Block.BlockState.Idle)
                    {
                        continue;
                    }

                    // Search down for matching blocks
                    if (row + 1 < rows &&
                        blocks[row + 1, column].State == Block.BlockState.Idle &&
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
                                blocks[row - matchingRow, column].State = Block.BlockState.Matched;
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
                score += matchingBlockCount * scoreBlockComboInterval;
                celebrations.Add(new Celebration(this, matchingBlockCount.ToString(), comboCelebrationRow, comboCelebrationColumn));
                //particleEmitters.Add(new ParticleEmitter(screen, 50, new Vector2(screen.ScreenManager.WorldViewport.Width / 2 - columns * BlockRenderer.Width / 2, (int)(-1 * BlockRenderer.Height * raiseTimeElapsed.TotalMilliseconds / raiseDuration.TotalMilliseconds)) + new Vector2(comboCelebrationColumn * BlockRenderer.Width, comboCelebrationRow * BlockRenderer.Width), new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f), Vector2.Zero, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()), new Vector2(10, 10), TimeSpan.FromMilliseconds(1000)));
                float pitch = 0.1f * (matchingBlockCount - 3);

                // Only play the combo sound if a chain sound isn't going to play (avoid cacophany!)
                if (!incrementChain)
                {
                    screen.ScreenManager.AudioManager.Play("Celebration", 1.0f, pitch, 0.0f);
                }
            }

            // Handle chain continuation
            if (incrementChain)
            {
                chainCount++;
                score += chainCount * scoreBlockChainInterval;
                celebrations.Add(new Celebration(this, chainCount.ToString() + "x", chainCelebrationRow, chainCelebrationColumn));
                //particleEmitters.Add(new ParticleEmitter(screen, 50, new Vector2(screen.ScreenManager.WorldViewport.Width / 2 - columns * BlockRenderer.Width / 2, (int)(-1 * BlockRenderer.Height * raiseTimeElapsed.TotalMilliseconds / raiseDuration.TotalMilliseconds)) + new Vector2(chainCelebrationColumn * BlockRenderer.Width, chainCelebrationRow * BlockRenderer.Width), new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f), Vector2.Zero, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()), new Vector2(10, 10), TimeSpan.FromMilliseconds(1000)));
                float pitch = 0.1f * chainCount;
                screen.ScreenManager.AudioManager.Play("Celebration", 1.0f, pitch, 0.0f);
            }

            // Setup matching blocks to pop
            int delayCounter = matchingBlockCount;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State == Block.BlockState.Matched)
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
        void DetectChain()
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
                            if (blocks[chainEligibleRow, column].State == Block.BlockState.Idle)
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
                    if (blocks[row, column].State != Block.BlockState.Idle && blocks[row, column].State != Block.BlockState.Empty && blocks[row, column].State != Block.BlockState.Sliding)
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
        void ApplyGravity(GameTime gameTime)
        {
            bool playLandSound = false;
            for (int column = 0; column < columns; column++)
            {
                bool emptyBlockDetected = false;

                for (int row = rows - 1; row >= 0; row--)
                {
                    if (blocks[row, column].State == Block.BlockState.Empty)
                    {
                        emptyBlockDetected = true;
                    }

                    if (blocks[row, column].State == Block.BlockState.Idle && emptyBlockDetected)
                    {
                        blocks[row, column].FallTarget = blocks[row + 1, column];
                        blocks[row, column].WaitToFall();
                    }

                    if (blocks[row, column].JustFell)
                    {
                        //If there are no more empty or falling blocks below, stop falling
                        if (row + 1 < rows && (blocks[row + 1, column].State == Block.BlockState.Empty || blocks[row + 1, column].State == Block.BlockState.Falling))
                        {
                            blocks[row, column].Fall();
                            blocks[row, column].FallTarget = blocks[row + 1, column];
                        }
                        else
                        {
                            blocks[row, column].Land();
                            playLandSound = true;
                            particleEmitters.Add(new ParticleEmitter(screen, 3, new Rectangle(blocks[row, column].Renderer.Rectangle.X, blocks[row, column].Renderer.Rectangle.Y + blocks[0, 0].Renderer.Height, 5, 5), new Vector2(-0.05f, -0.05f), new Vector2(0.0f, 0.0f), Vector2.Zero, Color.White, TimeSpan.FromMilliseconds(1000)));
                            particleEmitters.Add(new ParticleEmitter(screen, 3, new Rectangle(blocks[row, column].Renderer.Rectangle.X + blocks[0, 0].Renderer.Width, blocks[row, column].Renderer.Rectangle.Y + blocks[0, 0].Renderer.Height, 5, 5), new Vector2(0.0f, -0.05f), new Vector2(0.05f, 0.0f), Vector2.Zero, Color.White, TimeSpan.FromMilliseconds(1000)));
                        }

                        blocks[row, column].JustFell = false;
                    }
                }
            }

            if (playLandSound)
            {
                screen.ScreenManager.AudioManager.Play("BlockLand", 1.0f, 0.0f, 0.0f);
            }
        }

        void Raise(GameTime gameTime)
        {
            int rate = raiseRate;
            // Determine whether the board should raise based on whether there are any non-empty/idle blocks
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (blocks[row, column].State != Block.BlockState.Empty &&
                        blocks[row, column].State != Block.BlockState.Idle &&
                        blocks[row, column].State != Block.BlockState.Sliding)
                    {
                        if (rate == raiseRateNormal)
                        {
                            rate = 0;
                        }
                    }

                    // Stop raising if any cells in the top row are occupied
                    if (row == 0)
                    {
                        if (blocks[row, column].State != Block.BlockState.Empty)
                        {
                            if (rate == raiseRateNormal)
                            {
                                rate = 0;
                            }
                        }
                    }
                }
            }

            raiseTimeElapsed = raiseTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * rate));

            if (raiseTimeElapsed >= raiseDuration)
            {
                raiseTimeElapsed = TimeSpan.Zero;
                raiseRate = raiseRateNormal;

                for (int row = 0; row < rows - 1; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        blocks[row, column].Raise(blocks[row + 1, column]);
                    }
                }

                for (int column = 0; column < columns; column++)
                {
                    blocks[rows - 1, column].Raise(nextRowBlocks[column]);
                    blocks[rows - 1, column].State = Block.BlockState.Idle;

                    nextRowBlocks[column].Create();
                    nextRowBlocks[column].State = Block.BlockState.Preview;
                }
            }
        }

        /// <summary>
        /// Detects game over conditions
        /// </summary>
        void DetectGameOver(GameTime gameTime)
        {
            // Determine if we're about to lose based on idle blocks in the top row
            bool pendingGameOver = false;
            for (int column = 0; column < columns; column++)
            {
                if (blocks[0, column].State == Block.BlockState.Idle)
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
                    if (blocks[row, column].State != Block.BlockState.Idle &&
                        blocks[row, column].State != Block.BlockState.Empty)
                    {
                        activeBlocks = true;
                    }
                }
            }

            if (pendingGameOver && !activeBlocks)
            {
                gameOverDelayTimeElapsed = gameOverDelayTimeElapsed.Add(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

                if (gameOverDelayTimeElapsed >= gameOverDelayDuration)
                {
                    state = BoardState.GameOver;
                }
            }
            else
            {
                gameOverDelayTimeElapsed = TimeSpan.Zero;
            }
        }

        void UpdateLevel()
        {
            level = (int)MathHelper.Clamp(score / 1000, 1, 99);
            raiseRateNormal = Math.Max(level / 3, 1);
        }

        

        /// <summary>
        /// Process user input
        /// </summary>
        /// <param name="mouseManager"></param>
        public void HandleInput(GameTime gameTime)
        {
            controller.HandleInput(gameTime);
        }

        /// <summary>
        /// Draw the board
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphicsManager"></param>
        public void Draw(GameTime gameTime)
        {
            renderer.Draw(gameTime);
        }
    }
}
