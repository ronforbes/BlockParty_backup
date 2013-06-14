var Game = (function () {
    function Game() {
        this.board = new Board();
    }
    Game.prototype.Update = function (elapsedMilliseconds) {
        Graphics.Clear();
        this.board.Update(elapsedMilliseconds);
        Graphics.Draw();
    };
    return Game;
})();
