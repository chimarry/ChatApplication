using System;
using System.Collections.Generic;
using System.Text;
using ChatApplication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatApplication.Core.DTO
{
    public class ChatDTO
    {
        public string Username { get; set; }

        public List<OutputMessageDTO> Messages { get; set; }
    }
}
