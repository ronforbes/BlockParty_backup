/// <reference path="GameScreen.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="GraphicsManager.ts" />
/// <reference path="Viewport.ts" />
/// <reference path="Game.ts" />
var ScreenManager = (function () {
    function ScreenManager(game) {
        this.screens = new Array();
        this.screensToUpdate = new Array();
        this.Game = game;
    }
    ScreenManager.prototype.AddScreen = function (screen) {
        screen.LoadContent();

        this.screens.push(screen);
    };

    ScreenManager.prototype.RemoveScreen = function (screen) {
        screen.UnloadContent();

        var index = this.screens.indexOf(screen);
        this.screens.splice(index);
    };

    ScreenManager.prototype.LoadScreen = function (screen) {
        // Transition out of the current screens
        this.screens.forEach(function (screen, index, screens) {
            screen.TransitionOff();
        });

        this.screenToLoad = screen;
        this.loadingScreen = true;
    };

    ScreenManager.prototype.Update = function (gameTime) {
        // Read input
        this.Game.InputManager.Update();

        // Make a copy of the list of screens to allow for adds/removes not interfering w/ updates
        this.screensToUpdate.splice(0, this.screensToUpdate.length);

        this.screens.forEach(function (screen, index, screens) {
            this.screensToUpdate.push(screen);
        }, this);

        while (this.screensToUpdate.length > 0) {
            // Pop the topmost screen off the stack
            var screen = this.screensToUpdate.pop();

            // Update the screen
            screen.Update(gameTime);
            screen.HandleInput(gameTime);
        }

        if (this.loadingScreen) {
            if (this.screens.length == 0) {
                this.AddScreen(this.screenToLoad);
                this.loadingScreen = false;
            }
        }
    };

    ScreenManager.prototype.Draw = function (gameTime) {
        this.Game.GraphicsManager.Clear();

        // Draw screens
        this.screens.forEach(function (screen, index, screens) {
            screen.Draw(gameTime);
        });
    };
    return ScreenManager;
})();
