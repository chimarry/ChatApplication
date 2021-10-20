using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Core.Entities.Configuration
{
    public class MaliciousAttackRecordConfiguration : IEntityTypeConfiguration<MaliciousAttackRecord>
    {
        public void Configure(EntityTypeBuilder<MaliciousAttackRecord> builder)
        {
            builder.ToTable(nameof(MaliciousAttackRecord));
        }
    }
}
