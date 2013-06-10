using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockPartyWebRole.Server
{
    public class Game
    {
        readonly static Lazy<Game> instance = new Lazy<Game>(() => new Game());

        public static Game Instance
        {
            get { return instance.Value; }
        }
    }
}