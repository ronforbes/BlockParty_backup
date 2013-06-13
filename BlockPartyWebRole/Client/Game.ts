/// <reference path="Mouse.ts" />
/// <reference path="Block.ts" />
/// <reference path="Graphics.ts" />

class Game {
    private board: Block[][];
    private boardRows: number = 10;
    private boardColumns: number = 10;
    private boardStartingRow = 0;
    private blockWidth: number = 10;
    private blockHeight: number = 10;
    private blockFallDelayDuration: number = 50;
    private blockFallDuration: number = 100;
    private blockFlashingDuration: number = 1000;
    private blockPopDelayInterval: number = 250;
    private blockPoppingDuration: number = 250;
    private blockEmptyDelayInterval: number = 250;
    private previousMouseState: MouseState;
    private selectedBlockRow: number = -1;
    private selectedBlockColumn: number = -1;

    constructor() {
        // Initialize the board
        this.board = new Block[][];
        for (var row = 0; row < this.boardRows; row++) {
            this.board[row] = new Block[];
            for (var column = 0; column < this.boardColumns; column++) {
                this.board[row][column] = new Block();
                if (row < this.boardStartingRow) {
                    this.board[row][column].State = BlockState.Empty;
                }
                else {
                    this.board[row][column].State = BlockState.Idle;
                    this.board[row][column].Type = Math.floor(Math.random() * 6);
                }
            }
        }

        this.previousMouseState = Mouse.GetState();
    }

    private ProcessInput() {
        var mouseState: MouseState = Mouse.GetState();
        var row = Math.floor(mouseState.Y / this.blockHeight);
        var column = Math.floor(mouseState.X / this.blockWidth);

        if (row >= 0 && row < this.boardRows && column >= 0 && column < this.boardColumns) {
            // Select the block that the mouse is hovering over
            if (mouseState.LeftButton == true && this.previousMouseState.LeftButton == false) {
                if (this.board[row][column].State == BlockState.Idle) {
                    this.selectedBlockRow = row;
                    this.selectedBlockColumn = column;
                }
            }

            if (this.selectedBlockRow != -1 && this.selectedBlockColumn != -1) {
                // Swap blocks if the mouse has moved to a different column
                if (column < this.selectedBlockColumn &&
                    (this.board[this.selectedBlockRow][this.selectedBlockColumn - 1].State == BlockState.Idle ||
                    this.board[this.selectedBlockRow][this.selectedBlockColumn - 1].State == BlockState.Empty)) {
                    // Swap the selected block with the one to its left
                    var adjacentBlock: Block = this.board[this.selectedBlockRow][this.selectedBlockColumn - 1];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn - 1] = this.board[this.selectedBlockRow][this.selectedBlockColumn];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn] = adjacentBlock;

                    this.selectedBlockColumn--;
                }

                if (column > this.selectedBlockColumn &&
                    (this.board[this.selectedBlockRow][this.selectedBlockColumn + 1].State == BlockState.Idle ||
                    this.board[this.selectedBlockRow][this.selectedBlockColumn + 1].State == BlockState.Empty)) {
                    // Swap the selected block with the one to its right
                    var adjacentBlock: Block = this.board[this.selectedBlockRow][this.selectedBlockColumn + 1];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn + 1] = this.board[this.selectedBlockRow][this.selectedBlockColumn];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn] = adjacentBlock;

