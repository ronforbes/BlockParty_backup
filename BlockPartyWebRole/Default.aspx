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

            <script src="Scripts/jquery-2.0.2.js"></script>
            <script src="Scripts/jquery.signalR-1.1.2.js"></script>
            <script src='<%: ResolveClientUrl("~/signalr/hubs") %>'></script>
            <script src="Client/Vector2.js"></script>
            <script src="Client/Graphics.js"></script>
            <script src="Client/Mouse.js"></script>
            <script src="Client/Timer.js"></script>
            <script src="Client/Block.js"></script>
            <script src="Client/Game.js"></script>
            <script src="Client/Main.js"></script>
        </form>
    </body>
</html>
