using System.Collections.Generic;
using System.Linq;
using Reliability;

namespace DefaultNamespace
{
    public class MockCrowdInputReliability : ICrowdInputReliability
    {
        private int numberOfPlayers;
        private int numberOfCommands;

        public MockCrowdInputReliability(int numberOfPlayers, int numberOfCommands)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.numberOfCommands = numberOfCommands;
        }

        public int IssueCommands(int[] commands)
        {
            var commandFrequencies = new List<int>(Enumerable.Repeat(0, numberOfCommands));

            for (var i = 0; i < numberOfPlayers; i++)
            {
                commandFrequencies[commands[i]] += 1;
            }

            return commandFrequencies.Select((n, i) => (Number: n, Index: i)).Max().Index;
        }

        public List<float> GetPlayerReliabilities()
        {
            return Enumerable.Repeat(1f, numberOfPlayers).ToList();
        }
    }
}