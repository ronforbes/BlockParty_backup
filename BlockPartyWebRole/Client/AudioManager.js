/// <reference path="Game.ts" />
var AudioManager = (function () {
    function AudioManager(game) {
        this.game = game;
    }
    AudioManager.prototype.LoadContent = function () {
    };

    AudioManager.prototype.Play = function (key, volume, pitch, pan) {
        if (!this.Muted) {
        }
    };
    return AudioManager;
})();
