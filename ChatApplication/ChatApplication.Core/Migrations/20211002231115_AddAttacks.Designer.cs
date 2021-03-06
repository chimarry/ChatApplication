// <auto-generated />
using System;
using ChatApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatApplication.Core.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20211002231115_AddAttacks")]
    partial class AddAttacks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChatApplication.Core.Entities.MaliciousAttackRecord", b =>
                {
                    b.Property<int>("MaliciousAttackRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AttemptedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Details")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MaliciousAttackRecordId");

                    b.HasIndex("UserId");

                    b.ToTable("MaliciousAttacks");
                });

            modelBuilder.Entity("ChatApplication.Core.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("varchar(1023) CHARACTER SET utf8mb4")
                        .HasMaxLength(1023);

                    b.HasKey("MessageId");

                    b.HasIndex("SenderId");

                    b.HasIndex("ReceiverId", "SenderId", "SentOn")
                        .IsUnique();

                    b.ToTable("Message");
                });

            modelBuilder.Entity("ChatApplication.Core.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<string>("Otp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OtpApiKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("OtpExpiresOn")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(31) CHARACTER SET utf8mb4")
                        .HasMaxLength(31);

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("ChatApplication.Core.Entities.MaliciousAttackRecord", b =>
                {
                    b.HasOne("ChatApplication.Core.Entities.User", "User")
                        .WithMany("MaliciousActivities")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ChatApplication.Core.Entities.Message", b =>
                {
                    b.HasOne("ChatApplication.Core.Entities.User", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApplication.Core.Entities.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ChatApplication.Core.Entities.User", b =>
                {
                    b.OwnsMany("ChatApplication.Core.Entities.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<int>("RefreshTokenId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("CreatedByIp")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnType("datetime(6)");

                            b1.Property<DateTime>("ExpiresOn")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("ReplacedByToken")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("RevokedByIp")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<DateTime?>("RevokedOn")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.HasKey("RefreshTokenId");

                            b1.HasIndex("UserId");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
