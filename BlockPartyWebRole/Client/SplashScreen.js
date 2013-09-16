/// <reference path="GameScreen.ts" />
/// <reference path="TimeSpan.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var SplashScreen = (function (_super) {
    __extends(SplashScreen, _super);
    function SplashScreen(screenManager) {
        _super.call(this, screenManager);
        this.timeElapsed = TimeSpan.Zero;
        this.duration = TimeSpan.FromSeconds(1);
        this.logoImage = new Image();

        this.TransitionDuration = TimeSpan.FromSeconds(1);
    }
    SplashScreen.prototype.LoadContent = function () {
        this.logoImage.src = "Client/Images/OmegaSplashScreen.png";
    };

    SplashScreen.prototype.Update = function (gameTime) {
        _super.prototype.Update.call(this, gameTime);

        if (this.State == ScreenState.Active) {
            this.timeElapsed = new TimeSpan(this.timeElapsed.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);

            if (this.timeElapsed.Milliseconds >= this.duration.Milliseconds) {
                this.ScreenManager.RemoveScreen(this);
            }
        }
    };

    SplashScreen.prototype.Draw = function (gameTime) {
        this.ScreenManager.Game.GraphicsManager.Draw(this.logoImage, new Rectangle(this.ScreenManager.Game.WorldViewport.X, this.ScreenManager.Game.WorldViewport.Y, this.ScreenManager.Game.WorldViewport.Width, this.ScreenManager.Game.WorldViewport.Height), "white");
    };
    return SplashScreen;
})(GameScreen);
