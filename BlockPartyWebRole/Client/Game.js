/// <reference path="SplashScreen.ts" />
/// <reference path="FrameRateCounter.ts" />
/// <reference path="InputManager.ts" />
/// <reference path="AudioManager.ts" />
/// <reference path="Viewport.ts" />
/// <reference path="GraphicsManager.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="ScreenManager.ts" />
var Game = (function () {
    function Game() {
        this.defaultWorldWidth = 1600;
        this.defaultWorldHeight = 900;
        this.WorldViewport = new Viewport(0, 0, this.defaultWorldWidth, this.defaultWorldHeight);
    }
    Game.prototype.Initialize = function () {
        this.GraphicsManager = new GraphicsManager(this, $("#canvas"));
        this.InputManager = new InputManager(this, $("#canvas"));
        this.AudioManager = new AudioManager(this);
        this.screenManager = new ScreenManager(this);
        this.frameRateCounter = new FrameRateCounter(this);
    };

    Game.prototype.LoadContent = function () {
        this.AudioManager.LoadContent();

        this.screenManager.AddScreen(new SplashScreen(this.screenManager));
    };

    Game.prototype.Update = function (gameTime) {
        this.screenManager.Update(gameTime);
        this.frameRateCounter.Update(gameTime);
    };

    Game.prototype.Draw = function (gameTime) {
        this.screenManager.Draw(gameTime);
        this.frameRateCounter.Draw(gameTime);
        this.GraphicsManager.Present();
    };
    return Game;
})();
