/// <reference path="BlockParticle.ts" />
/// <reference path="GameScreen.ts" />
/// <reference path="GameTime.ts" />
/// <reference path="TimeSpan.ts" />
var BlockRain = (function () {
    function BlockRain(screen) {
        this.Images = new Array();
        this.blockParticles = new Array();
        this.blocksToRemove = new Array();
        this.spawnDelayTimeElapsed = TimeSpan.Zero;
        this.spawnDelayDuration = TimeSpan.FromMilliseconds(0);
        this.Screen = screen;
    }
    BlockRain.prototype.LoadContent = function () {
        var blockRedImage = new Image();
        var blockGreenImage = new Image();
        var blockBlueImage = new Image();
        var blockCyanImage = new Image();
        var blockMagentaImage = new Image();
        var blockYellowImage = new Image();

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
    };

    BlockRain.prototype.Update = function (gameTime) {
        this.spawnDelayTimeElapsed = new TimeSpan(this.spawnDelayTimeElapsed.TotalMilliseconds + gameTime.ElapsedGameTime.TotalMilliseconds);

        if (this.spawnDelayTimeElapsed.TotalMilliseconds >= this.spawnDelayDuration.TotalMilliseconds) {
            this.blockParticles.push(new BlockParticle(this));

            this.spawnDelayTimeElapsed = TimeSpan.Zero;
        }

        this.blocksToRemove.splice(0, this.blocksToRemove.length);

        this.blockParticles.forEach(function (blockParticle, index, blockParticles) {
            if (blockParticle.Active) {
                blockParticle.Update(gameTime);
            } else {
                this.blocksToRemove.push(blockParticle);
            }
        }, this);

        this.blocksToRemove.forEach(function (blockParticle, index, blocksToRemove) {
            var index = this.blockParticles.indexOf(blockParticle);
            this.blockParticles.splice(index, 1);
        }, this);
    };

    BlockRain.prototype.Draw = function (gameTime) {
        this.blockParticles.forEach(function (blockParticle, index, blockParticles) {
            blockParticle.Draw(gameTime);
        });
    };
    return BlockRain;
})();
