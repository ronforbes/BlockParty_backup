<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlockPartyWebRole._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Block Party</title>
        <link href="Styles/Main.css" rel="stylesheet" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    </head>

    <body>
        <form runat="server">
            <div id="wrapper">
                <canvas id="canvas"></canvas>
            </div>

            <script src="Scripts/jquery-2.0.3.js"></script>
            <script src="Scripts/jquery.signalR-1.1.3.js"></script>
            <script src='<%: ResolveClientUrl("~/signalr/hubs") %>'></script>
            <script src="Client/Tween.js"></script>
            <script src="Client/AudioManager.js"></script>
            <script src="Client/FrameRateCounter.js"></script>
            <script src="Client/Rectangle.js"></script>
            <script src="Client/Viewport.js"></script>
            <script src="Client/GameScreen.js"></script>
            <script src="Client/Point.js"></script>
            <script src="Client/Vector2.js"></script>
            <script src="Client/GraphicsManager.js"></script>
            <script src="Client/InputManager.js"></script>
            <script src="Client/GameTime.js"></script>
            <script src="Client/Board.js"></script>
            <script src="Client/Block.js"></script>
            <script src="Client/TimeSpan.js"></script>
            <script src="Client/BlockParticle.js"></script>
            <script src="Client/BlockRain.js"></script>
            <script src="Client/MainMenuScreen.js"></script>
            <script src="Client/SplashScreen.js"></script>
            <script src="Client/ScreenManager.js"></script>
            <script src="Client/Game.js"></script>
            <script src="Client/Main.js"></script>

            <script>
                (function (i, s, o, g, r, a, m) {
                    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                        (i[r].q = i[r].q || []).push(arguments)
                    }, i[r].l = 1 * new Date(); a = s.createElement(o),
                    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
                })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

                ga('create', 'UA-44067201-2', 'omegaentertainment.net');
                ga('send', 'pageview');
            </script>
        </form>
    </body>
</html>
