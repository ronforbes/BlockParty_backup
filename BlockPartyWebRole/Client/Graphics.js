var GraphicsManager = (function () {
    function GraphicsManager(element) {
        this.WorldWidth = 100;
        this.WorldHeight = 100;
        this.AspectRatio = 4 / 3;
        this.canvas = element[0];
        this.canvasContext = this.canvas.getContext("2d");
        this.backBuffer = document.createElement("canvas");
        this.backBuffer.width = this.canvas.width;
        this.backBuffer.height = this.canvas.height;
        this.backBufferContext = this.backBuffer.getContext("2d");
        var that = this;
        $(window).resize(function () {
            setTimeout(function () {
                that.WindowResized();
            }, 250);
        });
    }
    GraphicsManager.prototype.Initialize = function () {
        this.WindowResized();
    };
    GraphicsManager.prototype.WindowResized = function () {
        var that = this;
        this.UpdateDimensions();
        setTimeout(function () {
            that.UpdateDimensions();
        }, 1500);
    };
    GraphicsManager.prototype.UpdateDimensions = function () {
        var newCanvasWidth, newCanvasHeight;
        var windowAspectRatio = window.innerWidth / window.innerHeight;
        if(windowAspectRatio > this.AspectRatio) {
            newCanvasWidth = window.innerHeight * this.AspectRatio;
            newCanvasHeight = window.innerHeight;
        } else {
            newCanvasWidth = window.innerWidth;
            newCanvasHeight = window.innerWidth / this.AspectRatio;
        }
        this.canvas.style.width = newCanvasWidth + "px";
        this.canvas.style.height = newCanvasHeight + "px";
        this.canvas.width = newCanvasWidth;
        this.canvas.height = newCanvasHeight;
        this.backBuffer.width = newCanvasWidth;
        this.backBuffer.height = newCanvasHeight;
        this.canvas.style.left = (window.innerWidth - this.canvas.width) / 2 + "px";
        this.canvas.style.top = (window.innerHeight - this.canvas.height) / 2 + "px";
    };
    GraphicsManager.prototype.DrawLine = function (startPosition, endPosition, width, color) {
        var canvasStartPosition = this.TransformWorldToCanvas(startPosition);
        var canvasEndPosition = this.TransformWorldToCanvas(endPosition);
        this.backBufferContext.save();
        this.backBufferContext.beginPath();
        this.backBufferContext.strokeStyle = color;
        this.backBufferContext.lineWidth = width;
        this.backBufferContext.moveTo(canvasStartPosition.X, canvasStartPosition.Y);
        this.backBufferContext.lineTo(canvasEndPosition.X, canvasEndPosition.Y);
        this.backBufferContext.stroke();
        this.backBufferContext.restore();
    };
    GraphicsManager.prototype.DrawRectangle = function (position, width, height, lineWidth, color, filled) {
        var canvasPosition = this.TransformWorldToCanvas(position);
        var canvasWidth = this.TransformWorldToCanvasX(width);
        var canvasHeight = this.TransformWorldToCanvasY(height);
        this.backBufferContext.save();
        this.backBufferContext.lineWidth = lineWidth;
        if(filled) {
            this.backBufferContext.fillStyle = color;
            this.backBufferContext.fillRect(canvasPosition.X, canvasPosition.Y, canvasWidth, canvasHeight);
        } else {
            this.backBufferContext.strokeStyle = color;
            this.backBufferContext.strokeRect(canvasPosition.X, canvasPosition.Y, canvasWidth, canvasHeight);
        }
        this.backBufferContext.restore();
    };
    GraphicsManager.prototype.DrawFullscreenRectangle = function (color) {
        this.backBufferContext.save();
        this.backBufferContext.fillStyle = color;
        this.backBufferContext.fillRect(0, 0, this.canvas.width, this.canvas.height);
        this.backBufferContext.restore();
    };
    GraphicsManager.prototype.DrawCircle = function (position, radius, lineWidth, color, filled) {
        var canvasPosition = this.TransformWorldToCanvas(position);
        var canvasRadius = this.TransformWorldToCanvasX(radius);
        this.backBufferContext.save();
        this.backBufferContext.beginPath();
        this.backBufferContext.lineWidth = lineWidth;
        this.backBufferContext.arc(canvasPosition.X, canvasPosition.Y, canvasRadius, 0, Math.PI * 2, true);
        if(filled) {
            this.backBufferContext.fillStyle = color;
            this.backBufferContext.fill();
        } else {
            this.backBufferContext.strokeStyle = color;
            this.backBufferContext.stroke();
        }
        this.backBufferContext.restore();
    };
    GraphicsManager.prototype.DrawText = function (text, position, color) {
        var canvasPosition = this.TransformWorldToCanvas(position);
        this.backBufferContext.save();
        this.backBufferContext.font = "bold 16px Arial";
        this.backBufferContext.fillStyle = color;
        this.backBufferContext.fillText(text, canvasPosition.X, canvasPosition.Y);
        this.backBufferContext.restore();
    };
    GraphicsManager.prototype.Clear = function () {
        this.backBufferContext.clearRect(0, 0, this.canvas.width, this.canvas.height);
    };
    GraphicsManager.prototype.Draw = function () {
        this.canvasContext.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.canvasContext.drawImage(this.backBuffer, 0, 0);
    };
    GraphicsManager.prototype.TransformWorldToCanvas = function (vector) {
        return new Vector2(vector.X * this.canvas.width / this.WorldWidth, vector.Y * this.canvas.height / this.WorldHeight);
    };
    GraphicsManager.prototype.TransformWorldToCanvasX = function (x) {
        return x * this.canvas.width / this.WorldWidth;
    };
    GraphicsManager.prototype.TransformWorldToCanvasY = function (y) {
        return y * this.canvas.height / this.WorldHeight;
    };
    return GraphicsManager;
})();
var Graphics = new GraphicsManager($("#canvas"));
