/// <reference path="Graphics.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />

class MouseState {
    public X: number;
    public Y: number;
    public LeftButton: bool;
    public RightButton: bool;
    public Present: bool;
}

class MouseManager {
    private canvas: HTMLCanvasElement;
    private x: number;
    private y: number;
    private leftButton: bool;
    private rightButton: bool;
    private present: bool;

    constructor(element: JQuery) {
        this.canvas = <HTMLCanvasElement>element[0];
        var that = this;

        this.canvas.addEventListener('mousemove', function (event: MouseEvent) {
            var boundingRect: ClientRect = that.canvas.getBoundingClientRect();
            that.x = (event.clientX - boundingRect.left) * Graphics.WorldWidth / that.canvas.width;
            that.y = (event.clientY - boundingRect.top) * Graphics.WorldHeight / that.canvas.height;
        });

        this.canvas.addEventListener('mousedown', function (event: MouseEvent) {
            if (event.button == 0) {
                that.leftButton = true;
            }
            if (event.button == 2) {
                that.rightButton = true;
            }

            event.preventDefault();
        });

        this.canvas.addEventListener('mouseup', function (event: MouseEvent) {
            if (event.button == 0) {
                that.leftButton = false;
            }
            if (event.button == 2) {
                that.rightButton = false;
            }
        });

        this.canvas.addEventListener('mouseover', function (event: MouseEvent) {
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

    public GetState(): MouseState {
        var mouseState: MouseState = new MouseState();

        mouseState.X = this.x;
        mouseState.Y = this.y;
        mouseState.LeftButton = this.leftButton;
        mouseState.RightButton = this.rightButton;
        mouseState.Present = this.present;

        return mouseState;
    }
}

var Mouse = new MouseManager($("#canvas"));