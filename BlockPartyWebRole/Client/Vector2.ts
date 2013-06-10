class Vector2 {
    public X: number = 0;
    public Y: number = 0;

    constructor(x: number, y: number) {
        this.X = x;
        this.Y = y;
    }

    public AddVector(to: Vector2): Vector2 {
        return new Vector2(this.X + to.X, this.Y + to.Y);
    }

    public MultiplyScalar(by: number): Vector2 {
        return new Vector2(this.X * by, this.Y * by);
    }
}