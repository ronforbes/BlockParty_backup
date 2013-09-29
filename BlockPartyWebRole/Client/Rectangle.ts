class Rectangle {
    public X: number = 0;
    public Y: number = 0;
    public Width: number = 0;
    public Height: number = 0;

    constructor(x: number, y: number, width: number, height: number) {
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }

    public Contains(x: number, y: number): boolean {
        if (x >= this.X &&
            x <= this.X + this.Width &&
            y >= this.Y &&
            y <= this.Y + this.Height) {
                return true;
        }
        else {
            return false;
        }
    }
}