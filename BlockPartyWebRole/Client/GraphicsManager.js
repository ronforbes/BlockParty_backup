/// <reference path="Color.ts" />
/// <reference path="Rectangle.ts" />
/// <reference path="Viewport.ts" />
/// <reference path="ScreenManager.ts" />
/// <reference path="Vector2.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var GraphicsManager = (function () {
    function GraphicsManager(game, element) {
        this.game = game;
        this.canvas = element[0];
        this.canvasContext = this.canvas.getContext("2d");
        this.backBuffer = document.createElement("canvas");
        this.backBuffer.width = this.canvas.width;
        this.backBuffer.height = this.canvas.height;
        this.backBufferContext = this.backBuffer.getContext("2d");

        var that = this;

        $(window).resize(function () {
            // Wait until the window has finished resizing
            setTimeout(function () {
                that.WindowResized();
            }, 250);
        });

        this.WindowResized();
    }
    GraphicsManager.prototype.WindowResized = function () {
        var that = this;
        this.UpdateDimensions();

        // Recalculate screen dimensions in case there were scrollbars
        setTimeout(function () {
            that.UpdateDimensions();
        }, 1500);
    };

    GraphicsManager.prototype.UpdateDimensions = function () {
        var newCanvasWidth, newCanvasHeight;
        var windowAspectRatio = window.innerWidth / window.innerHeight;
        var worldAspectRatio = this.game.WorldViewport.Width / this.game.WorldViewport.Height;

        if (windowAspectRatio > worldAspectRatio) {
            newCanvasWidth = window.innerHeight * worldAspectRatio;
            newCanvasHeight = window.innerHeight;
        } else {
            newCanvasWidth = window.innerWidth;
            newCanvasHeight = window.innerWidth / worldAspectRatio;
        }

        // Scale the canvas element to fit the screen while maintaining aspect ratio
        this.canvas.style.width = newCanvasWidth + "px";
        this.canvas.style.height = newCanvasHeight + "px";

        // Update the canvas drawing dimensions
        this.canvas.width = newCanvasWidth;
        this.canvas.height = newCanvasHeight;
        this.backBuffer.width = newCanvasWidth;
        this.backBuffer.height = newCanvasHeight;

        // Center the canvas
        this.canvas.style.left = (window.innerWidth - this.canvas.width) / 2 + "px";
        this.canvas.style.top = (window.innerHeight - this.canvas.height) / 2 + "px";
    };

    GraphicsManager.prototype.DrawLine = function (startPosition, endPosition, width, color) {
        var canvasStartPosition = this.TransformWorldToCanvas(startPosition);
        var canvasEndPosition = this.TransformWorldToCanvas(endPosition);

        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        //this.backBufferContext.shadowOffsetX = 0;
        //this.backBufferContext.shadowOffsetY = 0;
        //this.backBufferContext.shadowBlur = 10;
        //this.backBufferContext.shadowColor = color;
        this.backBufferContext.beginPath();
        this.backBufferContext.strokeStyle = color.ToString();
        this.backBufferContext.lineWidth = width;
        this.backBufferContext.moveTo(canvasStartPosition.X, canvasStartPosition.Y);
        this.backBufferContext.lineTo(canvasEndPosition.X, canvasEndPosition.Y);
        this.backBufferContext.stroke();

        this.backBufferContext.restore();
    };

    GraphicsManager.prototype.DrawRectangle = function (position, width, height, lineWidth, strokeColor, fillColor) {
        var canvasPosition = this.TransformWorldToCanvas(position);
        var canvasWidth = this.TransformWorldToCanvasX(width);
        var canvasHeight = this.TransformWorldToCanvasY(height);

        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        //this.backBufferContext.shadowOffsetX = 0;
        //this.backBufferContext.shadowOffsetY = 0;
        //this.backBufferContext.shadowBlur = 30;
        //this.backBufferContext.shadowColor = fillColor;
        this.backBufferContext.lineWidth = lineWidth;
        if (strokeColor) {
            this.backBufferContext.strokeStyle = strokeColor.ToString();
            this.backBufferContext.strokeRect(canvasPosition.X, canvasPosition.Y, canvasWidth, canvasHeight);
        }
        if (fillColor) {
            this.backBufferContext.fillStyle = fillColor.ToString();
            this.backBufferContext.fillRect(canvasPosition.X, canvasPosition.Y, canvasWidth, canvasHeight);
        }

        this.backBufferContext.restore();
    };

    GraphicsManager.prototype.DrawFullscreenRectangle = function (color) {
        this.backBufferContext.save();

        this.backBufferContext.globalCompositeOperation = "lighter";
        this.backBufferContext.fillStyle = color.ToString();
        this.backBufferContext.fillRect(0, 0, this.canvas.width, this.canvas.height);

        this.backBufferContext.restore();
    };

    GraphicsManager.prototype.DrawCircle = function (position, radius, lineWidth, color, filled) {
        var canvasPosition = this.TransformWorldToCanvas(position);
        var canvasRadius = this.TransformWorldToCanvasX(radius);

        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        //this.backBufferContext.shadowOffsetX = 0;
        //this.backBufferContext.shadowOffsetY = 0;
        //this.backBufferContext.shadowBlur = 10;
        //this.backBufferContext.shadowColor = color;
        this.backBufferContext.beginPath();
        this.backBufferContext.lineWidth = lineWidth;
        this.backBufferContext.arc(canvasPosition.X, canvasPosition.Y, canvasRadius, 0, Math.PI * 2, true);
        if (filled) {
            this.backBufferContext.fillStyle = color.ToString();
            this.backBufferContext.fill();
        } else {
            this.backBufferContext.strokeStyle = color.ToString();
            this.backBufferContext.stroke();
        }

        this.backBufferContext.restore();
    };

    GraphicsManager.prototype.Draw = function (image, rectangle, color) {
        var canvasPosition = this.TransformWorldToCanvasRectangle(rectangle);

        this.backBufferContext.save();
        this.backBufferContext.drawImage(image, canvasPosition.X, canvasPosition.Y, canvasPosition.Width, canvasPosition.Height);
        this.backBufferContext.restore();
    };

    GraphicsManager.prototype.DrawText = function (text, position, color) {
        var canvasPosition = this.TransformWorldToCanvas(position);

        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        //this.backBufferContext.shadowOffsetX = 0;
        //this.backBufferContext.shadowOffsetY = 0;
        //this.backBufferContext.shadowBlur = 10;
        //this.backBufferContext.shadowColor = color;
        this.backBufferContext.font = "bold 24px Arial";
        this.backBufferContext.textBaseline = "top";
        this.backBufferContext.fillStyle = color;
        this.backBufferContext.fillText(text, canvasPosition.X, canvasPosition.Y);

        this.backBufferContext.restore();
    };

    GraphicsManager.prototype.Clear = function () {
        this.backBufferContext.clearRect(0, 0, this.canvas.width, this.canvas.height);
    };

    GraphicsManager.prototype.Present = function () {
        this.canvasContext.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.canvasContext.globalAlpha = this.GlobalAlpha;
        this.canvasContext.drawImage(this.backBuffer, 0, 0);
    };

    GraphicsManager.prototype.TransformWorldToCanvas = function (vector) {
        return new Vector2(vector.X * this.canvas.width / this.game.WorldViewport.Width, vector.Y * this.canvas.height / this.game.WorldViewport.Height);
    };

    GraphicsManager.prototype.TransformWorldToCanvasRectangle = function (rectangle) {
        return new Rectangle(rectangle.X * this.canvas.width / this.game.WorldViewport.Width, rectangle.Y * this.canvas.height / this.game.WorldViewport.Height, rectangle.Width * this.canvas.width / this.game.WorldViewport.Width, rectangle.Height * this.canvas.height / this.game.WorldViewport.Height);
    };

    GraphicsManager.prototype.TransformWorldToCanvasX = function (x) {
        return x * this.canvas.width / this.game.WorldViewport.Width;
    };

    GraphicsManager.prototype.TransformWorldToCanvasY = function (y) {
        return y * this.canvas.height / this.game.WorldViewport.Height;
    };
    return GraphicsManager;
})();
