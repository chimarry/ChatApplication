using ChatApplication.Core.DTO;
using ChatApplication.Core.ErrorHandling;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public interface IRefreshTokenService
    {
        Task<ResultMessage<RefreshTokenDTO>> SaveRefreshToken(RefreshTokenDTO refreshToken);

        Task<ResultMessage<bool>> RevokeRefreshToken(string token, string ipAddress);

        Task<ResultMessage<RefreshTokenDTO>> UpdateRefreshToken(string oldToken, RefreshTokenDTO newToken);
    }
}
