using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Gameplay
{
    public class Score
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
