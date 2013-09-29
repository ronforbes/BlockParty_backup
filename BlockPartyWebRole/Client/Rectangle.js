var Rectangle = (function () {
    function Rectangle(x, y, width, height) {
        this.X = 0;
        this.Y = 0;
        this.Width = 0;
        this.Height = 0;
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }
    Rectangle.prototype.Contains = function (x, y) {
        if (x >= this.X && x <= this.X + this.Width && y >= this.Y && y <= this.Y + this.Height) {
            return true;
        } else {
            return false;
        }
    };
    return Rectangle;
})();
