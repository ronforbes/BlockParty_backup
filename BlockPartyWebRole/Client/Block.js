var BlockState;
(function (BlockState) {
    BlockState._map = [];
    BlockState._map[0] = "Empty";
    BlockState.Empty = 0;
    BlockState._map[1] = "Idle";
    BlockState.Idle = 1;
    BlockState._map[2] = "WaitingToFall";
    BlockState.WaitingToFall = 2;
    BlockState._map[3] = "Falling";
    BlockState.Falling = 3;
    BlockState._map[4] = "Matched";
    BlockState.Matched = 4;
    BlockState._map[5] = "Flashing";
    BlockState.Flashing = 5;
    BlockState._map[6] = "WaitingToPop";
    BlockState.WaitingToPop = 6;
    BlockState._map[7] = "Popping";
    BlockState.Popping = 7;
    BlockState._map[8] = "WaitingToEmpty";
    BlockState.WaitingToEmpty = 8;
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
        this.FallDelayTimer = 0;
        this.FallTimer = 0;
        this.FlashingTimer = 0;
        this.PopDelayDuration = 0;
        this.PopDelayTimer = 0;
        this.PopTimer = 0;
        this.EmptyDelayDuration = 0;
        this.EmptyDelayTimer = 0;
    }
    return Block;
})();
