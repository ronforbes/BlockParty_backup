/// <reference path="SplashScreen.ts" />
/// <reference path="FrameRateCounter.ts" />
/// <reference path="InputManager.ts" />
/// <reference path="AudioManager.ts" />
/// <reference path="Viewport.ts" />
/// <reference path="GraphicsManager.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="ScreenManager.ts" />

class Game {
    public WorldViewport: Viewport;
    public GraphicsManager: GraphicsManager;
    public InputManager: InputManager;
    public AudioManager: AudioManager;

    private screenManager: ScreenManager;
    private frameRateCounter: FrameRateCounter;

    private defaultWorldWidth: number = 1600;
    private defaultWorldHeight: number = 900;

    constructor() {
        this.WorldViewport = new Viewport(0, 0, this.defaultWorldWidth, this.defaultWorldHeight);
    }

    public Initialize() {
        this.GraphicsManager = new GraphicsManager(this, $("#canvas"));
        this.InputManager = new InputManager(this, $("#canvas"));
        this.AudioManager = new AudioManager(this);
        this.screenManager = new ScreenManager(this);
        this.frameRateCounter = new FrameRateCounter(this);
    }

    public LoadContent() {
        this.AudioManager.LoadContent();
        
        this.screenManager.AddScreen(new SplashScreen(this.screenManager));
    }

    public Update(gameTime: GameTime): void {
        this.screenManager.Update(gameTime);
        this.frameRateCounter.Update(gameTime);
    }

    public Draw(gameTime: GameTime) {
        this.screenManager.Draw(gameTime);
        this.frameRateCounter.Draw(gameTime);
        this.GraphicsManager.Present();
    }
}