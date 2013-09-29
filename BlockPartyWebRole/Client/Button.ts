/// <reference path="Color.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="TimeSpan.ts" />
/// <reference path="Vector2.ts" />
/// <reference path="Rectangle.ts" />
/// <reference path="GameScreen.ts" />

enum ButtonAnimationState {
    Idle,
    Pressing,
    Releasing
}

class Button {
    public Text: string;
    public Selected: Function;

    private screen: GameScreen;
    private textColor: Color = Color.White;
    private rectangle: Rectangle;
    private rectangleColor: Color = Color.Gray;
    private scale: Vector2 = Vector2.Zero;
    private pressed: boolean;
    private animationState: ButtonAnimationState = ButtonAnimationState.Idle;
    private scaleTimeElapsed: TimeSpan = TimeSpan.Zero;
    private scaleDuration: TimeSpan = TimeSpan.FromSeconds(0.5);
    private image: HTMLImageElement;

    constructor(screen: GameScreen, text: string, textColor: Color, rectangle: Rectangle, rectangleColor: Color) {
        this.screen = screen;
        this.Text = text;
        this.textColor = textColor;
        this.rectangle = rectangle;
        this.rectangleColor = rectangleColor;
    }

    public LoadContent() {
        this.image.src = "Client/Images/Button.png";
    }

    public Update(gameTime: GameTime) {
        // Scale the button based on animation state
        switch (this.animationState) {
            case ButtonAnimationState.Idle: break;
            case ButtonAnimationState.Pressing:
                this.scaleTimeElapsed = TimeSpan.FromMilliseconds(this.scaleTimeElapsed.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);
                this.scale.X = Tween.BounceEaseOut(this.scaleTimeElapsed.TotalMilliseconds, 1, -0.25, this.scaleDuration.TotalMilliseconds);
                this.scale.Y = Tween.BounceEaseOut(this.scaleTimeElapsed.TotalMilliseconds, 1, -0.25, this.scaleDuration.TotalMilliseconds);
                if (this.scaleTimeElapsed.Milliseconds >= this.scaleDuration.Milliseconds) {
                    this.animationState = ButtonAnimationState.Idle;
                }
                break;
            case ButtonAnimationState.Releasing:
                this.scaleTimeElapsed = TimeSpan.FromMilliseconds(this.scaleTimeElapsed.Milliseconds + gameTime.ElapsedGameTime.Milliseconds);
                this.scale.X = Tween.BounceEaseOut(this.scaleTimeElapsed.TotalMilliseconds, 0.75, 0.25, this.scaleDuration.Milliseconds);
                this.scale.Y = Tween.BounceEaseOut(this.scaleTimeElapsed.TotalMilliseconds, 0.75, 0.25, this.scaleDuration.Milliseconds);
                if (this.scaleTimeElapsed.Milliseconds >= this.scaleDuration.Milliseconds) {
                    this.animationState = ButtonAnimationState.Idle;
                }
                break;
        }

        // Make the button pulse
        this.scale.X += Math.cos(gameTime.TotalGameTime.TotalSeconds * 4) / 500;
        this.scale.Y += Math.sin(gameTime.TotalGameTime.TotalSeconds * 4) / 500;
    }

    public HandleInput(gameTime: GameTime) {
        if (this.screen.ScreenManager.Game.InputManager.LeftButtonPressed && this.rectangle.Contains(this.screen.ScreenManager.Game.InputManager.WorldPosition.X, this.screen.ScreenManager.Game.InputManager.WorldPosition.Y)) {
            this.pressed = true;
            this.animationState = ButtonAnimationState.Pressing;
            this.scaleTimeElapsed = TimeSpan.Zero;
        }

        if (this.pressed && !this.rectangle.Contains(this.screen.ScreenManager.Game.InputManager.WorldPosition.X, this.screen.ScreenManager.Game.InputManager.WorldPosition.Y)) {
            this.pressed = false;
            this.animationState = ButtonAnimationState.Releasing;
            this.scaleTimeElapsed = TimeSpan.Zero;
        }

        if (this.pressed && this.screen.ScreenManager.Game.InputManager.LeftButtonReleased && this.rectangle.Contains(this.screen.ScreenManager.Game.InputManager.WorldPosition.X, this.screen.ScreenManager.Game.InputManager.WorldPosition.Y)) {
            this.pressed = false;
            this.animationState = ButtonAnimationState.Releasing;
            this.scaleTimeElapsed = TimeSpan.Zero;
            this.OnSelect();
        }
    }

    private OnSelect() {
        if (this.Selected != null) {
            this.Selected();
        }
    }

    public Draw(gameTime: GameTime) {
        var drawTextColor: Color = this.textColor;
        var drawRectangleColor: Color = this.rectangleColor;

        if (this.pressed) {
            drawTextColor = Color.FromVector3(drawTextColor.ToVector3().MultiplyScalar(0.5));
            drawRectangleColor = Color.FromVector3(drawRectangleColor.ToVector3().MultiplyScalar(0.5));
        }

        this.screen.ScreenManager.Game.GraphicsManager.Draw(this.image, new Rectangle(this.rectangle.X - this.rectangle.Width * (this.scale.X - 1) / 2, this.rectangle.Y - this.rectangle.Height * (this.scale.Y - 1) / 2, this.rectangle.Width * this.scale.X, this.rectangle.Height * this.scale.Y), drawRectangleColor);
    }
}