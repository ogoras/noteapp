﻿// <auto-generated />
using System;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220112135602_session")]
    partial class session
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Core.Domain.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<int?>("UserUid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserUid");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("Core.Domain.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Encrypted")
                        .HasColumnType("bit");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<bool?>("SharedPublically")
                        .HasColumnType("bit");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Core.Domain.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("NoteId")
                        .HasColumnType("int");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Core.Domain.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BioId")
                        .HasColumnType("int");

                    b.Property<int?>("ProfilePictureId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BioId")
                        .IsUnique()
                        .HasFilter("[BioId] IS NOT NULL");

                    b.HasIndex("ProfilePictureId")
                        .IsUnique()
                        .HasFilter("[ProfilePictureId] IS NOT NULL");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Core.Domain.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastActivity")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserUid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserUid");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LastLoginId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Uid");

                    b.HasIndex("LastLoginId")
                        .IsUnique()
                        .HasFilter("[LastLoginId] IS NOT NULL");

                    b.HasIndex("ProfileId")
                        .IsUnique()
                        .HasFilter("[ProfileId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NoteProfile", b =>
                {
                    b.Property<int>("NoteSharesId")
                        .HasColumnType("int");

                    b.Property<int>("ShareRecipientsId")
                        .HasColumnType("int");

                    b.HasKey("NoteSharesId", "ShareRecipientsId");

                    b.HasIndex("ShareRecipientsId");

                    b.ToTable("NoteProfile");
                });

            modelBuilder.Entity("PhotoProfile", b =>
                {
                    b.Property<int>("PhotoSharesId")
                        .HasColumnType("int");

                    b.Property<int>("ShareRecipientsId")
                        .HasColumnType("int");

                    b.HasKey("PhotoSharesId", "ShareRecipientsId");

                    b.HasIndex("ShareRecipientsId");

                    b.ToTable("PhotoProfile");
                });

            modelBuilder.Entity("Core.Domain.Login", b =>
                {
                    b.HasOne("Core.Domain.User", null)
                        .WithMany("UserLogins")
                        .HasForeignKey("UserUid");
                });

            modelBuilder.Entity("Core.Domain.Note", b =>
                {
                    b.HasOne("Core.Domain.Profile", "Owner")
                        .WithMany("Notes")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Core.Domain.Photo", b =>
                {
                    b.HasOne("Core.Domain.Note", "Note")
                        .WithMany("AttachedPhotos")
                        .HasForeignKey("NoteId");

                    b.HasOne("Core.Domain.Profile", "Owner")
                        .WithMany("Photos")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Note");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Core.Domain.Profile", b =>
                {
                    b.HasOne("Core.Domain.Note", "Bio")
                        .WithOne()
                        .HasForeignKey("Core.Domain.Profile", "BioId");

                    b.HasOne("Core.Domain.Photo", "ProfilePicture")
                        .WithOne()
                        .HasForeignKey("Core.Domain.Profile", "ProfilePictureId");

                    b.Navigation("Bio");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("Core.Domain.Session", b =>
                {
                    b.HasOne("Core.Domain.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserUid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.HasOne("Core.Domain.Login", "LastLogin")
                        .WithOne("User")
                        .HasForeignKey("Core.Domain.User", "LastLoginId");

                    b.HasOne("Core.Domain.Profile", "Profile")
                        .WithOne("User")
                        .HasForeignKey("Core.Domain.User", "ProfileId");

                    b.Navigation("LastLogin");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("NoteProfile", b =>
                {
                    b.HasOne("Core.Domain.Note", null)
                        .WithMany()
                        .HasForeignKey("NoteSharesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Profile", null)
                        .WithMany()
                        .HasForeignKey("ShareRecipientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhotoProfile", b =>
                {
                    b.HasOne("Core.Domain.Photo", null)
                        .WithMany()
                        .HasForeignKey("PhotoSharesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Profile", null)
                        .WithMany()
                        .HasForeignKey("ShareRecipientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Login", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Domain.Note", b =>
                {
                    b.Navigation("AttachedPhotos");
                });

            modelBuilder.Entity("Core.Domain.Profile", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("Photos");

                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Navigation("Sessions");

                    b.Navigation("UserLogins");
                });
#pragma warning restore 612, 618
        }
    }
}
