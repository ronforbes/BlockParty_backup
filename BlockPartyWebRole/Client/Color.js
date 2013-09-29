/// <reference path="Vector3.ts" />
var Color = (function () {
    function Color(r, g, b, a) {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }
    Color.prototype.ToVector3 = function () {
        return new Vector3(this.R / 255, this.G / 255, this.B / 255);
    };

    Color.prototype.ToString = function () {
        return "rgb(" + this.R + "," + this.G + "," + this.B + "," + this.A + ")";
    };

    Color.FromVector3 = function (vector) {
        return new Color(vector.X * 255, vector.Y * 255, vector.Z * 255, 255);
    };
    Color.Red = new Color(255, 0, 0, 255);
    Color.Green = new Color(0, 255, 0, 255);
    Color.Blue = new Color(0, 0, 255, 255);
    Color.Yellow = new Color(255, 255, 0, 255);
    Color.Cyan = new Color(0, 255, 255, 255);
    Color.Magenta = new Color(255, 0, 255, 255);
    Color.White = new Color(255, 255, 255, 255);
    Color.Black = new Color(0, 0, 0, 255);
    Color.Gray = new Color(128, 128, 128, 255);
    return Color;
})();
