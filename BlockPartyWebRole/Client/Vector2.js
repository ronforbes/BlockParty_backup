var Vector2 = (function () {
    function Vector2(x, y) {
        this.X = 0;
        this.Y = 0;
        this.X = x;
        this.Y = y;
    }
    Vector2.prototype.AddVector = function (to) {
        return new Vector2(this.X + to.X, this.Y + to.Y);
    };

    Vector2.prototype.MultiplyScalar = function (by) {
        return new Vector2(this.X * by, this.Y * by);
    };
    Vector2.Zero = new Vector2(0, 0);
    return Vector2;
})();
