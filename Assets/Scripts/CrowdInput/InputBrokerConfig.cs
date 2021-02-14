namespace DefaultNamespace
{
    public class InputBrokerConfig
    {
        public int numberOfPlayers;
        public int numberOfCommands;
        public float reliabilityCoefficient;
        public float agreementThreshold;
        public float inputTimeToLive;
        
        public InputBrokerConfig(int numberOfPlayers, int numberOfCommands, float reliabilityCoefficient,
            float agreementThreshold, float inputTimeToLive)
        {
            this.numberOfPlayers = numberOfPlayers;
            this.numberOfCommands = numberOfCommands;
            this.reliabilityCoefficient = reliabilityCoefficient;
            this.agreementThreshold = agreementThreshold;
            this.inputTimeToLive = inputTimeToLive;
        }
    }
}