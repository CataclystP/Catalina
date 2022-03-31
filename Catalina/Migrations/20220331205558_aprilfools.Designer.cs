﻿// <auto-generated />
using System;
using Catalina.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catalina.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220331205558_aprilfools")]
    partial class aprilfools
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("Catalina.Database.Models.GuildProperty", b =>
                {
                    b.Property<ulong>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("AdminRoleIDsSerialised")
                        .HasColumnType("longtext");

                    b.Property<string>("CommandChannelsSerialised")
                        .HasColumnType("longtext");

                    b.Property<ulong?>("DefaultRole")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Prefix")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("GuildProperties");
                });

            modelBuilder.Entity("Catalina.Database.Models.Reaction", b =>
                {
                    b.Property<ulong>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("ChannelID")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("EmojiName")
                        .HasColumnType("longtext");

                    b.Property<ulong>("GuildID")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("MessageID")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("RoleID")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("ID");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Catalina.Database.Models.Response", b =>
                {
                    b.Property<ulong>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<int>("Bonus")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<ulong>("GuildID")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Trigger")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("Catalina.Database.Models.Role", b =>
                {
                    b.Property<ulong>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("guildID")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("roleID")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("userID")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
