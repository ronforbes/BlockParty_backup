/// <reference path="Graphics.ts" />

class Game {
    constructor() {

    }

    public Update(elapsedMilliseconds: number): void {
        console.log("updating");

        Graphics.DrawFullscreenRectangle("white");
        Graphics.Draw();
    }
}