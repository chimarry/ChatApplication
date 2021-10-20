using ChatApplication.Core.DTO;
using ChatApplication.Core.ErrorHandling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public interface IChatService
    {
        Task<ResultMessage<OutputMessageDTO>> Send(MessageDTO message, string currentUser);

        Task<List<ChatDTO>> ReadChat(string username, string currentUser);
    }
}
