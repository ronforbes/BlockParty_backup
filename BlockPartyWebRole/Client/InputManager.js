/// <reference path="Point.ts" />
/// <reference path="GraphicsManager.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var MouseState = (function () {
    function MouseState() {
    }
    return MouseState;
})();

var InputManager = (function () {
    function InputManager(game, element) {
        this.WorldPosition = new Point();
        this.ScreenPosition = new Point();
        this.game = game;
        this.canvas = element[0];
        var that = this;

        this.canvas.addEventListener('mousemove', function (event) {
            var boundingRect = that.canvas.getBoundingClientRect();
            that.ScreenPosition.X = event.clientX - boundingRect.left;
            that.ScreenPosition.Y = event.clientY - boundingRect.top;
            that.WorldPosition.X = that.ScreenPosition.X * game.WorldViewport.Width / that.canvas.width;
            that.WorldPosition.Y = that.ScreenPosition.Y * game.WorldViewport.Height / that.canvas.height;
        });

        this.canvas.addEventListener('mousedown', function (event) {
            if (event.button == 0) {
                that.LeftButton = true;
            }
            if (event.button == 2) {
                that.RightButton = true;
            }

            event.preventDefault();
        });

        this.canvas.addEventListener('mouseup', function (event) {
            if (event.button == 0) {
                that.LeftButton = false;
            }
            if (event.button == 2) {
                that.RightButton = false;
            }
        });

        // Disable the right click context menu
        this.canvas.addEventListener('contextmenu', function (event) {
            event.preventDefault();
        });

        this.previousMouseState = new MouseState();
    }
    InputManager.prototype.Update = function () {
        this.LeftButtonPressed = (this.LeftButton && !this.previousMouseState.LeftButton) ? true : false;
        this.RightButtonPressed = (this.RightButton && !this.previousMouseState.RightButton) ? true : false;
        this.LeftButtonReleased = (!this.LeftButton && this.previousMouseState.LeftButton) ? true : false;
        this.RightButtonReleased = (!this.RightButton && this.previousMouseState.RightButton) ? true : false;

        this.previousMouseState.ScreenPosition = this.ScreenPosition;
        this.previousMouseState.WorldPosition = this.WorldPosition;
        this.previousMouseState.LeftButton = this.LeftButton;
        this.previousMouseState.RightButton = this.RightButton;
    };
    return InputManager;
})();
