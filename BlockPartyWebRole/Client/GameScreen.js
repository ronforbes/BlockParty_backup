/// <reference path="ScreenManager.ts" />
var ScreenState;
(function (ScreenState) {
    ScreenState[ScreenState["TransitioningOn"] = 0] = "TransitioningOn";
    ScreenState[ScreenState["Active"] = 1] = "Active";
    ScreenState[ScreenState["TransitioningOff"] = 2] = "TransitioningOff";
})(ScreenState || (ScreenState = {}));

var GameScreen = (function () {
    function GameScreen(screenManager) {
        this.TransitionTimeElapsed = TimeSpan.Zero;
        this.TransitionDuration = TimeSpan.Zero;
        this.ScreenManager = screenManager;
    }
    GameScreen.prototype.LoadContent = function () {
    };

    GameScreen.prototype.UnloadContent = function () {
    };

    GameScreen.prototype.TransitionOff = function () {
        this.State = ScreenState.TransitioningOff;
        this.TransitionTimeElapsed = TimeSpan.Zero;
    };

    GameScreen.prototype.Update = function (gameTime) {
        if (this.State == ScreenState.TransitioningOn || this.State == ScreenState.TransitioningOff) {
            this.UpdateTransition(gameTime);
        }
    };

    GameScreen.prototype.UpdateTransition = function (gameTime) {
        this.TransitionTimeElapsed = new TimeSpan(this.TransitionTimeElapsed.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);

        if (this.State == ScreenState.TransitioningOn) {
            if (this.TransitionTimeElapsed.Milliseconds >= this.TransitionDuration.Milliseconds) {
                this.State = ScreenState.Active;
            }
        }

        if (this.State == ScreenState.TransitioningOff) {
            if (this.TransitionTimeElapsed.Milliseconds >= this.TransitionDuration.Milliseconds) {
                this.ScreenManager.RemoveScreen(this);
            }
        }
    };

    GameScreen.prototype.HandleInput = function (gameTime) {
    };

    GameScreen.prototype.Draw = function (gameTime) {
        if (this.State == ScreenState.TransitioningOn || this.State == ScreenState.TransitioningOff) {
            this.DrawTransition();
        }
    };

    GameScreen.prototype.DrawTransition = function () {
    };
    return GameScreen;
})();
