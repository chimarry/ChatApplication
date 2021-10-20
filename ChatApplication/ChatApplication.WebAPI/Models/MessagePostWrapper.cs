namespace ChatApplication.WebAPI.Models
{
    public class MessagePostWrapper
    {
        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string Text { get; set; }
    }
}
