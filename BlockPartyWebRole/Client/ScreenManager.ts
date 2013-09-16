/// <reference path="GameScreen.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="GraphicsManager.ts" />
/// <reference path="Viewport.ts" />
/// <reference path="Game.ts" />

class ScreenManager {
    public Game: Game;

    private screens: Array<GameScreen> = new Array<GameScreen>();
    private screensToUpdate: Array<GameScreen> = new Array<GameScreen>();
    private screenToLoad: GameScreen;
    private loadingScreen: boolean;

    constructor(game: Game) {
        this.Game = game;    
    }

    public AddScreen(screen: GameScreen) {
        screen.LoadContent();

        this.screens.push(screen);
    }

    public RemoveScreen(screen: GameScreen) {
        screen.UnloadContent();

        var index = this.screens.indexOf(screen);
        this.screens.splice(index);
    }

    public LoadScreen(screen: GameScreen) {
        // Transition out of the current screens
        this.screens.forEach(function (screen: GameScreen, index: number, screens: Array<GameScreen>) {
            screen.TransitionOff();
        });

        this.screenToLoad = screen;
        this.loadingScreen = true;
    }

    public Update(gameTime: GameTime) {
        // Read input
        this.Game.InputManager.Update();

        // Make a copy of the list of screens to allow for adds/removes not interfering w/ updates
        this.screensToUpdate.splice(0, this.screensToUpdate.length);

        this.screens.forEach(function (screen: GameScreen, index: number, screens: Array<GameScreen>) {
            this.screensToUpdate.push(screen);
        }, this);

        while (this.screensToUpdate.length > 0) {
            // Pop the topmost screen off the stack
            var screen: GameScreen = this.screensToUpdate.pop();

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
    }

    public Draw(gameTime: GameTime) {
        this.Game.GraphicsManager.Clear();

        // Draw screens
        this.screens.forEach(function (screen: GameScreen, index: number, screens: Array<GameScreen>) {
            screen.Draw(gameTime);
        });
    }
}