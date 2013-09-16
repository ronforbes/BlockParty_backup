/// <reference path="Rectangle.ts" />
/// <reference path="Viewport.ts" />
/// <reference path="ScreenManager.ts" />
/// <reference path="Vector2.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />

class GraphicsManager {
    private game: Game;
    private canvas: HTMLCanvasElement;
    private canvasContext: CanvasRenderingContext2D;
    private backBuffer: HTMLCanvasElement;
    private backBufferContext: CanvasRenderingContext2D;

    constructor(game: Game, element: JQuery) {
        this.game = game;
        this.canvas = <HTMLCanvasElement>element[0];
        this.canvasContext = this.canvas.getContext("2d");
        this.backBuffer = <HTMLCanvasElement>document.createElement("canvas");
        this.backBuffer.width = this.canvas.width;
        this.backBuffer.height = this.canvas.height;
        this.backBufferContext = this.backBuffer.getContext("2d");

        var that: GraphicsManager = this;

        $(window).resize(function () {
            // Wait until the window has finished resizing
            setTimeout(function () {
                that.WindowResized();
            }, 250);
        });

        this.WindowResized();
    }

    private WindowResized(): void {
        var that = this;
        this.UpdateDimensions();

        // Recalculate screen dimensions in case there were scrollbars
        setTimeout(function () {
            that.UpdateDimensions();
        }, 1500);
    }

    private UpdateDimensions(): void {
        var newCanvasWidth, newCanvasHeight;
        var windowAspectRatio = window.innerWidth / window.innerHeight;
        var worldAspectRatio = this.game.WorldViewport.Width / this.game.WorldViewport.Height;

        // Adjust the new canvas dimensions to maintain aspect ratio
        if (windowAspectRatio > worldAspectRatio) {
            newCanvasWidth = window.innerHeight * worldAspectRatio;
            newCanvasHeight = window.innerHeight;
        }
        else {
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
    }

    public DrawLine(startPosition: Vector2, endPosition: Vector2, width: number, color: string): void {
        var canvasStartPosition: Vector2 = this.TransformWorldToCanvas(startPosition);
        var canvasEndPosition: Vector2 = this.TransformWorldToCanvas(endPosition);

        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        //this.backBufferContext.shadowOffsetX = 0;
        //this.backBufferContext.shadowOffsetY = 0;
        //this.backBufferContext.shadowBlur = 10;
        //this.backBufferContext.shadowColor = color;
        this.backBufferContext.beginPath();
        this.backBufferContext.strokeStyle = color;
        this.backBufferContext.lineWidth = width;
        this.backBufferContext.moveTo(canvasStartPosition.X, canvasStartPosition.Y);
        this.backBufferContext.lineTo(canvasEndPosition.X, canvasEndPosition.Y);
        this.backBufferContext.stroke();

        this.backBufferContext.restore();
    }

    public DrawRectangle(position: Vector2, width: number, height: number, lineWidth: number, strokeColor: string, fillColor: string) {
        var canvasPosition: Vector2 = this.TransformWorldToCanvas(position);
        var canvasWidth: number = this.TransformWorldToCanvasX(width);
        var canvasHeight: number = this.TransformWorldToCanvasY(height);

        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        //this.backBufferContext.shadowOffsetX = 0;
        //this.backBufferContext.shadowOffsetY = 0;
        //this.backBufferContext.shadowBlur = 30;
        //this.backBufferContext.shadowColor = fillColor;
        this.backBufferContext.lineWidth = lineWidth;
        if (strokeColor) {
            this.backBufferContext.strokeStyle = strokeColor;
            this.backBufferContext.strokeRect(canvasPosition.X, canvasPosition.Y, canvasWidth, canvasHeight);
        }
        if (fillColor) {
            this.backBufferContext.fillStyle = fillColor;
            this.backBufferContext.fillRect(canvasPosition.X, canvasPosition.Y, canvasWidth, canvasHeight);
        }

        this.backBufferContext.restore();
    }

    public DrawFullscreenRectangle(color: string) {
        this.backBufferContext.save();

        //this.backBufferContext.globalCompositeOperation = "lighter";
        this.backBufferContext.fillStyle = color;
        this.backBufferContext.fillRect(0, 0, this.canvas.width, this.canvas.height);

        this.backBufferContext.restore();
    }

    public DrawCircle(position: Vector2, radius: number, lineWidth: number, color: string, filled: boolean): void {
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
            this.backBufferContext.fillStyle = color;
            this.backBufferContext.fill();
        }
        else {
            this.backBufferContext.strokeStyle = color;
            this.backBufferContext.stroke();
        }

        this.backBufferContext.restore();
    }

    public Draw(image: HTMLImageElement, rectangle: Rectangle, color: string) {
        var canvasPosition = this.TransformWorldToCanvasRectangle(rectangle);

        this.backBufferContext.save();

        this.backBufferContext.drawImage(image, canvasPosition.X, canvasPosition.Y, canvasPosition.Width, canvasPosition.Height);

        this.backBufferContext.restore();
    }

    public DrawText(text: string, position: Vector2, color: string) {
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
    }

    public Clear(): void {
        this.backBufferContext.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }

    public Present(): void {
        this.canvasContext.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.canvasContext.drawImage(this.backBuffer, 0, 0);
    }

    private TransformWorldToCanvas(vector: Vector2): Vector2 {
        return new Vector2(vector.X * this.canvas.width / this.game.WorldViewport.Width, vector.Y * this.canvas.height / this.game.WorldViewport.Height);
    }

    private TransformWorldToCanvasRectangle(rectangle: Rectangle): Rectangle {
        return new Rectangle(rectangle.X * this.canvas.width / this.game.WorldViewport.Width, rectangle.Y * this.canvas.height / this.game.WorldViewport.Height, rectangle.Width * this.canvas.width / this.game.WorldViewport.Width, rectangle.Height * this.canvas.height / this.game.WorldViewport.Height);
    }

    private TransformWorldToCanvasX(x: number) {
        return x * this.canvas.width / this.game.WorldViewport.Width;
    }

    private TransformWorldToCanvasY(y: number) {
        return y * this.canvas.height / this.game.WorldViewport.Height;
    }
}