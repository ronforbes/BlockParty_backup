using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Gameplay
{
    class CelebrationManager
    {        
        public List<Celebration> Celebrations = new List<Celebration>();

        Board board;
        List<Celebration> celebrationsToRemove = new List<Celebration>();

        public CelebrationManager(Board board)
        {
            this.board = board;
        }

        public void Add(string text, int row, int column)
        {
            Celebrations.Add(new Celebration(board, text, row, column));
        }

        public void Update(GameTime gameTime)
        {
            celebrationsToRemove.Clear();

            foreach (Celebration celebration in Celebrations)
            {
                if (celebration.Active)
                {
                    celebration.Update(gameTime);
                }
                else
                {
                    celebrationsToRemove.Add(celebration);
                }
            }

            foreach (Celebration celebration in celebrationsToRemove)
            {
                Celebrations.Remove(celebration);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Celebration celebration in Celebrations)
            {
                celebration.Draw(gameTime);
            }
        }
    }
}
