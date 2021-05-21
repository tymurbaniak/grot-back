﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagement.DBContexts;

namespace UserManagement.Migrations
{
    [DbContext(typeof(UsersDBContext))]
    [Migration("20210515201740_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("UserManagement.DBModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<int?>("UserGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Users");

                    b.HasIndex("FirstName")
                        .HasName("Idx_FirstName");

                    b.HasIndex("LastName")
                        .HasName("Idx_LastName");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("Idx_Name");

                    b.HasIndex("UserGroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserManagement.DBModels.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK_UserGroups");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("Idx_Name");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("UserManagement.ViewModels.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("nvarchar(254)");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(254)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_RefreshToken");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("UserManagement.DBModels.User", b =>
                {
                    b.HasOne("UserManagement.DBModels.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupId")
                        .HasConstraintName("FK_Users_UserGroups")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("UserManagement.ViewModels.RefreshToken", b =>
                {
                    b.HasOne("UserManagement.DBModels.User", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}