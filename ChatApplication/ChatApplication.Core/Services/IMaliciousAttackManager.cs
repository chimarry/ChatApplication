using ChatApplication.Core.Entities;
using ChatApplication.Core.ErrorHandling;
using System;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public interface IMaliciousAttackManager
    {
        Task<ResultMessage<bool>> PreventXss(string text, string username = null);

        Task<ResultMessage<bool>> PreventSqlInjection(string text, string username = null);

        Task<ResultMessage<bool>> PreventParameterTampering<T>(Func<T, T, bool> condition, T x, T y, string username = null);

        Task<ResultMessage<bool>> ProcessManInTheMiddleAttack(string details);

        Task LogMaliciousAttack(MaliciousAttack attack, string message = null, string username = null);
    }
}
