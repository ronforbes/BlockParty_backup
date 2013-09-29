/// <reference path="Vector3.ts" />

class Color {
    public R: number;
    public G: number;
    public B: number;
    public A: number;

    constructor(r: number, g: number, b: number, a: number) {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    public ToVector3(): Vector3 {
        return new Vector3(this.R / 255, this.G / 255, this.B / 255);
    }

    public ToString(): string {
        return "rgb(" + this.R + "," + this.G + "," + this.B + "," + this.A + ")";
    }

    public static Red: Color = new Color(255, 0, 0, 255);
    public static Green: Color = new Color(0, 255, 0, 255);
    public static Blue: Color = new Color(0, 0, 255, 255);
    public static Yellow: Color = new Color(255, 255, 0, 255);
    public static Cyan: Color = new Color(0, 255, 255, 255);
    public static Magenta: Color = new Color(255, 0, 255, 255);
    public static White: Color = new Color(255, 255, 255, 255);
    public static Black: Color = new Color(0, 0, 0, 255);
    public static Gray: Color = new Color(128, 128, 128, 255);

    public static FromVector3(vector: Vector3): Color {
        return new Color(vector.X * 255, vector.Y * 255, vector.Z * 255, 255);
    }
}