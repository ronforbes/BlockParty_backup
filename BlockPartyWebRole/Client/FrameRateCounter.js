/// <reference path="GameTime.ts" />
/// <reference path="TimeSpan.ts" />
/// <reference path="Game.ts" />
var FrameRateCounter = (function () {
    function FrameRateCounter(game) {
        this.frameCounter = 0;
        this.framesPerSecond = 0;
        this.elapsedTime = TimeSpan.Zero;
        this.game = game;
    }
    FrameRateCounter.prototype.Update = function (gameTime) {
        this.elapsedTime = TimeSpan.FromMilliseconds(this.elapsedTime.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);

        if (this.elapsedTime.Seconds >= 1) {
            // Set the frame rate
            this.framesPerSecond = this.frameCounter;

            // Reset the frame counter
            this.frameCounter = 0;

            // Reset the elapsed time
            this.elapsedTime = TimeSpan.Zero;
        }
    };

    FrameRateCounter.prototype.Draw = function (gameTime) {
        this.frameCounter++;

        this.game.GraphicsManager.DrawText("Frames per second: " + this.framesPerSecond.toString(), Vector2.Zero, "white");
    };
    return FrameRateCounter;
})();
