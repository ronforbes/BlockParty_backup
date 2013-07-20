/// <reference path="Board.ts" />

enum BlockState {
    Empty,
    Idle,
    SlidingLeft,
    SlidingRight,
    WaitingToFall,
    Falling,
    Matched,
    Flashing,
    WaitingToPop,
    Popping,
    WaitingToEmpty
}

enum BlockType {
    A,
    B,
    C,
    D,
    E,
    F
}

class Block {
    public static TYPE_COUNT: number = 6;
    public static WIDTH: number = 15;
    public static HEIGHT: number = 15;
    public static FALL_DURATION: number = 50;
    public static POP_DELAY_INTERVAL: number = 250;
    public static EMPTY_DELAY_INTERVAL: number = 250;

    public State: BlockState;
    public Type: BlockType;
    public FallTimer: number = 0;
    public PopDelayDuration: number = 0;
    public EmptyDelayDuration: number = 0;
    public JustBecameEmpty: bool = false;
    public EligibleForChain: bool = false;

    private FALL_DELAY_DURATION: number = 250;
    private FLASH_DURATION: number = 1000;
    private POP_DURATION: number = 250;
    private POP_SCORE: number = 10;

    private fallDelayTimer: number = 0;
    private flashTimer: number = 0;
    private popDelayTimer: number = 0;
    private popTimer: number = 0;
    private emptyDelayTimer: number = 0;    

    public Update(elapsedMilliseconds: number, board: Board) {
        switch (this.State) {
            case BlockState.WaitingToFall:
                this.fallDelayTimer += elapsedMilliseconds;

                // Once the block has waited to fall for its full duration, start falling
                if (this.fallDelayTimer >= this.FALL_DELAY_DURATION) {
                    this.fallDelayTimer = 0;
                    this.State = BlockState.Falling;
                }
                break;

                // Once the block has 
            case BlockState.Flashing:
                this.flashTimer += elapsedMilliseconds;

                // Once the block has flashed for its full duration, start waiting to pop
                if (this.flashTimer >= this.FLASH_DURATION) {
                    this.flashTimer = 0;
                    this.State = BlockState.WaitingToPop;
                }
                break;

            case BlockState.WaitingToPop:
                this.popDelayTimer += elapsedMilliseconds;

                // Once the block's popping has been delayed for its full duration, start popping
                if (this.popDelayTimer >= this.PopDelayDuration) {
                    this.popDelayTimer = 0;
                    this.PopDelayDuration = 0;
                    this.State = BlockState.Popping;
                }
                break;

            case BlockState.Popping:
                this.popTimer += elapsedMilliseconds;

                // Once the block has popped for the full duration, start waiting to empty
                if (this.popTimer >= this.POP_DURATION) {
                    this.popTimer = 0;
                    this.State = BlockState.WaitingToEmpty;
                    board.Score += this.POP_SCORE;
                }
                break;

            case BlockState.WaitingToEmpty:
                this.emptyDelayTimer += elapsedMilliseconds;

                // Once the block's emptying has been delayed for the full duration, empty it
                if (this.emptyDelayTimer >= this.EmptyDelayDuration) {
                    this.emptyDelayTimer = 0;
                    this.EmptyDelayDuration = 0;
                    this.State = BlockState.Empty;
                    this.JustBecameEmpty = true;
                }
                break;
        }
    }
}