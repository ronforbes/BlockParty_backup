class Vector3 {
    public X: number = 0;
    public Y: number = 0;
    public Z: number = 0;

    constructor(x: number, y: number, z: number) {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public static Zero: Vector3 = new Vector3(0, 0, 0);

    public AddVector(to: Vector3): Vector3 {
        return new Vector3(this.X + to.X, this.Y + to.Y, this.Z + to.Z);
    }

    public MultiplyScalar(by: number): Vector3 {
        return new Vector3(this.X * by, this.Y * by, this.Z * by);
    }
}