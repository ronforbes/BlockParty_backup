/// <reference path="BlockRain.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="ScreenManager.ts" />
/// <reference path="GameScreen.ts" />

class MainMenuScreen extends GameScreen {
    private backgroundImage: HTMLImageElement = new Image();
    private titleImage: HTMLImageElement = new Image();
    private blockRain: BlockRain;

    constructor(screenManager: ScreenManager) {
        super(screenManager);

        this.TransitionDuration = TimeSpan.FromSeconds(1);

        this.blockRain = new BlockRain(this);
    }

    public LoadContent() {
        super.LoadContent();

        this.backgroundImage.src = "Client/Images/MainMenuBackground.png";
        this.titleImage.src = "Client/Images/MainMenuTitle.png";

        this.blockRain.LoadContent();
    }

    public Update(gameTime: GameTime) {
        super.Update(gameTime);

        this.blockRain.Update(gameTime);
    }

    public Draw(gameTime: GameTime) {
        this.ScreenManager.Game.GraphicsManager.Draw(this.backgroundImage, new Rectangle(this.ScreenManager.Game.WorldViewport.X, this.ScreenManager.Game.WorldViewport.Y, this.ScreenManager.Game.WorldViewport.Width, this.ScreenManager.Game.WorldViewport.Height), "white");
        this.blockRain.Draw(gameTime);
        this.ScreenManager.Game.GraphicsManager.Draw(this.titleImage, new Rectangle(this.ScreenManager.Game.WorldViewport.X, this.ScreenManager.Game.WorldViewport.Y, this.ScreenManager.Game.WorldViewport.Width, this.ScreenManager.Game.WorldViewport.Height), "white");

        super.Draw(gameTime);
    }
}