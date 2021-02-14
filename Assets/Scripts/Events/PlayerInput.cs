namespace DefaultNamespace.Events
{
    public class PlayerInput
    {
        public int playerId { get; }
        public string playerNickname { get; }
        public int inputId { get; }
        public float timestamp { get;  }
        
        public PlayerInput(int playerId, string playerNickname, int inputId, float timestamp)
        {
            this.playerId = playerId;
            this.playerNickname = playerNickname;
            this.inputId = inputId;
            this.timestamp = timestamp;
        }
    }
}