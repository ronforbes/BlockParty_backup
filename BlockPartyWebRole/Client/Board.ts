/// <reference path="Mouse.ts" />
/// <reference path="Block.ts" />

class Board {
    public Score: number = 0;

    private board: Block[][];
    private newRow: Block[];
    private slideInProgress: bool = false;
    private slideDirection: number = 0;
    private slideTimer: number = 0;
    private slideDuration: number = 50;
    private chainCount: number = 1;
    private raiseStack: bool = false;
    private riseTimer: number = 0;
    private riseDuration: number = 10000;
    private riseForcedRate: number = 15;
    private loseTimer: number = 0;
    private loseDuration: number = 10000;
    private lost: bool = false;
    private previousMouseState: MouseState;
    private previousClickTime: number;
    private selectedRow: number = -1;
    private selectedColumn: number = -1;

    private ROWS: number = 10;
    private COLUMNS: number = 6;
    private STARTING_ROW: number = 5;
    private SCORE_FORCED_RISE: number = 1;

    constructor() {
        // Initialize the board
        this.board = new Block[][];
        for (var row = 0; row < this.ROWS; row++) {
            this.board[row] = new Block[];
            for (var column = 0; column < this.COLUMNS; column++) {
                this.board[row][column] = new Block();
                if (row < this.STARTING_ROW) {
                    this.board[row][column].State = BlockState.Empty;
                }
                else {
                    this.board[row][column].State = BlockState.Idle;
                    this.board[row][column].Type = Math.floor(Math.random() * Block.TYPE_COUNT);
                }
            }
        }

        // Initialize the new row of blocks
        this.newRow = new Block[];
        for (var column = 0; column < this.COLUMNS; column++) {
            this.newRow[column] = new Block();
            this.newRow[column].State = BlockState.Idle;
            this.newRow[column].Type = Math.floor(Math.random() * Block.TYPE_COUNT);
        }

        // Initialze mouse state
        this.previousMouseState = Mouse.GetState();
    }

    private HandleInput(elapsedMilliseconds: number) {
        // Determine which block is being hovered over
        var mouseState: MouseState = Mouse.GetState();
        var row: number = Math.floor(((BlockRenderer.Height * this.riseTimer / this.riseDuration) + mouseState.Y) / BlockRenderer.Height);
        var column: number = Math.floor(mouseState.X / BlockRenderer.Width);

        // If the mouse is hovering over a valid block, select or slide it
        if (row >= 0 && row < this.ROWS && column >= 0 && column < this.COLUMNS) {
            // If the mouse has been clicked, select the block that the mouse is hovering over
            if (mouseState.LeftButton == true && this.previousMouseState.LeftButton == false) {
                var delta = Date.now() - this.previousClickTime;
                if (delta <= Mouse.DoubleClickDelay) {
                    if (Math.sqrt(Math.pow(Math.abs(mouseState.X - this.previousMouseState.X), 2) + Math.pow(Math.abs(mouseState.Y - this.previousMouseState.Y), 2)) < 10) {
                        this.raiseStack = true;
                    }        
                }
                else {
                    this.raiseStack = false;
                }
                
                this.previousClickTime = Date.now();
                
                if (this.board[row][column].State == BlockState.Idle) {
                    this.selectedRow = row;
                    this.selectedColumn = column;
                }
            }

            // If a block is selected, swap it based on the mouse's position
            if (this.selectedRow != -1 && this.selectedColumn != -1) {
                // Swap blocks if the mouse has moved to a different column
                if (column < this.selectedColumn &&
                    this.board[this.selectedRow][this.selectedColumn].State == BlockState.Idle &&
                    (this.board[this.selectedRow][this.selectedColumn - 1].State == BlockState.Idle ||
                    this.board[this.selectedRow][this.selectedColumn - 1].State == BlockState.Empty)) {
                    
                    // Swap the selected block with the one to its left
                    this.slideInProgress = true;
                    this.slideDirection = -1;
                }

                if (column > this.selectedColumn &&
                    this.board[this.selectedRow][this.selectedColumn].State == BlockState.Idle &&
                    (this.board[this.selectedRow][this.selectedColumn + 1].State == BlockState.Idle ||
                    this.board[this.selectedRow][this.selectedColumn + 1].State == BlockState.Empty)) {
                    
                    // Swap the selected block with the one to its right
                    this.slideInProgress = true;
                    this.slideDirection = 1;
                }
            }
        }

        // Deselect the block if the mouse button is no longer being held
        if (mouseState.LeftButton == false) {
            this.selectedRow = -1;
            this.selectedColumn = -1;
        }

        if(this.raiseStack) {
            this.riseTimer += elapsedMilliseconds * this.riseForcedRate;
            this.Score += this.SCORE_FORCED_RISE;

            if (this.riseTimer >= this.riseDuration && !mouseState.LeftButton) {
                this.raiseStack = false;
            }
        }

        this.previousMouseState = mouseState;
    }

