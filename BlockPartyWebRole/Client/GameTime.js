/// <reference path="TimeSpan.ts" />
var GameTime = (function () {
    function GameTime() {
        this.initialTime = this.previousTime = Date.now();
    }
    GameTime.prototype.Update = function () {
        this.ElapsedGameTime = TimeSpan.FromMilliseconds(Date.now() - this.previousTime);
        this.TotalGameTime = TimeSpan.FromMilliseconds(Date.now() - this.initialTime);

        this.previousTime = Date.now();
    };
    return GameTime;
})();
