/// <reference path="ScreenManager.ts" />
enum ScreenState {
    TransitioningOn,
    Active,
    TransitioningOff
}

class GameScreen {
    public ScreenManager: ScreenManager;
    public State: ScreenState;
    public TransitionTimeElapsed: TimeSpan = TimeSpan.Zero;
    public TransitionDuration: TimeSpan = TimeSpan.Zero;

    constructor(screenManager: ScreenManager) {
        this.ScreenManager = screenManager;
    }

    public LoadContent() {

    }

    public UnloadContent() {

    }

    public TransitionOff() {
        this.State = ScreenState.TransitioningOff;
        this.TransitionTimeElapsed = TimeSpan.Zero;
    }

    public Update(gameTime: GameTime) {
        if (this.State == ScreenState.TransitioningOn || this.State == ScreenState.TransitioningOff) {
            this.UpdateTransition(gameTime);
        }
    }

    private UpdateTransition(gameTime: GameTime) {
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
    }

    public HandleInput(gameTime: GameTime) {

    }

    public Draw(gameTime: GameTime) {
        if (this.State == ScreenState.TransitioningOn || this.State == ScreenState.TransitioningOff) {
            this.DrawTransition();
        }
    }

    private DrawTransition() {

    }
}