using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Core.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(31);
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Salt).IsRequired();

            builder.OwnsMany(x => x.RefreshTokens, tokenBuilder =>
            {
                tokenBuilder.ToTable(nameof(RefreshToken));
                tokenBuilder.Property(token => token.ExpiresOn).IsRequired();
                tokenBuilder.Property(token => token.CreatedOn).IsRequired();
                tokenBuilder.Property(token => token.Token).IsRequired();
                tokenBuilder.WithOwner().HasForeignKey(token => token.UserId);
                tokenBuilder.HasKey(token => token.RefreshTokenId);
            });

            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
