namespace UserActorService.Interfaces
{
    public class Message
    {
        public  string Text { get; }

        public Message() { }
        public Message(string message)
        {
            Text = message;
        }
    }

    public class Acknowledgement
    {
        public string AckMessage { get; }

        public Acknowledgement() { }
        public Acknowledgement(string ackMessage)
        {
            AckMessage = ackMessage;
        }
    }
}