var Game = (function () {
    function Game() {
    }
    Game.prototype.Update = function (elapsedMilliseconds) {
        console.log("updating");
        Graphics.DrawFullscreenRectangle("white");
        Graphics.Draw();
    };
    return Game;
})();
