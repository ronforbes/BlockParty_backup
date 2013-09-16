/// <reference path="Game.ts" />

class AudioManager {
    private game: Game;

    public Muted: boolean;

    constructor(game: Game) {
        this.game = game;
    }

    public LoadContent() {

    }

    public Play(key: string, volume: number, pitch: number, pan: number) {
        if (!this.Muted) {

        }
    }
}