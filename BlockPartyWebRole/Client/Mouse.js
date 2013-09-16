/// <reference path="Graphics.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var MouseState = (function () {
    function MouseState() {
    }
    return MouseState;
})();

var MouseManager = (function () {
    function MouseManager(element) {
        this.DoubleClickDelay = 150;
        this.canvas = element[0];
        var that = this;

        this.canvas.addEventListener('mousemove', function (event) {
            var boundingRect = that.canvas.getBoundingClientRect();
            that.x = (event.clientX - boundingRect.left) * Graphics.WorldWidth / that.canvas.width;
            that.y = (event.clientY - boundingRect.top) * Graphics.WorldHeight / that.canvas.height;
        });

        this.canvas.addEventListener('mousedown', function (event) {
            if (event.button == 0) {
                that.leftButton = true;
            }
            if (event.button == 2) {
                that.rightButton = true;
            }

            event.preventDefault();
        });

        if (window.navigator.msPointerEnabled) {
            this.canvas.addEventListener('MSPointerDown', function (event) {
            });
        }

        this.canvas.addEventListener('mouseup', function (event) {
            if (event.button == 0) {
                that.leftButton = false;
            }
            if (event.button == 2) {
                that.rightButton = false;
            }
        });

        this.canvas.addEventListener('mouseover', function (event) {
            that.present = true;
        });

        this.canvas.addEventListener('mouseout', function () {
            that.present = false;
        });

        // Disable the right click context menu
        this.canvas.addEventListener('contextmenu', function (event) {
            event.preventDefault();
        });
    }
    MouseManager.prototype.GetState = function () {
        var mouseState = new MouseState();

        mouseState.X = this.x;
        mouseState.Y = this.y;
        mouseState.LeftButton = this.leftButton;
        mouseState.RightButton = this.rightButton;
        mouseState.Present = this.present;

        return mouseState;
    };
    return MouseManager;
})();

var Mouse = new MouseManager($("#canvas"));
