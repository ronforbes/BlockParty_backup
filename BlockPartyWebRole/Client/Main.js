/// <reference path="Game.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="GameTime.ts" />
$(function () {
    var gameHub = ($).connection.gameHub;
    var game;
    var gameTime;
    var targetFrameRate = 60;

    function InitializeGame() {
        game = new Game();
        gameTime = new GameTime();

        game.Initialize();
        game.LoadContent();

        // Start the game loop
        (window).requestAnimationFrame = (function () {
            return window.requestAnimationFrame || (window).webkitRequestAnimationFrame || (window).mozRequestAnimationFrame || (window).oRequestAnimationFrame || (window).msRequestAnimationFrame || function (Callback) {
                window.setTimeout(Callback, 1000 / targetFrameRate);
            };
        })();

        (function Update() {
            (window).requestAnimationFrame(Update);
            gameTime.Update();
            game.Update(gameTime);
            game.Draw(gameTime);
        })();
    }

    ($).connection.hub.start(function () {
        InitializeGame();
    });
});