                    this.selectedBlockColumn++;
                }
            }
        }

        // Deselect the block if the mouse button is no longer being held
        if (mouseState.LeftButton == false/* && this.previousMouseState.LeftButton == true*/) {
            this.selectedBlockRow = -1;
            this.selectedBlockColumn = -1;
        }

        this.previousMouseState = mouseState;
    }

    private DetectMatches() {
        // Look for horizontal matches
        for (var row = 0; row < this.boardRows; row++) {
            var horizontallyMatchingBlocks = 0;
            for (var column = 0; column < this.boardColumns; column++) {
                // Skip empty cells
                if (this.board[row][column].State != BlockState.Idle) {
                    continue;
                }

                // Search to the right for matching blocks
                if (column + 1 < this.boardColumns &&
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
        for (var column = 0; column < this.boardColumns; column++) {
            var verticallyMatchingBlocks = 0;
            for (var row = 0; row < this.boardRows; row++) {
                // Skip empty cells
                if (this.board[row][column].State != BlockState.Idle) {
                    continue;
                }

                // Search below for matching blocks
                if (row + 1 < this.boardRows &&
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

        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                if (this.board[row][column].State == BlockState.Matched) {
                    matchedBlocks++;
                }
            }
        }

        var delayCounter = matchedBlocks;

        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                if (this.board[row][column].State == BlockState.Matched) {
                    this.board[row][column].State = BlockState.Flashing;
                    this.board[row][column].PopDelayDuration = (matchedBlocks - delayCounter) * this.blockPopDelayInterval;
                    this.board[row][column].EmptyDelayDuration = delayCounter * this.blockEmptyDelayInterval;
                    delayCounter--;
                }
            }
        }
    }

    private ProcessFlashingBlocks(elapsedMilliseconds: number) {
        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                if (this.board[row][column].State == BlockState.Flashing) {
                    this.board[row][column].FlashingTimer += elapsedMilliseconds;
                    
                    // Once the block has been in the flashing state for its full duration, start waiting to pop
                    if (this.board[row][column].FlashingTimer >= this.blockFlashingDuration) {
                        this.board[row][column].FlashingTimer = 0;
                        this.board[row][column].State = BlockState.WaitingToPop;
                    }
                }
            }
        }
    }

    private ProcessWaitingToPopBlocks(elapsedMilliseconds: number) {
        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                if (this.board[row][column].State == BlockState.WaitingToPop) {
                    this.board[row][column].PopDelayTimer += elapsedMilliseconds;

                    // Once the block's popping has been delayed for its full duration, start popping
                    if (this.board[row][column].PopDelayTimer >= this.board[row][column].PopDelayDuration) {
                        this.board[row][column].PopDelayTimer = 0;
                        this.board[row][column].PopDelayDuration = 0;
                        this.board[row][column].State = BlockState.Popping;
                    }
                }    
            }
        }
    }

    private ProcessPoppingBlocks(elapsedMilliseconds: number) {
        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                if (this.board[row][column].State == BlockState.Popping) {
                    this.board[row][column].PopTimer += elapsedMilliseconds;

                    // Once the block has popped for the full duration, start waiting to empty
                    if (this.board[row][column].PopTimer >= this.blockPoppingDuration) {
                        this.board[row][column].PopTimer = 0;
                        this.board[row][column].State = BlockState.WaitingToEmpty;
                    }
                }
            }
        }
    }

    private ProcessWaitingToEmptyBlocks(elapsedMilliseconds: number) {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.WaitingToEmpty) {
                    this.board[row][column].EmptyDelayTimer += elapsedMilliseconds;

                    // Once the block's emptying has been delayed for the full duration, empty it
                    if (this.board[row][column].EmptyDelayTimer >= this.board[row][column].EmptyDelayDuration) {
                        this.board[row][column].EmptyDelayTimer = 0;
                        this.board[row][column].EmptyDelayDuration = 0;
                        this.board[row][column].State = BlockState.Empty;
                    }
                }
            }
        }
    }

    private ProcessGravity(elapsedMilliseconds: number) {
        // Mark blocks above empty cells as waiting to fall
        for (var column = 0; column < this.boardColumns; column++) {
            var emptyBlockDetected: bool = false;

            for (var row = this.boardRows - 1; row >= 0; row--) {
                if (this.board[row][column].State == BlockState.Empty) {
                    emptyBlockDetected = true;
                }

                if (this.board[row][column].State == BlockState.Idle && emptyBlockDetected) {
                    this.board[row][column].State = BlockState.WaitingToFall;
                }
            }
        }
    }

    private ProcessWaitingToFallBlocks(elapsedMilliseconds: number) {
        // Wait for the duration of the fall delay then start falling
        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                if (this.board[row][column].State == BlockState.WaitingToFall) {
                    this.board[row][column].FallDelayTimer += elapsedMilliseconds;
                    if (this.board[row][column].FallDelayTimer >= this.blockFallDelayDuration) {
                        this.board[row][column].FallDelayTimer = 0;
                        this.board[row][column].State = BlockState.Falling;
                    }
                }
            }
        }
    }

    private ProcessFallingBlocks(elapsedMilliseconds: number) {
        // Drop falling blocks to the next row
        for (var column = 0; column < this.boardColumns; column++) {
            for (var row = this.boardRows - 1; row >= 0; row--) {
                if (this.board[row][column].State == BlockState.Falling) {
                    if (row + 1 < this.boardRows && (this.board[row + 1][column].State == BlockState.Empty || this.board[row + 1][column].State == BlockState.Falling)) {
                        if (this.board[row][column].FallTimer >= this.blockFallDuration) {
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

    private DrawBoard() {
        // Draw the board
        for (var row = 0; row < this.boardRows; row++) {
            for (var column = 0; column < this.boardColumns; column++) {
                var color: string;
                var text: string;

                switch (this.board[row][column].Type) {
                    case BlockType.A: color = "red"; break;
                    case BlockType.B: color = "green"; break;
                    case BlockType.C: color = "blue"; break;
                    case BlockType.D: color = "cyan"; break;
                    case BlockType.E: color = "magenta"; break;
                    case BlockType.F: color = "yellow"; break;
                }

                switch (this.board[row][column].State) {
                    case BlockState.Empty: color = "black"; text = "E"; break;
                    case BlockState.Idle: text = "I"; break;
                    case BlockState.WaitingToFall: text = "WF"; break;
                    case BlockState.Falling: text = "Fall"; break;
                    case BlockState.Matched: text = "M"; break;
                    case BlockState.Flashing: text = "Flash"; break;
                    case BlockState.WaitingToPop: text = "WP"; break;
                    case BlockState.Popping: text = "P"; break;
                    case BlockState.WaitingToEmpty: color = "black"; text = "WE"; break;
                }

                Graphics.DrawRectangle(new Vector2(column * this.blockWidth, row * this.blockHeight), this.blockWidth, this.blockHeight, 5, "black", color);
                Graphics.DrawText(text, new Vector2(column * this.blockWidth + this.blockWidth / 2, row * this.blockHeight + this.blockHeight / 2), "white");
            }
        }
    }

    public Update(elapsedMilliseconds: number): void {
        Graphics.Clear();

        this.ProcessInput();
        this.ProcessGravity(elapsedMilliseconds);
        this.ProcessWaitingToFallBlocks(elapsedMilliseconds);
        this.ProcessFallingBlocks(elapsedMilliseconds);
        this.DetectMatches();
        this.ProcessMatchedBlocks();
        this.ProcessFlashingBlocks(elapsedMilliseconds);
        this.ProcessWaitingToPopBlocks(elapsedMilliseconds);
        this.ProcessPoppingBlocks(elapsedMilliseconds);
        this.ProcessWaitingToEmptyBlocks(elapsedMilliseconds);
        this.DrawBoard();

        Graphics.Draw();
    }
}