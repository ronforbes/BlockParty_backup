/// <reference path="GameTime.ts" />
/// <reference path="TimeSpan.ts" />
/// <reference path="Game.ts" />

class FrameRateCounter {
    private game: Game;
    private frameCounter: number = 0;
    private framesPerSecond: number = 0;
    private elapsedTime: TimeSpan = TimeSpan.Zero;

    constructor(game: Game) {
        this.game = game;
    }

    public Update(gameTime: GameTime) {
        this.elapsedTime = TimeSpan.FromMilliseconds(this.elapsedTime.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);

        if (this.elapsedTime.Seconds >= 1) {
            // Set the frame rate
            this.framesPerSecond = this.frameCounter;

            // Reset the frame counter
            this.frameCounter = 0;

            // Reset the elapsed time
            this.elapsedTime = TimeSpan.Zero;
        }
    }

    public Draw(gameTime: GameTime) {
        this.frameCounter++;

        this.game.GraphicsManager.DrawText("Frames per second: " + this.framesPerSecond.toString(), Vector2.Zero, "white");
    }
}