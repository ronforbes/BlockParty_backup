using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BlockPartyWebRole.Server
{
    public class GameHub : Hub
    {
        readonly Game game;

        public GameHub() : this(Game.Instance) { }

        public GameHub(Game game)
        {
            this.game = game;
        }
    }
}