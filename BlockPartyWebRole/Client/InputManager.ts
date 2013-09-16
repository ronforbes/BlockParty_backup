/// <reference path="Point.ts" />
/// <reference path="GraphicsManager.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />

class MouseState {
    public ScreenPosition: Point;
    public WorldPosition: Point;
    public LeftButton: boolean;
    public RightButton: boolean;
}

class InputManager {
    private game: Game;

    public WorldPosition: Point = new Point();
    public ScreenPosition: Point = new Point();
    public LeftButton: boolean;
    public RightButton: boolean;
    public LeftButtonPressed: boolean;
    public RightButtonPressed: boolean;
    public LeftButtonReleased: boolean;
    public RightButtonReleased: boolean;
    
    private canvas: HTMLCanvasElement;
    private previousMouseState: MouseState;

    constructor(game: Game, element: JQuery) {
        this.game = game;
        this.canvas = <HTMLCanvasElement>element[0];
        var that = this;

        this.canvas.addEventListener('mousemove', function (event: MouseEvent) {
            var boundingRect: ClientRect = that.canvas.getBoundingClientRect();
            that.ScreenPosition.X = event.clientX - boundingRect.left;
            that.ScreenPosition.Y = event.clientY - boundingRect.top;
            that.WorldPosition.X = that.ScreenPosition.X * game.WorldViewport.Width / that.canvas.width;
            that.WorldPosition.Y = that.ScreenPosition.Y * game.WorldViewport.Height / that.canvas.height;
        });

        this.canvas.addEventListener('mousedown', function (event: MouseEvent) {
            if (event.button == 0) {
                that.LeftButton = true;
            }
            if (event.button == 2) {
                that.RightButton = true;
            }

            event.preventDefault();
        });
        
        this.canvas.addEventListener('mouseup', function (event: MouseEvent) {
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

    public Update() {
        this.LeftButtonPressed = (this.LeftButton && !this.previousMouseState.LeftButton) ? true : false;
        this.RightButtonPressed = (this.RightButton && !this.previousMouseState.RightButton) ? true : false;
        this.LeftButtonReleased = (!this.LeftButton && this.previousMouseState.LeftButton) ? true : false;
        this.RightButtonReleased = (!this.RightButton && this.previousMouseState.RightButton) ? true : false;

        this.previousMouseState.ScreenPosition = this.ScreenPosition;
        this.previousMouseState.WorldPosition = this.WorldPosition;
        this.previousMouseState.LeftButton = this.LeftButton;
        this.previousMouseState.RightButton = this.RightButton;
    }
}