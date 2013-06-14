/// <reference path="Board.ts" />
/// <reference path="Mouse.ts" />
/// <reference path="Block.ts" />
/// <reference path="Graphics.ts" />

class Game {
    private board: Board;

    constructor() {
        // Initialize the board
        this.board = new Board();
    }

    public Update(elapsedMilliseconds: number): void {
        Graphics.Clear();

        this.board.Update(elapsedMilliseconds);

        Graphics.Draw();
    }
}