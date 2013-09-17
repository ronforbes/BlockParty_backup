/// <reference path="BlockRain.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="ScreenManager.ts" />
/// <reference path="GameScreen.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MainMenuScreen = (function (_super) {
    __extends(MainMenuScreen, _super);
    function MainMenuScreen(screenManager) {
        _super.call(this, screenManager);
        this.backgroundImage = new Image();
        this.titleImage = new Image();

        this.TransitionDuration = TimeSpan.FromSeconds(1);

        this.blockRain = new BlockRain(this);
    }
    MainMenuScreen.prototype.LoadContent = function () {
        _super.prototype.LoadContent.call(this);

        this.backgroundImage.src = "Client/Images/MainMenuBackground.png";
        this.titleImage.src = "Client/Images/MainMenuTitle.png";

        this.blockRain.LoadContent();
    };

    MainMenuScreen.prototype.Update = function (gameTime) {
        _super.prototype.Update.call(this, gameTime);

        this.blockRain.Update(gameTime);
    };

    MainMenuScreen.prototype.Draw = function (gameTime) {
        this.ScreenManager.Game.GraphicsManager.Draw(this.backgroundImage, new Rectangle(this.ScreenManager.Game.WorldViewport.X, this.ScreenManager.Game.WorldViewport.Y, this.ScreenManager.Game.WorldViewport.Width, this.ScreenManager.Game.WorldViewport.Height), "white");
        this.blockRain.Draw(gameTime);
        this.ScreenManager.Game.GraphicsManager.Draw(this.titleImage, new Rectangle(this.ScreenManager.Game.WorldViewport.X, this.ScreenManager.Game.WorldViewport.Y, this.ScreenManager.Game.WorldViewport.Width, this.ScreenManager.Game.WorldViewport.Height), "white");

        _super.prototype.Draw.call(this, gameTime);
    };
    return MainMenuScreen;
})(GameScreen);
