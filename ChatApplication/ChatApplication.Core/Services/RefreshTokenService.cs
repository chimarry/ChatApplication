using ChatApplication.Core.AutoMapper;
using ChatApplication.Core.DTO;
using ChatApplication.Core.Entities;
using ChatApplication.Core.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ChatDbContext context;
        private readonly IMaliciousAttackManager maliciousAttackManager;

        public RefreshTokenService(ChatDbContext context, IMaliciousAttackManager maliciousAttackManager)
        {
            this.context = context;
            this.maliciousAttackManager = maliciousAttackManager;
        }

        public async Task<ResultMessage<bool>> RevokeRefreshToken(string token, string ipAddress)
        {
            User user = await context.Users.Include(x => x.RefreshTokens)
                                           .SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));
            if (user != null)
            {
                RefreshToken refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);
                if (refreshToken.IsActive())
                {
                    refreshToken.RevokedOn = DateTime.UtcNow;
                    refreshToken.RevokedByIp = ipAddress;
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                    return new ResultMessage<bool>(true);
                }
            }

            // Man in the middle was detected
            await maliciousAttackManager.LogMaliciousAttack(MaliciousAttack.ManInTheMiddle, token);
            throw new RefreshTokenException();
        }

        public async Task<ResultMessage<RefreshTokenDTO>> SaveRefreshToken(RefreshTokenDTO refreshToken)
        {
            User user = await context.Users.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId);
            if (user == null)
                await maliciousAttackManager.ProcessManInTheMiddleAttack(refreshToken.UserId.ToString());

            RefreshToken refreshTokenEntity = Mapping.Mapper.Map<RefreshToken>(refreshToken);

            user.RefreshTokens.Add(refreshTokenEntity);
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return new ResultMessage<RefreshTokenDTO>(Mapping.Mapper.Map<RefreshTokenDTO>(refreshTokenEntity));
        }

        public async Task<ResultMessage<RefreshTokenDTO>> UpdateRefreshToken(string oldToken, RefreshTokenDTO newToken)
        {
            User user = await context.Users.Include(x => x.RefreshTokens)
                                           .SingleOrDefaultAsync(x => x.RefreshTokens.Any(token => token.Token == oldToken));
            if (user == null)
                await maliciousAttackManager.ProcessManInTheMiddleAttack(oldToken);

            RefreshToken refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == oldToken);

            if (refreshToken.RevokedOn != null)
                throw new UnauthorizedException();

            if (!refreshToken.IsActive())
                await maliciousAttackManager.ProcessManInTheMiddleAttack(oldToken);

            refreshToken.ReplacedByToken = newToken.Token;
            refreshToken.RevokedOn = DateTime.UtcNow;
            refreshToken.RevokedByIp = newToken.RevokedByIp;

            RefreshToken newRefreshTokenEntity = Mapping.Mapper.Map<RefreshToken>(newToken);
            user.RefreshTokens.Add(newRefreshTokenEntity);
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return new ResultMessage<RefreshTokenDTO>(Mapping.Mapper.Map<RefreshTokenDTO>(newRefreshTokenEntity));
        }
    }
}
