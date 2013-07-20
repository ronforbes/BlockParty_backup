$(function () {
    var gameHub = ($).connection.gameHub;
    var game;
    var timer;
    var updatesPerSecond = 60;
    function InitializeGame() {
        game = new Game();
        timer = new Timer();
        Graphics.Initialize();
        (window).requestAnimationFrame = (function () {
            return window.requestAnimationFrame || (window).webkitRequestAnimationFrame || (window).mozRequestAnimationFrame || (window).oRequestAnimationFrame || (window).msRequestAnimationFrame || function (Callback) {
                window.setTimeout(Callback, 1000 / updatesPerSecond);
            };
        })();
        (function Update() {
            (window).requestAnimationFrame(Update);
            timer.Update();
            game.Update(timer.ElapsedMilliseconds);
        })();
    }
    ($).connection.hub.start(function () {
        InitializeGame();
    });
});
