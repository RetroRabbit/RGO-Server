﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RGO.Repository;

#nullable disable

namespace RGO.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RGO.Repository.Entities.Certifications", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Certifications");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Events", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("endDate");

                    b.Property<int>("EventType")
                        .HasColumnType("integer")
                        .HasColumnName("eventType");

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer")
                        .HasColumnName("groupId");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("startDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("UserType")
                        .HasColumnType("integer")
                        .HasColumnName("userType");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Projects", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Social", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeWars")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("codeWars");

                    b.Property<string>("Discord")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discord");

                    b.Property<string>("GitHub")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gitHub");

                    b.Property<string>("LinkedIn")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("linkedIn");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Social");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Stacks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("StackType")
                        .HasColumnType("integer")
                        .HasColumnName("stackType");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("Stacks");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Bio");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstName");

                    b.Property<int?>("GradGroupId")
                        .HasColumnType("integer")
                        .HasColumnName("gradGroupId");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("joinDate");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastName");

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("Level");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Phone");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("GradGroupId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("RGO.Repository.Entities.UserStacks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BackendId")
                        .HasColumnType("integer")
                        .HasColumnName("backendId");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createDate");

                    b.Property<int>("DatabaseId")
                        .HasColumnType("integer")
                        .HasColumnName("databaseId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("FrontendId")
                        .HasColumnType("integer")
                        .HasColumnName("frontendId");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("BackendId");

                    b.HasIndex("DatabaseId");

                    b.HasIndex("FrontendId");

                    b.HasIndex("UserId");

                    b.ToTable("UserStacks");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Workshop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer")
                        .HasColumnName("eventId");

                    b.Property<string>("Presenter")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("presenter");

                    b.Property<bool>("Viewable")
                        .HasColumnType("boolean")
                        .HasColumnName("viewable");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Workshop");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Certifications", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", "User")
                        .WithMany("UserCertifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Events", b =>
                {
                    b.HasOne("RGO.Repository.Entities.UserGroup", "UserGroup")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Projects", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Skill", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", "User")
                        .WithMany("Skills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Social", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", "User")
                        .WithMany("Socials")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.HasOne("RGO.Repository.Entities.UserGroup", "UserGroup")
                        .WithMany()
                        .HasForeignKey("GradGroupId");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("RGO.Repository.Entities.UserStacks", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Stacks", "BackendUserStack")
                        .WithMany()
                        .HasForeignKey("BackendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.Stacks", "DatabaseUserStack")
                        .WithMany()
                        .HasForeignKey("DatabaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.Stacks", "FrontendUserStack")
                        .WithMany()
                        .HasForeignKey("FrontendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BackendUserStack");

                    b.Navigation("DatabaseUserStack");

                    b.Navigation("FrontendUserStack");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Workshop", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Events", "Events")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Events");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.Navigation("Skills");

                    b.Navigation("Socials");

                    b.Navigation("UserCertifications");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
