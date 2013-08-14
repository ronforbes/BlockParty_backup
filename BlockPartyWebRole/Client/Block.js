var BlockState;
(function (BlockState) {
    BlockState._map = [];
    BlockState._map[0] = "Empty";
    BlockState.Empty = 0;
    BlockState._map[1] = "Idle";
    BlockState.Idle = 1;
    BlockState._map[2] = "SlidingLeft";
    BlockState.SlidingLeft = 2;
    BlockState._map[3] = "SlidingRight";
    BlockState.SlidingRight = 3;
    BlockState._map[4] = "WaitingToFall";
    BlockState.WaitingToFall = 4;
    BlockState._map[5] = "Falling";
    BlockState.Falling = 5;
    BlockState._map[6] = "Matched";
    BlockState.Matched = 6;
    BlockState._map[7] = "Flashing";
    BlockState.Flashing = 7;
    BlockState._map[8] = "WaitingToPop";
    BlockState.WaitingToPop = 8;
    BlockState._map[9] = "Popping";
    BlockState.Popping = 9;
    BlockState._map[10] = "WaitingToEmpty";
    BlockState.WaitingToEmpty = 10;
})(BlockState || (BlockState = {}));
var BlockType;
(function (BlockType) {
    BlockType._map = [];
    BlockType._map[0] = "A";
    BlockType.A = 0;
    BlockType._map[1] = "B";
    BlockType.B = 1;
    BlockType._map[2] = "C";
    BlockType.C = 2;
    BlockType._map[3] = "D";
    BlockType.D = 3;
    BlockType._map[4] = "E";
    BlockType.E = 4;
    BlockType._map[5] = "F";
    BlockType.F = 5;
})(BlockType || (BlockType = {}));
var Block = (function () {
    function Block() {
        this.FallTimer = 0;
        this.PopDelayDuration = 0;
        this.EmptyDelayDuration = 0;
        this.JustBecameEmpty = false;
        this.EligibleForChain = false;
        this.FALL_DELAY_DURATION = 250;
        this.FLASH_DURATION = 1000;
        this.POP_DURATION = 250;
        this.POP_SCORE = 10;
        this.fallDelayTimer = 0;
        this.flashTimer = 0;
        this.popDelayTimer = 0;
        this.popTimer = 0;
        this.emptyDelayTimer = 0;
    }
    Block.TYPE_COUNT = 6;
    BlockRenderer.Width = 15;
    BlockRenderer.Height = 15;
    Block.FALL_DURATION = 50;
    Block.POP_DELAY_INTERVAL = 250;
    Block.EMPTY_DELAY_INTERVAL = 250;
    Block.prototype.Update = function (elapsedMilliseconds, board) {
        switch(this.State) {
            case BlockState.WaitingToFall:
                this.fallDelayTimer += elapsedMilliseconds;
                if(this.fallDelayTimer >= this.FALL_DELAY_DURATION) {
                    this.fallDelayTimer = 0;
                    this.State = BlockState.Falling;
                }
                break;
            case BlockState.Flashing:
                this.flashTimer += elapsedMilliseconds;
                if(this.flashTimer >= this.FLASH_DURATION) {
                    this.flashTimer = 0;
                    this.State = BlockState.WaitingToPop;
                }
                break;
            case BlockState.WaitingToPop:
                this.popDelayTimer += elapsedMilliseconds;
                if(this.popDelayTimer >= this.PopDelayDuration) {
                    this.popDelayTimer = 0;
                    this.PopDelayDuration = 0;
                    this.State = BlockState.Popping;
                }
                break;
            case BlockState.Popping:
                this.popTimer += elapsedMilliseconds;
                if(this.popTimer >= this.POP_DURATION) {
                    this.popTimer = 0;
                    this.State = BlockState.WaitingToEmpty;
                    board.Score += this.POP_SCORE;
                }
                break;
            case BlockState.WaitingToEmpty:
                this.emptyDelayTimer += elapsedMilliseconds;
                if(this.emptyDelayTimer >= this.EmptyDelayDuration) {
                    this.emptyDelayTimer = 0;
                    this.EmptyDelayDuration = 0;
                    this.State = BlockState.Empty;
                    this.JustBecameEmpty = true;
                }
                break;
        }
    };
    return Block;
})();