    private HandleSlidingBlocks(elapsedMilliseconds: number) {
        if (this.slideInProgress == true) {
            this.slideTimer += elapsedMilliseconds;
            if (this.slideTimer >= this.slideDuration) {
                this.slideTimer = 0;

                var adjacentBlock: Block = this.board[this.selectedRow][this.selectedColumn + this.slideDirection];
                this.board[this.selectedRow][this.selectedColumn + this.slideDirection] = this.board[this.selectedRow][this.selectedColumn];
                this.board[this.selectedRow][this.selectedColumn] = adjacentBlock;

                this.selectedColumn += this.slideDirection;

                this.slideInProgress = false;
                this.slideDirection = 0;
            }
        }
    }

    private DetectMatches() {
        var incrementChain: bool = false;

        // Look for horizontal matches
        for (var row = 0; row < this.ROWS; row++) {
            var horizontallyMatchingBlocks = 0;
            for (var column = 0; column < this.COLUMNS; column++) {
                // Skip empty cells
                if (this.board[row][column].State != BlockState.Idle) {
                    continue;
                }

                // Search to the right for matching blocks
                if (column + 1 < this.COLUMNS &&
                    this.board[row][column + 1].State == BlockState.Idle &&
                    this.board[row][column].Type == this.board[row][column + 1].Type) {
                    horizontallyMatchingBlocks++;
                }
                else {
                    // Mark sequences of 2 or more blocks matching the first
                    if (horizontallyMatchingBlocks >= 2) {
                        for (var matchingColumn = horizontallyMatchingBlocks; matchingColumn >= 0; matchingColumn--) {
                            this.board[row][column - matchingColumn].State = BlockState.Matched;
                            if (this.board[row][column - matchingColumn].EligibleForChain == true) {
                                incrementChain = true;
                            }
                        }
                    }

                    // Reset the matching block counter
                    horizontallyMatchingBlocks = 0;
                }
            }
        }

        // Look for vertical matches
        for (var column = 0; column < this.COLUMNS; column++) {
            var verticallyMatchingBlocks = 0;
            for (var row = 0; row < this.ROWS; row++) {
                // Skip empty cells
                if (this.board[row][column].State != BlockState.Idle) {
                    continue;
                }

                // Search below for matching blocks
                if (row + 1 < this.ROWS &&
                    this.board[row + 1][column].State == BlockState.Idle &&
                    this.board[row][column].Type == this.board[row + 1][column].Type) {
                    verticallyMatchingBlocks++;
                }
                else {
                    // Mark sequences of 2 or more blocks matching the first
                    if (verticallyMatchingBlocks >= 2) {
                        for (var matchingRow = verticallyMatchingBlocks; matchingRow >= 0; matchingRow--) {
                            this.board[row - matchingRow][column].State = BlockState.Matched;
                            if (this.board[row - matchingRow][column].EligibleForChain == true) {
                                incrementChain = true;
                            }
                        }
                    }

                    // Reset the matching block counter
                    verticallyMatchingBlocks = 0;
                }
            }
        }

        if (incrementChain == true) {
            this.chainCount++;
        }
    }

