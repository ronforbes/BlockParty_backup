var Timer = (function () {
    function Timer() {
        this.previousTime = Date.now();
    }
    Timer.prototype.Update = function () {
        this.ElapsedMilliseconds = Date.now() - this.previousTime;
        this.previousTime = Date.now();
    };
    return Timer;
})();
