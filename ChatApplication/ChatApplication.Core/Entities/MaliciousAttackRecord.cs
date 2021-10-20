using System;

namespace ChatApplication.Core.Entities
{
    public class MaliciousAttackRecord
    {
        public int MaliciousAttackRecordId { get; set; }

        public string Details { get; set; }

        public string Attacker { get; set; }

        public MaliciousAttack Type { get; set; }

        public DateTime AttemptedOn { get; set; }
    }
}
