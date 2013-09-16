/// <reference path="TimeSpan.ts" />

class GameTime {
    public ElapsedGameTime: TimeSpan;
    public TotalGameTime: TimeSpan;

    private initialTime: number;
    private previousTime: number;

    constructor() {
        this.initialTime = this.previousTime = Date.now();
    }

    public Update(): void {
        this.ElapsedGameTime = TimeSpan.FromMilliseconds(Date.now() - this.previousTime);
        this.TotalGameTime = TimeSpan.FromMilliseconds(Date.now() - this.initialTime);

        this.previousTime = Date.now();
    }
}