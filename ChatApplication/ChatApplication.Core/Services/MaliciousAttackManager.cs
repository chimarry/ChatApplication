using ChatApplication.Core.Entities;
using ChatApplication.Core.ErrorHandling;
using ChatApplication.Core.Util;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatApplication.Core.Services
{
    public class MaliciousAttackManager : IMaliciousAttackManager
    {
        // Inject logger
        private readonly ChatDbContext context;

        private static readonly Regex xssRegex = new Regex(@"(\b)(on\S+)(\s*)=|javascript|alert|console\.|\.\$|<(|\/|[^\/>][^>]+|\/[^>][^>]+)>");
        private static readonly Regex[] sqlInjectionRegex =
            new Regex[]{
                 new Regex(@"\b\s*(?i)(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}\s*|\s*INSERT( +INTO){0,1}\s*|MERGE|SELECT|UPDATE|UNION( +ALL){0,1}\s*)(?-i)\s*\b\s*", RegexOptions.Compiled| RegexOptions.IgnoreCase),
                 new Regex(@"\s*'\s*(''|[^'])*") };

        public MaliciousAttackManager(ChatDbContext context)
        {
            this.context = context;
        }

        public async Task<ResultMessage<bool>> PreventParameterTampering<T>(Func<T, T, bool> condition, T x, T y, string username = null)
        {
            if (!condition(x, y))
                return new ResultMessage<bool>(OperationStatus.Success);

            MaliciousAttack detectedAttack = MaliciousAttack.ParameterTampering;

            await LogAttackToDatabase(detectedAttack.GetMessage(), detectedAttack, username);
            throw new MaliciousAttackException(MaliciousAttack.ParameterTampering);
        }

        public async Task<ResultMessage<bool>> PreventXss(string text, string username = null)
        {
            if (!RegexUtil.IsMatch(text, xssRegex))
                return new ResultMessage<bool>(OperationStatus.Success);

            MaliciousAttack detectedAttack = MaliciousAttack.XSS;

            await LogAttackToDatabase(detectedAttack.GetMessage(), detectedAttack, username);
            throw new MaliciousAttackException(detectedAttack);
        }

        public async Task<ResultMessage<bool>> PreventSqlInjection(string text, string username = null)
        {
            if (!sqlInjectionRegex.Any(sqlIn => RegexUtil.IsMatch(text, sqlIn)))
                return new ResultMessage<bool>(OperationStatus.Success);

            MaliciousAttack detectedAttack = MaliciousAttack.SQLInjection;

            await LogAttackToDatabase(detectedAttack.GetMessage(), detectedAttack, username);
            throw new MaliciousAttackException(detectedAttack);
        }

        public async Task<ResultMessage<bool>> ProcessManInTheMiddleAttack(string details)
        {
            MaliciousAttack detectedAttack = MaliciousAttack.ManInTheMiddle;

            await LogAttackToDatabase(detectedAttack.GetMessage(), detectedAttack);
            throw new MaliciousAttackException(detectedAttack);
        }

        public async Task LogMaliciousAttack(MaliciousAttack attack, string message = null, string username = null)
        {
            await LogAttackToDatabase(message ?? attack.GetMessage(), attack, username);
        }

        private async Task LogAttackToDatabase(string details, MaliciousAttack attack, string username = null)
        {
            MaliciousAttackRecord maliciousAttack = new MaliciousAttackRecord()
            {
                Details = details,
                Type = attack,
                AttemptedOn = DateTime.UtcNow,
                Attacker = username
            };

            await context.MaliciousAttackRecords.AddAsync(maliciousAttack);
            await context.SaveChangesAsync();
        }
    }
}
