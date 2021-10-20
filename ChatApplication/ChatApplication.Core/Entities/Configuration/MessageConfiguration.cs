using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApplication.Core.Entities.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable(nameof(Message));
            builder.Property(x => x.Text).IsRequired().HasMaxLength(1023);
            builder.Property(x => x.SentOn).IsRequired();

            builder.HasOne(x => x.Receiver)
                   .WithMany(x => x.ReceivedMessages)
                   .HasForeignKey(x => x.ReceiverId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();

            builder.HasOne(x => x.Sender)
                   .WithMany(x => x.SentMessages)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();

            builder.HasIndex(x => new { x.ReceiverId, x.SenderId, x.SentOn }).IsUnique();
        }
    }
}
