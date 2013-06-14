/// <reference path="Mouse.ts" />
/// <reference path="Block.ts" />

class Board {
    private board: Block[][];
    private slideInProgress: bool = false;
    private slideDirection: number = 0;
    private slideTimer: number = 0;
    private slideDuration: number = 50;
    private previousMouseState: MouseState;
    private selectedRow: number = -1;
    private selectedColumn: number = -1;

    private ROWS: number = 10;
    private COLUMNS: number = 10;
    private STARTING_ROW: number = 0;

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

        // Initialze mouse state
        this.previousMouseState = Mouse.GetState();
    }

    private HandleInput() {
        // Determine which block is being hovered over
        var mouseState: MouseState = Mouse.GetState();
        var row: number = Math.floor(mouseState.Y / Block.HEIGHT);
        var column: number = Math.floor(mouseState.X / Block.WIDTH);

        // If the mouse is hovering over a valid block, select or slide it
        if (row >= 0 && row < this.ROWS && column >= 0 && column < this.COLUMNS) {
            // If the mouse has been clicked, select the block that the mouse is hovering over
            if (mouseState.LeftButton == true && this.previousMouseState.LeftButton == false) {
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
                        }
                    }

                    // Reset the matching block counter
                    verticallyMatchingBlocks = 0;
                }
            }
        }
    }

    private ProcessMatchedBlocks() {
        var matchedBlocks: number = 0;

        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                if (this.board[row][column].State == BlockState.Matched) {
                    matchedBlocks++;
                }
            }
        }

        var delayCounter = matchedBlocks;

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
                            this.board[row][column].State = BlockState.Empty;
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

    private UpdateBlocks(elapsedMilliseconds: number) {
        for (var row = 0; row < this.ROWS; row++) {
            for (var column = 0; column < this.COLUMNS; column++) {
                this.board[row][column].Update(elapsedMilliseconds);
            }
        }
    }

    private Draw(elapsedMilliseconds: number) {
        // Draw the board
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
                    case BlockState.Empty: color = "#000000"; text = "E"; continue; break;
                    case BlockState.Idle: text = "I"; break;
                    case BlockState.WaitingToFall: text = "WF"; break;
                    case BlockState.Falling: text = "Fall"; y = this.board[row][column].FallTimer * Block.HEIGHT / Block.FALL_DURATION; break;
                    case BlockState.Matched: text = "M"; break;
                    case BlockState.Flashing:
                        if (Date.now() % 100 > 50) {
                            color = "white";
                        }
                        text = "Flash";
                        break;
                    case BlockState.WaitingToPop: text = "WP"; break;
                    case BlockState.Popping: text = "P"; break;
                    case BlockState.WaitingToEmpty: color = "black"; text = "WE"; break;
                }

                if (this.slideInProgress == true &&
                    row == this.selectedRow) {
                    if (column == this.selectedColumn) {
                        x = this.slideTimer * Block.WIDTH / this.slideDuration * this.slideDirection;
                    }
                    if (column == this.selectedColumn + this.slideDirection) {
                        x = -1 * this.slideTimer * Block.WIDTH / this.slideDuration * this.slideDirection;
                    }
                }

                Graphics.DrawRectangle(new Vector2(column * Block.WIDTH + x, row * Block.HEIGHT + y), Block.WIDTH, Block.HEIGHT, 1, "black", color);
                //Graphics.DrawText(text, new Vector2(column * Block.WIDTH + Block.WIDTH / 2 + x, row * Block.HEIGHT + Block.HEIGHT / 2 + y), "white");
            }
        }
    }

    public Update(elapsedMilliseconds: number) {
        this.HandleInput();
        this.HandleSlidingBlocks(elapsedMilliseconds);
        this.DetectMatches();
        this.ProcessMatchedBlocks();
        this.ApplyGravity(elapsedMilliseconds);
        this.UpdateBlocks(elapsedMilliseconds);
        this.Draw(elapsedMilliseconds);
    }
}