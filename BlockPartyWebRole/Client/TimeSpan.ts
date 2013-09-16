class TimeSpan {
    public Seconds: number;
    public Milliseconds: number;
    public TotalSeconds: number;
    public TotalMilliseconds: number;

    constructor(milliseconds: number) {
        this.Seconds = Math.floor(milliseconds / 1000);
        this.Milliseconds = milliseconds;
        this.TotalSeconds = milliseconds / 1000;
        this.TotalMilliseconds = milliseconds;
    }

    public static FromSeconds(seconds: number): TimeSpan {
        return new TimeSpan(seconds * 1000);
    }

    public static FromMilliseconds(milliseconds: number): TimeSpan {
        return new TimeSpan(milliseconds);
    }

    public static Zero: TimeSpan = TimeSpan.FromSeconds(0);
}