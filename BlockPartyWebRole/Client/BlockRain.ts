/// <reference path="BlockParticle.ts" />
/// <reference path="GameScreen.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="TimeSpan.ts" />

class BlockRain {
    public Screen: GameScreen;
    public Images: Array<HTMLImageElement> = new Array<HTMLImageElement>();

    private blockParticles: Array<BlockParticle> = new Array<BlockParticle>();
    private blocksToRemove: Array<BlockParticle> = new Array<BlockParticle>();
    private spawnDelayTimeElapsed: TimeSpan = TimeSpan.Zero;
    private spawnDelayDuration: TimeSpan = TimeSpan.FromMilliseconds(0);

    constructor(screen: GameScreen) {
        this.Screen = screen;
    }

    public LoadContent() {
        var blockRedImage: HTMLImageElement = new Image();
        var blockGreenImage: HTMLImageElement = new Image();
        var blockBlueImage: HTMLImageElement = new Image();
        var blockCyanImage: HTMLImageElement = new Image();
        var blockMagentaImage: HTMLImageElement = new Image();
        var blockYellowImage: HTMLImageElement = new Image();

        blockRedImage.src = "Client/Images/BlockRed.png";
        blockGreenImage.src = "Client/Images/BlockGreen.png";
        blockBlueImage.src = "Client/Images/BlockBlue.png";
        blockCyanImage.src = "Client/Images/BlockCyan.png";
        blockMagentaImage.src = "Client/Images/BlockMagenta.png";
        blockYellowImage.src = "Client/Images/BlockYellow.png";
        
        this.Images.push(blockRedImage);
        this.Images.push(blockGreenImage);
        this.Images.push(blockBlueImage);
        this.Images.push(blockCyanImage);
        this.Images.push(blockMagentaImage);
        this.Images.push(blockYellowImage);
    }

    public Update(gameTime: GameTime) {
        this.spawnDelayTimeElapsed = new TimeSpan(this.spawnDelayTimeElapsed.TotalMilliseconds + gameTime.ElapsedGameTime.TotalMilliseconds);

        if (this.spawnDelayTimeElapsed.TotalMilliseconds >= this.spawnDelayDuration.TotalMilliseconds) {
            this.blockParticles.push(new BlockParticle(this));

            this.spawnDelayTimeElapsed = TimeSpan.Zero;
        }

        this.blocksToRemove.splice(0, this.blocksToRemove.length);

        this.blockParticles.forEach(function (blockParticle: BlockParticle, index: number, blockParticles: Array<BlockParticle>) {
            if (blockParticle.Active) {
                blockParticle.Update(gameTime);
            }
            else {
                this.blocksToRemove.push(blockParticle);
            }
        }, this);

        this.blocksToRemove.forEach(function (blockParticle: BlockParticle, index: number, blocksToRemove: Array<BlockParticle>) {
            var index = this.blockParticles.indexOf(blockParticle);
            this.blockParticles.splice(index, 1);
        }, this);
    }

    public Draw(gameTime: GameTime) {
        this.blockParticles.forEach(function(blockParticle: BlockParticle, index: number, blockParticles: Array < BlockParticle>) {
            blockParticle.Draw(gameTime);
        });
    }
}