var Board = (function () {
    function Board() {
        this.Score = 0;
        this.slideInProgress = false;
        this.slideDirection = 0;
        this.slideTimer = 0;
        this.slideDuration = 50;
        this.chainCount = 1;
        this.raiseStack = false;
        this.riseTimer = 0;
        this.riseDuration = 10000;
        this.riseForcedRate = 15;
        this.loseTimer = 0;
        this.loseDuration = 10000;
        this.lost = false;
        this.selectedRow = -1;
        this.selectedColumn = -1;
        this.ROWS = 10;
        this.COLUMNS = 6;
        this.STARTING_ROW = 5;
        this.SCORE_FORCED_RISE = 1;
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
        this.newRow = new Array();
        for(var column = 0; column < this.COLUMNS; column++) {
            this.newRow[column] = new Block();
            this.newRow[column].State = BlockState.Idle;
            this.newRow[column].Type = Math.floor(Math.random() * Block.TYPE_COUNT);
        }
        this.previousMouseState = Mouse.GetState();
    }
    Board.prototype.HandleInput = function (elapsedMilliseconds) {
        var mouseState = Mouse.GetState();
        var row = Math.floor(((BlockRenderer.Height * this.riseTimer / this.riseDuration) + mouseState.Y) / BlockRenderer.Height);
        var column = Math.floor(mouseState.X / BlockRenderer.Width);
        if(row >= 0 && row < this.ROWS && column >= 0 && column < this.COLUMNS) {
            if(mouseState.LeftButton == true && this.previousMouseState.LeftButton == false) {
                var delta = Date.now() - this.previousClickTime;
                if(delta <= Mouse.DoubleClickDelay) {
                    if(Math.sqrt(Math.pow(Math.abs(mouseState.X - this.previousMouseState.X), 2) + Math.pow(Math.abs(mouseState.Y - this.previousMouseState.Y), 2)) < 10) {
                        this.raiseStack = true;
                    }
                } else {
                    this.raiseStack = false;
                }
                this.previousClickTime = Date.now();
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
        if(this.raiseStack) {
            this.riseTimer += elapsedMilliseconds * this.riseForcedRate;
            this.Score += this.SCORE_FORCED_RISE;
            if(this.riseTimer >= this.riseDuration && !mouseState.LeftButton) {
                this.raiseStack = false;
            }
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
        var incrementChain = false;
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
                            if(this.board[row][column - matchingColumn].EligibleForChain == true) {
                                incrementChain = true;
                            }
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
                            if(this.board[row - matchingRow][column].EligibleForChain == true) {
                                incrementChain = true;
                            }
                        }
                    }
                    verticallyMatchingBlocks = 0;
                }
            }
        }
        if(incrementChain == true) {
            this.chainCount++;
        }
    };
    Board.prototype.ProcessMatchedBlocks = function () {
        var matchedBlocks = 0;
        var incrementChain = false;
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                if(this.board[row][column].State == BlockState.Matched) {
                    matchedBlocks++;
                }
            }
        }
        var delayCounter = matchedBlocks;
        if(matchedBlocks > 3) {
            this.Score += matchedBlocks * 100;
        }
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
    Board.prototype.DetectChains = function () {
        for(var column = 0; column < this.COLUMNS; column++) {
            for(var row = this.ROWS - 1; row >= 0; row--) {
                if(this.board[row][column].JustBecameEmpty == true) {
                    for(var chainEligibleRow = row - 1; chainEligibleRow >= 0; chainEligibleRow--) {
                        if(this.board[chainEligibleRow][column].State == BlockState.Idle) {
                            this.board[chainEligibleRow][column].EligibleForChain = true;
                        }
                    }
                }
                this.board[row][column].JustBecameEmpty = false;
            }
        }
    };
    Board.prototype.DetectChainStop = function () {
        var stopChain = true;
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                if(this.board[row][column].State != BlockState.Idle && this.board[row][column].State != BlockState.Empty) {
                    stopChain = false;
                }
            }
        }
        if(stopChain == true) {
            for(var row = 0; row < this.ROWS; row++) {
                for(var column = 0; column < this.COLUMNS; column++) {
                    this.board[row][column].EligibleForChain = false;
                }
            }
            this.chainCount = 1;
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
                            this.board[row + 1][column].EligibleForChain = this.board[row][column].EligibleForChain;
                            this.board[row][column].State = BlockState.Empty;
                            this.board[row][column].EligibleForChain = false;
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
    Board.prototype.RaiseBoard = function (elapsedMilliseconds) {
        var raiseBoard = true;
        var losing = false;
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                if(row == 0) {
                    if(this.board[row][column].State == BlockState.Idle) {
                        raiseBoard = false;
                        losing = true;
                    }
                }
                if(this.board[row][column].State != BlockState.Empty && this.board[row][column].State != BlockState.Idle) {
                    raiseBoard = false;
                }
            }
        }
        if(raiseBoard) {
            this.riseTimer += elapsedMilliseconds;
            if(this.riseTimer >= this.riseDuration) {
                this.riseTimer = 0;
                for(var row = 0; row < this.ROWS - 1; row++) {
                    for(var column = 0; column < this.COLUMNS; column++) {
                        this.board[row][column].State = this.board[row + 1][column].State;
                        this.board[row][column].Type = this.board[row + 1][column].Type;
                    }
                }
                for(var column = 0; column < this.COLUMNS; column++) {
                    this.board[this.ROWS - 1][column].State = this.newRow[column].State;
                    this.board[this.ROWS - 1][column].Type = this.newRow[column].Type;
                    this.newRow[column].Type = Math.floor(Math.random() * Block.TYPE_COUNT);
                }
            }
        }
        if(losing) {
            this.loseTimer += elapsedMilliseconds;
            if(this.loseTimer >= this.loseDuration) {
                this.lost = true;
            }
        } else {
            this.loseTimer = 0;
        }
    };
    Board.prototype.UpdateBlocks = function (elapsedMilliseconds) {
        for(var row = 0; row < this.ROWS; row++) {
            for(var column = 0; column < this.COLUMNS; column++) {
                this.board[row][column].Update(elapsedMilliseconds, this);
            }
        }
    };
    Board.prototype.Draw = function (elapsedMilliseconds) {
        Graphics.DrawFullscreenRectangle("#222222");
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
                        break;
                    case BlockState.Idle:
                        break;
                    case BlockState.WaitingToFall:
                        break;
                    case BlockState.Falling:
                        y = this.board[row][column].FallTimer * BlockRenderer.Height / Block.FALL_DURATION;
                        break;
                    case BlockState.Matched:
                        break;
                    case BlockState.Flashing:
                        if(Date.now() % 100 > 50) {
                            color = "white";
                        }
                        break;
                    case BlockState.WaitingToPop:
                        break;
                    case BlockState.Popping:
                        break;
                    case BlockState.WaitingToEmpty:
                        color = "black";
                        break;
                }
                if(this.board[row][column].EligibleForChain == true) {
                    text = "E";
                } else {
                    text = "";
                }
                if(this.slideInProgress == true && row == this.selectedRow) {
                    if(column == this.selectedColumn) {
                        x = this.slideTimer * BlockRenderer.Width / this.slideDuration * this.slideDirection;
                    }
                    if(column == this.selectedColumn + this.slideDirection) {
                        x = -1 * this.slideTimer * BlockRenderer.Width / this.slideDuration * this.slideDirection;
                    }
                }
                y -= BlockRenderer.Height * this.riseTimer / this.riseDuration;
                Graphics.DrawRectangle(new Vector2(column * BlockRenderer.Width + x, row * BlockRenderer.Height + y), BlockRenderer.Width, BlockRenderer.Height, 1, "black", color);
            }
        }
        for(var column = 0; column < this.COLUMNS; column++) {
            var color;
            var text;
            var x = 0;
            var y = 0;
            switch(this.newRow[column].Type) {
                case BlockType.A:
                    color = "#880000";
                    break;
                case BlockType.B:
                    color = "#008800";
                    break;
                case BlockType.C:
                    color = "#000088";
                    break;
                case BlockType.D:
                    color = "#008888";
                    break;
                case BlockType.E:
                    color = "#880088";
                    break;
                case BlockType.F:
                    color = "#888800";
                    break;
            }
        }
        if(this.lost) {
        }
    };
    Board.prototype.Update = function (elapsedMilliseconds) {
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
    };
    return Board;
})();
