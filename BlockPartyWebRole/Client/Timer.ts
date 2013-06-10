class Timer {
    public ElapsedMilliseconds: number;
    private previousTime: number;

    constructor() {
        this.previousTime = Date.now();
    }

    public Update(): void {
        this.ElapsedMilliseconds = Date.now() - this.previousTime;
        this.previousTime = Date.now();
    }
}