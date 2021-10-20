using ChatApplication.Core.AutoMapper;
using ChatApplication.Core.DTO;
using ChatApplication.Core.Entities;
using ChatApplication.Core.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IMaliciousAttackManager maliciousAttackManager;
        private readonly ChatDbContext context;

        public ChatService(IMaliciousAttackManager maliciousAttackDetector, ChatDbContext context)
        {
            this.maliciousAttackManager = maliciousAttackDetector;
            this.context = context;
        }

        public async Task<List<ChatDTO>> ReadChat(string username, string currentUser)
        {
            if (username != currentUser)
            {
                await maliciousAttackManager.LogMaliciousAttack(MaliciousAttack.ParameterTampering,
                    MaliciousAttack.ParameterTampering.GetMessage(), currentUser);
                throw new ForbiddenAccessException();
            }

            List<ChatDTO> chatList = await context.Users.Where(us => us.Username != username)
                .Select(
                    us => new ChatDTO()
                    {
                        Username = us.Username
                    }).ToListAsync();

            foreach (ChatDTO chat in chatList)
            {
                chat.Messages = await context.Messages.Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Where(x => (x.Sender.Username == username
                                    && x.Receiver.Username == chat.Username)
                                || (x.Sender.Username == chat.Username
                                    && x.Receiver.Username == username))
                    .OrderBy(x => x.SentOn)
                    .Select(msg => MapMessage(msg, null))
                    .ToListAsync();
            }

            return chatList;
        }

        public async Task<ResultMessage<OutputMessageDTO>> Send(MessageDTO message, string currentUser)
        {
            await maliciousAttackManager.PreventXss(message.Text, currentUser);
            await maliciousAttackManager.PreventSqlInjection(message.Text, currentUser);
            await maliciousAttackManager.PreventParameterTampering((x, y) => x != y, message.SenderUsername, currentUser, currentUser);

            int? senderId = (await context.Users.SingleOrDefaultAsync(x => x.Username == message.SenderUsername))?.UserId;
            int? receiverId = (await context.Users.SingleOrDefaultAsync(x => x.Username == message.ReceiverUsername))?.UserId;

            if (!senderId.HasValue || !receiverId.HasValue)
                return new ResultMessage<OutputMessageDTO>(OperationStatus.NotFound);

            Message entity = Mapping.Mapper.Map<Message>(message);
            entity.SenderId = senderId.Value;
            entity.ReceiverId = receiverId.Value;
            entity.SentOn = DateTime.UtcNow;

            await context.Messages.AddAsync(entity);
            await context.SaveChangesAsync();

            return new ResultMessage<OutputMessageDTO>(MapMessage(entity, message));
        }

        private static OutputMessageDTO MapMessage(Message entity, MessageDTO inputMessage = null)
        {
            OutputMessageDTO outputMessage = Mapping.Mapper.Map<OutputMessageDTO>(entity);
            if (inputMessage != null)
            {
                outputMessage.ReceiverUsername = inputMessage.ReceiverUsername;
                outputMessage.SenderUsername = inputMessage.SenderUsername;
            }
            else
            {
                outputMessage.ReceiverUsername = entity.Receiver.Username;
                outputMessage.SenderUsername = entity.Sender.Username;
            }
            return outputMessage;
        }
    }
}
