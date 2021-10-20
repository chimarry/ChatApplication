using System;

namespace ChatApplication.Core.DTO
{
    public class OutputMessageDTO : MessageDTO
    {
        public int MessageId { get; set; }

        public DateTime SentOn { get; set; }
    }
}
