/// <reference path="Game.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="GameTime.ts" />

$(function () {
    var gameHub = (<any>$).connection.gameHub;
    var game: Game;
    var gameTime: GameTime;
    var targetFrameRate: number = 60;

    function InitializeGame() {
        game = new Game();
        gameTime = new GameTime();

        game.Initialize();        
        game.LoadContent();

        // Start the game loop
        (<any>window).requestAnimationFrame = (function () {
            return window.requestAnimationFrame ||
                (<any>window).webkitRequestAnimationFrame ||
                (<any>window).mozRequestAnimationFrame ||
                (<any>window).oRequestAnimationFrame ||
                (<any>window).msRequestAnimationFrame ||
                function (Callback) {
                    window.setTimeout(Callback, 1000 / targetFrameRate);
                };
        })();

        (function Update() {
            (<any>window).requestAnimationFrame(Update);
            gameTime.Update();
            game.Update(gameTime);
            game.Draw(gameTime);
        })();
    }

    (<any>$).connection.hub.start(function () {
        InitializeGame();
    });
});