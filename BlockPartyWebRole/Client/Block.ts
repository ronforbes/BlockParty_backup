enum BlockState {
    Empty,
    Idle,
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
    public State: BlockState;
    public Type: BlockType;
    public FallDelayTimer: number = 0;
    public FallTimer: number = 0;
    public FlashingTimer: number = 0;
    public PopDelayDuration: number = 0;
    public PopDelayTimer: number = 0;
    public PopTimer: number = 0;
    public EmptyDelayDuration: number = 0;
    public EmptyDelayTimer: number = 0;
}