    private ProcessMatchedBlocks() {
        var matchedBlocks: number = 0;
        var incrementChain: bool = false;

        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                if (this.board[row][column].State == BlockState.Matched) {
                    matchedBlocks++;
                }
            }
        }

        var delayCounter = matchedBlocks;

        // Score combos
        if (matchedBlocks > 3) {
            this.Score += matchedBlocks * 100;
        }

        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                if (this.board[row][column].State == BlockState.Matched) {
                    this.board[row][column].State = BlockState.Flashing;
                    this.board[row][column].PopDelayDuration = (matchedBlocks - delayCounter) * Block.POP_DELAY_INTERVAL;
                    this.board[row][column].EmptyDelayDuration = delayCounter * Block.EMPTY_DELAY_INTERVAL;
                    delayCounter--;
                }
            }
        }
    }

    private DetectChains() {
        for (var column = 0; column < this.COLUMNS; column++) {
            for (var row = this.ROWS - 1; row >= 0; row--) {
                if (this.board[row][column].JustBecameEmpty == true) {
                    for (var chainEligibleRow = row - 1; chainEligibleRow >= 0; chainEligibleRow--) {
                        if (this.board[chainEligibleRow][column].State == BlockState.Idle) {
                            this.board[chainEligibleRow][column].EligibleForChain = true;
                        }
                    }
                }

                this.board[row][column].JustBecameEmpty = false;
            }
        }
    }

    private DetectChainStop() {
        var stopChain: bool = true;
        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                if (this.board[row][column].State != BlockState.Idle && this.board[row][column].State != BlockState.Empty) {
                    stopChain = false;
                }
            }
        }

        if (stopChain == true) {
            for (var row = 0; row < this.ROWS; row++) {
                for (var column = 0; column < this.COLUMNS; column++) {
                    this.board[row][column].EligibleForChain = false;
                }
            }

            this.chainCount = 1;
        }
    }

    private ApplyGravity(elapsedMilliseconds: number) {
        // Mark blocks above empty cells as waiting to fall
        for (var column = 0; column < this.COLUMNS; column++) {
            var emptyBlockDetected: bool = false;

            for (var row = this.ROWS - 1; row >= 0; row--) {
                if (this.board[row][column].State == BlockState.Empty) {
                    emptyBlockDetected = true;
                }

                if (this.board[row][column].State == BlockState.Idle && emptyBlockDetected) {
                    this.board[row][column].State = BlockState.WaitingToFall;
                }

                if (this.board[row][column].State == BlockState.Falling) {
                    if (row + 1 < this.ROWS && (this.board[row + 1][column].State == BlockState.Empty || this.board[row + 1][column].State == BlockState.Falling)) {
                        if (this.board[row][column].FallTimer >= Block.FALL_DURATION) {
                            this.board[row][column].FallTimer = 0;
                            this.board[row + 1][column].Type = this.board[row][column].Type;
                            this.board[row + 1][column].State = BlockState.Falling;
                            this.board[row + 1][column].EligibleForChain = this.board[row][column].EligibleForChain;
                            this.board[row][column].State = BlockState.Empty;
                            this.board[row][column].EligibleForChain = false;
                        }
                        else {
                            this.board[row][column].FallTimer += elapsedMilliseconds;
                        }
                    }
                    else {
                        this.board[row][column].State = BlockState.Idle;
                    }
                }
            }
        }
    }

    private RaiseBoard(elapsedMilliseconds: number) {
        // Determine whether the board should raise by searching for blocks are in a non-idle/empty state
        var raiseBoard: bool = true;
        var losing: bool = false;
        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                if (row == 0) {
                    // If there are any idle blocks in the top row, stop raising the board and start the losing condition timer
                    if (this.board[row][column].State == BlockState.Idle) {
                        raiseBoard = false;
                        losing = true;
                    }
                }

                // If there are any non-idle / non-empty blocks, don't raise the board
                if (this.board[row][column].State != BlockState.Empty &&
                    this.board[row][column].State != BlockState.Idle) {
                    raiseBoard = false;
                }
            }
        }

        // If the board should rise, increment the rising timer and check whether it's met its duration
        if (raiseBoard) {
            this.riseTimer += elapsedMilliseconds;

            if (this.riseTimer >= this.riseDuration) {
                this.riseTimer = 0;

                for (var row = 0; row < this.ROWS - 1; row++) {
                    for (var column = 0; column < this.COLUMNS; column++) {
                        this.board[row][column].State = this.board[row + 1][column].State;
                        this.board[row][column].Type = this.board[row + 1][column].Type;
                    }
                }

                for (var column = 0; column < this.COLUMNS; column++) {
                    this.board[this.ROWS - 1][column].State = this.newRow[column].State;
                    this.board[this.ROWS - 1][column].Type = this.newRow[column].Type;

                    this.newRow[column].Type = Math.floor(Math.random() * Block.TYPE_COUNT);
                }
            }
        }

        if (losing) {
            this.loseTimer += elapsedMilliseconds;

            // If the losing timer has met its duration, end the game
            if (this.loseTimer >= this.loseDuration) {
                this.lost = true;
            }
        }
        else {
            this.loseTimer = 0;
        }
    }

    private UpdateBlocks(elapsedMilliseconds: number) {
        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                this.board[row][column].Update(elapsedMilliseconds, this);
            }
        }
    }

    private Draw(elapsedMilliseconds: number) {
        // Draw the board

        // Draw the background
        Graphics.DrawFullscreenRectangle("#222222");

        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                var color: string;
                var text: string;
                var x: number = 0;
                var y: number = 0;

                switch (this.board[row][column].Type) {
                    case BlockType.A: color = "red"; break;
                    case BlockType.B: color = "green"; break;
                    case BlockType.C: color = "blue"; break;
                    case BlockType.D: color = "cyan"; break;
                    case BlockType.E: color = "magenta"; break;
                    case BlockType.F: color = "yellow"; break;
                }

                switch (this.board[row][column].State) {
                    case BlockState.Empty: color = "#000000"; break;
                    case BlockState.Idle: break;
                    case BlockState.WaitingToFall: break;
                    case BlockState.Falling: y = this.board[row][column].FallTimer * BlockRenderer.Height / Block.FALL_DURATION; break;
                    case BlockState.Matched: break;
                    case BlockState.Flashing:
                        if (Date.now() % 100 > 50) {
                            color = "white";
                        }
                        break;
                    case BlockState.WaitingToPop: break;
                    case BlockState.Popping: break;
                    case BlockState.WaitingToEmpty: color = "black"; break;
                }

                if (this.board[row][column].EligibleForChain == true) {
                    text = "E";
                }
                else {
                    text = "";
                }

                if (this.slideInProgress == true &&
                    row == this.selectedRow) {
                    if (column == this.selectedColumn) {
                        x = this.slideTimer * BlockRenderer.Width / this.slideDuration * this.slideDirection;
                    }
                    if (column == this.selectedColumn + this.slideDirection) {
                        x = -1 * this.slideTimer * BlockRenderer.Width / this.slideDuration * this.slideDirection;
                    }
                }

                y -= BlockRenderer.Height * this.riseTimer / this.riseDuration;

                Graphics.DrawRectangle(new Vector2(column * BlockRenderer.Width + x /*+ (Graphics.WorldWidth / 2 - BlockRenderer.Width * this.COLUMNS / 2)*/, row * BlockRenderer.Height + y), BlockRenderer.Width, BlockRenderer.Height, 1, "black", color);
                //Graphics.DrawText(text, new Vector2(column * BlockRenderer.Width + BlockRenderer.Width / 2 + x, row * BlockRenderer.Height + BlockRenderer.Height / 2 + y), "white");
            }
        }

        // Draw the new row of blocks
        for (var column = 0; column < this.COLUMNS; column++) {
            var color: string;
            var text: string;
            var x: number = 0;
            var y: number = 0;

            switch (this.newRow[column].Type) {
                case BlockType.A: color = "#880000"; break;
                case BlockType.B: color = "#008800"; break;
                case BlockType.C: color = "#000088"; break;
                case BlockType.D: color = "#008888"; break;
                case BlockType.E: color = "#880088"; break;
                case BlockType.F: color = "#888800"; break;
            }

            //Graphics.DrawRectangle(new Vector2(column * BlockRenderer.Width /*+ (Graphics.WorldWidth / 2 - BlockRenderer.Width * this.COLUMNS / 2)*/, this.ROWS * BlockRenderer.Height - (BlockRenderer.Height * this.riseTimer / this.riseDuration)), BlockRenderer.Width, BlockRenderer.Height, 1, "black", color);
        }

        //Graphics.DrawText("Score: " + this.Score.toString(), new Vector2(70, 10), "white");
        //Graphics.DrawText("Chain Count: " + this.chainCount.toString(), new Vector2(70, 15), "white");

        if (this.lost) {
            //Graphics.DrawText("You lose... LOSER!!!", new Vector2(50, 50), "white");
        }
    }

    public Update(elapsedMilliseconds: number) {
        this.HandleInput(elapsedMilliseconds);
        this.HandleSlidingBlocks(elapsedMilliseconds);
        this.DetectMatches();
        this.ProcessMatchedBlocks();
        this.DetectChains();
        this.DetectChainStop();
        this.ApplyGravity(elapsedMilliseconds);
        this.RaiseBoard(elapsedMilliseconds);
        this.UpdateBlocks(elapsedMilliseconds);
        this.Draw(elapsedMilliseconds);
    }
}