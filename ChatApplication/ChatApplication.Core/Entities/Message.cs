using System;

namespace ChatApplication.Core.Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string Text { get; set; }

        public DateTime SentOn { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }
    }
}
