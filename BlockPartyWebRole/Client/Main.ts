/// <reference path="Graphics.ts" />
/// <reference path="Game.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Timer.ts" />

$(function () {
    var gameHub = (<any>$).connection.gameHub;
    var game: Game;
    var timer: Timer;
    var UpdatesPerSecond: number = 60;

    function InitializeGame() {
        game = new Game();
        timer = new Timer();

        Graphics.Initialize();

        // Start the update loop
        (<any>window).requestAnimationFrame = (function () {
            return window.requestAnimationFrame ||
                (<any>window).webkitRequestAnimationFrame ||
                (<any>window).mozRequestAnimationFrame ||
                (<any>window).oRequestAnimationFrame ||
                (<any>window).msRequestAnimationFrame ||
                function (Callback) {
                    window.setTimeout(Callback, 1000 / UpdatesPerSecond);
                };
        })();

        (function Update() {
            (<any>window).requestAnimationFrame(Update);
            timer.Update();
            game.Update(timer.ElapsedMilliseconds);
        })();
    }

    (<any>$).connection.hub.start(function () {
        InitializeGame();
    });
});