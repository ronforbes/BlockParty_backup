var Game = (function () {
    function Game() {
        this.boardRows = 10;
        this.boardColumns = 10;
        this.boardStartingRow = 0;
        this.blockWidth = 10;
        this.blockHeight = 10;
        this.blockFallDelayDuration = 50;
        this.blockFallDuration = 100;
        this.blockFlashingDuration = 1000;
        this.blockPopDelayInterval = 250;
        this.blockPoppingDuration = 250;
        this.blockEmptyDelayInterval = 250;
        this.selectedBlockRow = -1;
        this.selectedBlockColumn = -1;
        this.board = new Array();
        for(var row = 0; row < this.boardRows; row++) {
            this.board[row] = new Array();
            for(var column = 0; column < this.boardColumns; column++) {
                this.board[row][column] = new Block();
                if(row < this.boardStartingRow) {
                    this.board[row][column].State = BlockState.Empty;
                } else {
                    this.board[row][column].State = BlockState.Idle;
                    this.board[row][column].Type = Math.floor(Math.random() * 6);
                }
            }
        }
        this.previousMouseState = Mouse.GetState();
    }
    Game.prototype.ProcessInput = function () {
        var mouseState = Mouse.GetState();
        var row = Math.floor(mouseState.Y / this.blockHeight);
        var column = Math.floor(mouseState.X / this.blockWidth);
        if(row >= 0 && row < this.boardRows && column >= 0 && column < this.boardColumns) {
            if(mouseState.LeftButton == true && this.previousMouseState.LeftButton == false) {
                if(this.board[row][column].State == BlockState.Idle) {
                    this.selectedBlockRow = row;
                    this.selectedBlockColumn = column;
                }
            }
            if(this.selectedBlockRow != -1 && this.selectedBlockColumn != -1) {
                if(column < this.selectedBlockColumn && (this.board[this.selectedBlockRow][this.selectedBlockColumn - 1].State == BlockState.Idle || this.board[this.selectedBlockRow][this.selectedBlockColumn - 1].State == BlockState.Empty)) {
                    var adjacentBlock = this.board[this.selectedBlockRow][this.selectedBlockColumn - 1];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn - 1] = this.board[this.selectedBlockRow][this.selectedBlockColumn];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn] = adjacentBlock;
                    this.selectedBlockColumn--;
                }
                if(column > this.selectedBlockColumn && (this.board[this.selectedBlockRow][this.selectedBlockColumn + 1].State == BlockState.Idle || this.board[this.selectedBlockRow][this.selectedBlockColumn + 1].State == BlockState.Empty)) {
                    var adjacentBlock = this.board[this.selectedBlockRow][this.selectedBlockColumn + 1];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn + 1] = this.board[this.selectedBlockRow][this.selectedBlockColumn];
                    this.board[this.selectedBlockRow][this.selectedBlockColumn] = adjacentBlock;
                    this.selectedBlockColumn++;
                }
            }
        }
        if(mouseState.LeftButton == false) {
            this.selectedBlockRow = -1;
            this.selectedBlockColumn = -1;
        }
        this.previousMouseState = mouseState;
    };
    Game.prototype.DetectMatches = function () {
        for(var row = 0; row < this.boardRows; row++) {
            var horizontallyMatchingBlocks = 0;
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State != BlockState.Idle) {
                    continue;
                }
                if(column + 1 < this.boardColumns && this.board[row][column + 1].State == BlockState.Idle && this.board[row][column].Type == this.board[row][column + 1].Type) {
                    horizontallyMatchingBlocks++;
                } else {
                    if(horizontallyMatchingBlocks >= 2) {
                        for(var matchingColumn = horizontallyMatchingBlocks; matchingColumn >= 0; matchingColumn--) {
                            this.board[row][column - matchingColumn].State = BlockState.Matched;
                        }
                    }
                    horizontallyMatchingBlocks = 0;
                }
            }
        }
        for(var column = 0; column < this.boardColumns; column++) {
            var verticallyMatchingBlocks = 0;
            for(var row = 0; row < this.boardRows; row++) {
                if(this.board[row][column].State != BlockState.Idle) {
                    continue;
                }
                if(row + 1 < this.boardRows && this.board[row + 1][column].State == BlockState.Idle && this.board[row][column].Type == this.board[row + 1][column].Type) {
                    verticallyMatchingBlocks++;
                } else {
                    if(verticallyMatchingBlocks >= 2) {
                        for(var matchingRow = verticallyMatchingBlocks; matchingRow >= 0; matchingRow--) {
                            this.board[row - matchingRow][column].State = BlockState.Matched;
                        }
                    }
                    verticallyMatchingBlocks = 0;
                }
            }
        }
    };
    Game.prototype.ProcessMatchedBlocks = function () {
        var matchedBlocks = 0;
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.Matched) {
                    matchedBlocks++;
                }
            }
        }
        var delayCounter = matchedBlocks;
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.Matched) {
                    this.board[row][column].State = BlockState.Flashing;
                    this.board[row][column].PopDelayDuration = (matchedBlocks - delayCounter) * this.blockPopDelayInterval;
                    this.board[row][column].EmptyDelayDuration = delayCounter * this.blockEmptyDelayInterval;
                    delayCounter--;
                }
            }
        }
    };
    Game.prototype.ProcessFlashingBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.Flashing) {
                    this.board[row][column].FlashingTimer += elapsedMilliseconds;
                    if(this.board[row][column].FlashingTimer >= this.blockFlashingDuration) {
                        this.board[row][column].FlashingTimer = 0;
                        this.board[row][column].State = BlockState.WaitingToPop;
                    }
                }
            }
        }
    };
    Game.prototype.ProcessWaitingToPopBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.WaitingToPop) {
                    this.board[row][column].PopDelayTimer += elapsedMilliseconds;
                    if(this.board[row][column].PopDelayTimer >= this.board[row][column].PopDelayDuration) {
                        this.board[row][column].PopDelayTimer = 0;
                        this.board[row][column].PopDelayDuration = 0;
                        this.board[row][column].State = BlockState.Popping;
                    }
                }
            }
        }
    };
    Game.prototype.ProcessPoppingBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.Popping) {
                    this.board[row][column].PopTimer += elapsedMilliseconds;
                    if(this.board[row][column].PopTimer >= this.blockPoppingDuration) {
                        this.board[row][column].PopTimer = 0;
                        this.board[row][column].State = BlockState.WaitingToEmpty;
                    }
                }
            }
        }
    };
    Game.prototype.ProcessWaitingToEmptyBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.WaitingToEmpty) {
                    this.board[row][column].EmptyDelayTimer += elapsedMilliseconds;
                    if(this.board[row][column].EmptyDelayTimer >= this.board[row][column].EmptyDelayDuration) {
                        this.board[row][column].EmptyDelayTimer = 0;
                        this.board[row][column].EmptyDelayDuration = 0;
                        this.board[row][column].State = BlockState.Empty;
                    }
                }
            }
        }
    };
    Game.prototype.ProcessGravity = function (elapsedMilliseconds) {
        for(var column = 0; column < this.boardColumns; column++) {
            var emptyBlockDetected = false;
            for(var row = this.boardRows - 1; row >= 0; row--) {
                if(this.board[row][column].State == BlockState.Empty) {
                    emptyBlockDetected = true;
                }
                if(this.board[row][column].State == BlockState.Idle && emptyBlockDetected) {
                    this.board[row][column].State = BlockState.WaitingToFall;
                }
            }
        }
    };
    Game.prototype.ProcessWaitingToFallBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                if(this.board[row][column].State == BlockState.WaitingToFall) {
                    this.board[row][column].FallDelayTimer += elapsedMilliseconds;
                    if(this.board[row][column].FallDelayTimer >= this.blockFallDelayDuration) {
                        this.board[row][column].FallDelayTimer = 0;
                        this.board[row][column].State = BlockState.Falling;
                    }
                }
            }
        }
    };
    Game.prototype.ProcessFallingBlocks = function (elapsedMilliseconds) {
        for(var column = 0; column < this.boardColumns; column++) {
            for(var row = this.boardRows - 1; row >= 0; row--) {
                if(this.board[row][column].State == BlockState.Falling) {
                    if(row + 1 < this.boardRows && (this.board[row + 1][column].State == BlockState.Empty || this.board[row + 1][column].State == BlockState.Falling)) {
                        if(this.board[row][column].FallTimer >= this.blockFallDuration) {
                            this.board[row][column].FallTimer = 0;
                            this.board[row + 1][column].Type = this.board[row][column].Type;
                            this.board[row + 1][column].State = BlockState.Falling;
                            this.board[row][column].State = BlockState.Empty;
                        } else {
                            this.board[row][column].FallTimer += elapsedMilliseconds;
                        }
                    } else {
                        this.board[row][column].State = BlockState.Idle;
                    }
                }
            }
        }
    };
    Game.prototype.DrawBoard = function () {
        for(var row = 0; row < this.boardRows; row++) {
            for(var column = 0; column < this.boardColumns; column++) {
                var color;
                var text;
                switch(this.board[row][column].Type) {
                    case BlockType.A:
                        color = "red";
                        break;
                    case BlockType.B:
                        color = "green";
                        break;
                    case BlockType.C:
                        color = "blue";
                        break;
                    case BlockType.D:
                        color = "cyan";
                        break;
                    case BlockType.E:
                        color = "magenta";
                        break;
                    case BlockType.F:
                        color = "yellow";
                        break;
                }
                switch(this.board[row][column].State) {
                    case BlockState.Empty:
                        color = "black";
                        text = "E";
                        break;
                    case BlockState.Idle:
                        text = "I";
                        break;
                    case BlockState.WaitingToFall:
                        text = "WF";
                        break;
                    case BlockState.Falling:
                        text = "Fall";
                        break;
                    case BlockState.Matched:
                        text = "M";
                        break;
                    case BlockState.Flashing:
                        text = "Flash";
                        break;
                    case BlockState.WaitingToPop:
                        text = "WP";
                        break;
                    case BlockState.Popping:
                        text = "P";
                        break;
                    case BlockState.WaitingToEmpty:
                        color = "black";
                        text = "WE";
                        break;
                }
                Graphics.DrawRectangle(new Vector2(column * this.blockWidth, row * this.blockHeight), this.blockWidth, this.blockHeight, 5, "black", color);
                Graphics.DrawText(text, new Vector2(column * this.blockWidth + this.blockWidth / 2, row * this.blockHeight + this.blockHeight / 2), "white");
            }
        }
    };
    Game.prototype.Update = function (elapsedMilliseconds) {
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
    };
    return Game;
})();
