namespace DefaultNamespace.Events
{
    public class PlayerInput
    {
        public int playerId { get; }
        public int inputId { get; }

        public PlayerInput(int playerId, int inputId)
        {
            this.playerId = playerId;
            this.inputId = inputId;
        }
    }
}