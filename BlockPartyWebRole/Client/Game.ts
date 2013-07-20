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
        
        /*var mouseState = Mouse.GetState();
        
        if (mouseState.LeftButton) {
            Graphics.DrawRectangle(new Vector2(mouseState.X - 7.5, mouseState.Y - 7.5), 15, 15, 0, "black", "red");
        }*/
        
        Graphics.Draw();
    }
}