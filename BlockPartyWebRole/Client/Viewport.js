/// <reference path="Rectangle.ts" />
var Viewport = (function () {
    function Viewport(x, y, width, height) {
        this.X = 0;
        this.Y = 0;
        this.Width = 0;
        this.Height = 0;
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }
    return Viewport;
})();
