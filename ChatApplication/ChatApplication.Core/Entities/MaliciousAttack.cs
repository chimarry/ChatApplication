using System;
using System.ComponentModel;

namespace ChatApplication.Core.Entities
{
    public enum MaliciousAttack
    {
        /// <summary>
        /// XSS attack
        /// </summary>
        XSS,

        /// <summary>
        /// SQL Injection attack
        /// </summary>
        SQLInjection,

        /// <summary>
        /// Parameter Tampering attack
        /// </summary>
        ParameterTampering,

        /// <summary>
        /// Denial of Service
        /// </summary>
        DoS,

        /// <summary>
        /// Man in The Middle
        /// </summary>
        ManInTheMiddle
    }

    public static class MaliciousAttackExtensions
    {
        private const string xssMessage = "Attempt to do XSS attack was detected.";
        private const string sqlInjectionMessage = "Attempt to do SQL injection attack was detected.";
        private const string manInTheMiddleMessage = "Man in The Middle attack was detected.";
        private const string parameterTamperingMessage = "Parameter tampering was detected.";
        private const string dosMessage = "Denial of Service was detected";

        public static string GetMessage(this MaliciousAttack attack)
            => attack switch
            {
                MaliciousAttack.XSS => xssMessage,
                MaliciousAttack.SQLInjection => sqlInjectionMessage,
                MaliciousAttack.DoS => dosMessage,
                MaliciousAttack.ManInTheMiddle => manInTheMiddleMessage,
                MaliciousAttack.ParameterTampering => parameterTamperingMessage,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}