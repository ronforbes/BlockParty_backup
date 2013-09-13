using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockPartyWindowsStore.Gameplay
{
    class BoardStats
    {
        Board board;

        public int Score = 0;
        public int Level = 1;
        public TimeSpan TimeElapsed = TimeSpan.Zero;
        public int BlocksMatched = 0;
        public int Combos = 0;
        public Dictionary<int, int> ComboBreakdown = new Dictionary<int, int>();
        public int Chains = 0;
        public Dictionary<int, int> ChainBreakdown = new Dictionary<int, int>();

        const int scoreBlockPop = 10;
        const int scoreBlockComboInterval = 100;
        const int scoreBlockChainInterval = 200;

        public BoardStats(Board board)
        {
            this.board = board;

            for (int comboStrength = 4; comboStrength < board.Rows * board.Columns; comboStrength++)
            {
                ComboBreakdown.Add(comboStrength, 0);
            }

            for (int chainLength = 2; chainLength < board.Rows * board.Columns / 3; chainLength++)
            {
                ChainBreakdown.Add(chainLength, 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            TimeElapsed += gameTime.ElapsedGameTime;

            Level = (int)MathHelper.Clamp(Score / 1000, 1, 99);
        }

        public void ScoreBlockPop()
        {
            Score += scoreBlockPop;
        }

        public void ScoreCombo(int matchingBlockCount)
        {
            Score += matchingBlockCount * scoreBlockComboInterval;
            Combos++;

            // Add the combo in the breakdown stats
            if (!ComboBreakdown.ContainsKey(matchingBlockCount))
            {
                ComboBreakdown.Add(matchingBlockCount, 0);
            }
            ComboBreakdown[matchingBlockCount]++;
        }

        public void ScoreChain(int chainCount)
        {
            Score += chainCount * scoreBlockChainInterval;
        }

        public void IncrementChainCounter(int chainCount)
        {
            Chains++;

            // Add to the chain breakdown
            if (chainCount > 1)
            {
                if (!ChainBreakdown.ContainsKey(chainCount))
                {
                    ChainBreakdown.Add(chainCount, 0);
                }
                ChainBreakdown[chainCount]++;
            }
        }
    }
}
