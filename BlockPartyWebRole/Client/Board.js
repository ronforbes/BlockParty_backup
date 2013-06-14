var Board = (function () {
    function Board() {
        this.slideInProgress = false;
        this.slideDirection = 0;
        this.slideTimer = 0;
        this.slideDuration = 50;
        this.selectedRow = -1;
        this.selectedColumn = -1;
        this.ROWS = 10;
        this.COLUMNS = 10;
        this.STARTING_ROW = 0;
        this.board = new Array();
        for(var row = 0; row < this.ROWS; row++) {
            this.board[row] = new Array();
            for(var column = 0; column < this.COLUMNS; column++) {
                this.board[row][column] = new Block();
                if(row < this.STARTING_ROW) {
                    this.board[row][column].State = BlockState.Empty;
                } else {
                    this.board[row][column].State = BlockState.Idle;
                    this.board[row][column].Type = Math.floor(Math.random() * Block.TYPE_COUNT);
                }
            }
        }
        this.previousMouseState = Mouse.GetState();
    }
    Board.prototype.HandleInput = function () {
        var mouseState = Mouse.GetState();
        var row = Math.floor(mouseState.Y / Block.HEIGHT);
        var column = Math.floor(mouseState.X / Block.WIDTH);
        if(row >= 0 && row < this.ROWS && column >= 0 && column < this.COLUMNS) {
            if(mouseState.LeftButton == true && this.previousMouseState.LeftButton == false) {
                if(this.board[row][column].State == BlockState.Idle) {
                    this.selectedRow = row;
                    this.selectedColumn = column;
                }
            }
            if(this.selectedRow != -1 && this.selectedColumn != -1) {
                if(column < this.selectedColumn && this.board[this.selectedRow][this.selectedColumn].State == BlockState.Idle && (this.board[this.selectedRow][this.selectedColumn - 1].State == BlockState.Idle || this.board[this.selectedRow][this.selectedColumn - 1].State == BlockState.Empty)) {
                    this.slideInProgress = true;
                    this.slideDirection = -1;
                }
                if(column > this.selectedColumn && this.board[this.selectedRow][this.selectedColumn].State == BlockState.Idle && (this.board[this.selectedRow][this.selectedColumn + 1].State == BlockState.Idle || this.board[this.selectedRow][this.selectedColumn + 1].State == BlockState.Empty)) {
                    this.slideInProgress = true;
                    this.slideDirection = 1;
                }
            }
        }
        if(mouseState.LeftButton == false) {
            this.selectedRow = -1;
            this.selectedColumn = -1;
        }
        this.previousMouseState = mouseState;
    };
    Board.prototype.HandleSlidingBlocks = function (elapsedMilliseconds) {
        if(this.slideInProgress == true) {
            this.slideTimer += elapsedMilliseconds;
            if(this.slideTimer >= this.slideDuration) {
                this.slideTimer = 0;
                var adjacentBlock = this.board[this.selectedRow][this.selectedColumn + this.slideDirection];
                this.board[this.selectedRow][this.selectedColumn + this.slideDirection] = this.board[this.selectedRow][this.selectedColumn];
                this.board[this.selectedRow][this.selectedColumn] = adjacentBlock;
                this.selectedColumn += this.slideDirection;
                this.slideInProgress = false;
                this.slideDirection = 0;
            }
        }
    };
    Board.prototype.DetectMatches = function () {
        for(var row = 0; row < this.ROWS; row++) {
            var horizontallyMatchingBlocks = 0;
            for(var column = 0; column < this.COLUMNS; column++) {
                if(this.board[row][column].State != BlockState.Idle) {
                    continue;
                }
                if(column + 1 < this.COLUMNS && this.board[row][column + 1].State == BlockState.Idle && this.board[row][column].Type == this.board[row][column + 1].Type) {
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
        for(var column = 0; column < this.COLUMNS; column++) {
            var verticallyMatchingBlocks = 0;
            for(var row = 0; row < this.ROWS; row++) {
                if(this.board[row][column].State != BlockState.Idle) {
                    continue;
                }
                if(row + 1 < this.ROWS && this.board[row + 1][column].State == BlockState.Idle && this.board[row][column].Type == this.board[row + 1][column].Type) {
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
    Board.prototype.ProcessMatchedBlocks = function () {
        var matchedBlocks = 0;
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                if(this.board[row][column].State == BlockState.Matched) {
                    matchedBlocks++;
                }
            }
        }
        var delayCounter = matchedBlocks;
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                if(this.board[row][column].State == BlockState.Matched) {
                    this.board[row][column].State = BlockState.Flashing;
                    this.board[row][column].PopDelayDuration = (matchedBlocks - delayCounter) * Block.POP_DELAY_INTERVAL;
                    this.board[row][column].EmptyDelayDuration = delayCounter * Block.EMPTY_DELAY_INTERVAL;
                    delayCounter--;
                }
            }
        }
    };
    Board.prototype.ApplyGravity = function (elapsedMilliseconds) {
        for(var column = 0; column < this.COLUMNS; column++) {
            var emptyBlockDetected = false;
            for(var row = this.ROWS - 1; row >= 0; row--) {
                if(this.board[row][column].State == BlockState.Empty) {
                    emptyBlockDetected = true;
                }
                if(this.board[row][column].State == BlockState.Idle && emptyBlockDetected) {
                    this.board[row][column].State = BlockState.WaitingToFall;
                }
                if(this.board[row][column].State == BlockState.Falling) {
                    if(row + 1 < this.ROWS && (this.board[row + 1][column].State == BlockState.Empty || this.board[row + 1][column].State == BlockState.Falling)) {
                        if(this.board[row][column].FallTimer >= Block.FALL_DURATION) {
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
    Board.prototype.UpdateBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                this.board[row][column].Update(elapsedMilliseconds);
            }
        }
    };
    Board.prototype.Draw = function (elapsedMilliseconds) {
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                var color;
                var text;
                var x = 0;
                var y = 0;
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
                        color = "#000000";
                        text = "E";
                        continue;
                        break;
                    case BlockState.Idle:
                        text = "I";
                        break;
                    case BlockState.WaitingToFall:
                        text = "WF";
                        break;
                    case BlockState.Falling:
                        text = "Fall";
                        y = this.board[row][column].FallTimer * Block.HEIGHT / Block.FALL_DURATION;
                        break;
                    case BlockState.Matched:
                        text = "M";
                        break;
                    case BlockState.Flashing:
                        if(Date.now() % 100 > 50) {
                            color = "white";
                        }
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
                if(this.slideInProgress == true && row == this.selectedRow) {
                    if(column == this.selectedColumn) {
                        x = this.slideTimer * Block.WIDTH / this.slideDuration * this.slideDirection;
                    }
                    if(column == this.selectedColumn + this.slideDirection) {
                        x = -1 * this.slideTimer * Block.WIDTH / this.slideDuration * this.slideDirection;
                    }
                }
                Graphics.DrawRectangle(new Vector2(column * Block.WIDTH + x, row * Block.HEIGHT + y), Block.WIDTH, Block.HEIGHT, 1, "black", color);
            }
        }
    };
    Board.prototype.Update = function (elapsedMilliseconds) {
        this.HandleInput();
        this.HandleSlidingBlocks(elapsedMilliseconds);
        this.DetectMatches();
        this.ProcessMatchedBlocks();
        this.ApplyGravity(elapsedMilliseconds);
        this.UpdateBlocks(elapsedMilliseconds);
        this.Draw(elapsedMilliseconds);
    };
    return Board;
})();
