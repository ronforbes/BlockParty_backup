var TimeSpan = (function () {
    function TimeSpan(milliseconds) {
        this.Seconds = Math.floor(milliseconds / 1000);
        this.Milliseconds = milliseconds;
        this.TotalSeconds = milliseconds / 1000;
        this.TotalMilliseconds = milliseconds;
    }
    TimeSpan.FromSeconds = function (seconds) {
        return new TimeSpan(seconds * 1000);
    };

    TimeSpan.FromMilliseconds = function (milliseconds) {
        return new TimeSpan(milliseconds);
    };

    TimeSpan.Zero = TimeSpan.FromSeconds(0);
    return TimeSpan;
})();
