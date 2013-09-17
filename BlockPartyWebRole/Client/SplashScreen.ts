/// <reference path="MainMenuScreen.ts" />
/// <reference path="GameScreen.ts" />
/// <reference path="TimeSpan.ts" />

class SplashScreen extends GameScreen {
    private timeElapsed: TimeSpan = TimeSpan.Zero;
    private duration: TimeSpan = TimeSpan.FromSeconds(1);
    private logoImage: HTMLImageElement = new Image();

    constructor(screenManager: ScreenManager) {
        super(screenManager);

        this.TransitionDuration = TimeSpan.FromSeconds(1);
    }

    public LoadContent() {
        this.logoImage.src = "Client/Images/OmegaSplashScreen.png";
    }

    public Update(gameTime: GameTime) {
        super.Update(gameTime);

        if (this.State == ScreenState.Active) {
            this.timeElapsed = new TimeSpan(this.timeElapsed.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);

            if (this.timeElapsed.Milliseconds >= this.duration.Milliseconds) {
                this.ScreenManager.LoadScreen(new MainMenuScreen(this.ScreenManager));
            }
        }
    }

    public Draw(gameTime: GameTime) {
        this.ScreenManager.Game.GraphicsManager.Draw(this.logoImage, new Rectangle(this.ScreenManager.Game.WorldViewport.X, this.ScreenManager.Game.WorldViewport.Y, this.ScreenManager.Game.WorldViewport.Width, this.ScreenManager.Game.WorldViewport.Height), "white");

        super.Draw(gameTime);
    }
}