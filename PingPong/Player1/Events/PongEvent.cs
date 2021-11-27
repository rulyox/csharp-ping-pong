namespace PingPong.Player1.Events
{
    public class PongEvent
    {
        public int Id { get; set; }

        public PongEvent(int id)
        {
            Id = id;
        }
    }
}
