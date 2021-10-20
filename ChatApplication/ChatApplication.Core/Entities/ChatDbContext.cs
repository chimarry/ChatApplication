using ChatApplication.Core.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Core.Entities
{
    public class ChatDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MaliciousAttackRecord> MaliciousAttackRecords { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new MaliciousAttackRecordConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
