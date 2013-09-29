var Vector3 = (function () {
    function Vector3(x, y, z) {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    Vector3.prototype.AddVector = function (to) {
        return new Vector3(this.X + to.X, this.Y + to.Y, this.Z + to.Z);
    };

    Vector3.prototype.MultiplyScalar = function (by) {
        return new Vector3(this.X * by, this.Y * by, this.Z * by);
    };
    Vector3.Zero = new Vector3(0, 0, 0);
    return Vector3;
})();
