/// <reference path="Vector2.ts" />
/// <reference path="Rectangle.ts" />
/// <reference path="BlockRain.ts" />

class BlockParticle {
    public Active: boolean = true;

    private blockRain: BlockRain;
    private rectangle: Rectangle;
    private velocity: Vector2;
    private acceleration: Vector2;
    private image: HTMLImageElement;

    constructor(blockRain: BlockRain) {
        this.blockRain = blockRain;

        var width: number = Math.random() * (this.blockRain.Screen.ScreenManager.Game.WorldViewport.Width > this.blockRain.Screen.ScreenManager.Game.WorldViewport.Height ?
            this.blockRain.Screen.ScreenManager.Game.WorldViewport.Height / 5 :
            this.blockRain.Screen.ScreenManager.Game.WorldViewport.Width / 5);
        var height = width;
        var x = Math.random() * this.blockRain.Screen.ScreenManager.Game.WorldViewport.Width;
        var y = -1 * height;

        this.rectangle = new Rectangle(x, y, width, height);
        this.velocity = Vector2.Zero;
        this.acceleration = new Vector2(0, 0.25);
        this.image = this.blockRain.Images[Math.floor(Math.random() * this.blockRain.Images.length)];
    }

    public Update(gameTime: GameTime) {
        if (this.Active) {
            this.velocity = this.velocity.AddVector(this.acceleration);
            this.rectangle.Y += this.velocity.Y;

            if (this.rectangle.Y >= this.blockRain.Screen.ScreenManager.Game.WorldViewport.Height) {
                this.Active = false;
            }
        }
    }

    public Draw(gameTime: GameTime) {
        if (this.Active) {
            this.blockRain.Screen.ScreenManager.Game.GraphicsManager.Draw(this.image, this.rectangle, "white");
        }
    }
}