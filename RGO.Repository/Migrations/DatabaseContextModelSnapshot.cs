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

                    b.Property<int?>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("userId");

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

                    b.Property<int>("groupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("groupId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Field", b =>
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

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("errorMessage");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("label");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean")
                        .HasColumnName("required");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<int>("formId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("formId");

                    b.ToTable("Field");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("endDate");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("startDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("groupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("groupId");

                    b.ToTable("Form");
                });

            modelBuilder.Entity("RGO.Repository.Entities.FormSubmit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createDate");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("formId")
                        .HasColumnType("integer");

                    b.Property<int?>("formSubmitId")
                        .HasColumnType("integer");

                    b.Property<string>("rejectionReason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("rejectionReason");

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("formId");

                    b.HasIndex("formSubmitId");

                    b.HasIndex("userId");

                    b.ToTable("FormSubmit");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Input", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateDate")
                        .HasColumnType("integer")
                        .HasColumnName("createDate");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.Property<int>("fieldId")
                        .HasColumnType("integer");

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("fieldId");

                    b.HasIndex("userId");

                    b.ToTable("input");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.Property<int>("fieldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("fieldId");

                    b.ToTable("options");
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

                    b.Property<int?>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("projects");
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

                    b.Property<int?>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("skill", t =>
                        {
                            t.Property("userId")
                                .HasColumnName("userId1");
                        });
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

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("social");
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

                    b.ToTable("stacks");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstName");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("joinDate");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastName");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<int?>("gradGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("gradGroupId");

                    b.ToTable("users");
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

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createDate");

                    b.Property<int>("backendId")
                        .HasColumnType("integer");

                    b.Property<int>("databaseId")
                        .HasColumnType("integer");

                    b.Property<int>("frontendId")
                        .HasColumnType("integer");

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("backendId");

                    b.HasIndex("databaseId");

                    b.HasIndex("frontendId");

                    b.HasIndex("userId");

                    b.ToTable("UserStacks");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Workshop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Presenter")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("presenter");

                    b.Property<int>("eventId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("eventId");

                    b.ToTable("Workshop");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Certifications", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", null)
                        .WithMany("UserCertifications")
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Events", b =>
                {
                    b.HasOne("RGO.Repository.Entities.UserGroup", "GroupEvents")
                        .WithMany()
                        .HasForeignKey("groupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupEvents");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Field", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Form", "FormFields")
                        .WithMany()
                        .HasForeignKey("formId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FormFields");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Form", b =>
                {
                    b.HasOne("RGO.Repository.Entities.UserGroup", "UserGroupForm")
                        .WithMany()
                        .HasForeignKey("groupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserGroupForm");
                });

            modelBuilder.Entity("RGO.Repository.Entities.FormSubmit", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Form", "Form")
                        .WithMany()
                        .HasForeignKey("formId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.FormSubmit", null)
                        .WithMany("InputSubmit")
                        .HasForeignKey("formSubmitId");

                    b.HasOne("RGO.Repository.Entities.User", "UserSubmit")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");

                    b.Navigation("UserSubmit");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Input", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Field", "InputFieldId")
                        .WithMany()
                        .HasForeignKey("fieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.User", "InputUserId")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InputFieldId");

                    b.Navigation("InputUserId");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Option", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Field", "FieldOptions")
                        .WithMany()
                        .HasForeignKey("fieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldOptions");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Projects", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", null)
                        .WithMany("UserProjects")
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Skill", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", null)
                        .WithMany("Skills")
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Social", b =>
                {
                    b.HasOne("RGO.Repository.Entities.User", "UserSocial")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserSocial");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.HasOne("RGO.Repository.Entities.UserGroup", "UserGroup")
                        .WithMany()
                        .HasForeignKey("gradGroupId");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("RGO.Repository.Entities.UserStacks", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Stacks", "BackendUserStack")
                        .WithMany()
                        .HasForeignKey("backendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.Stacks", "DatabaseUserStack")
                        .WithMany()
                        .HasForeignKey("databaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.Stacks", "FrontendUserStack")
                        .WithMany()
                        .HasForeignKey("frontendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RGO.Repository.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BackendUserStack");

                    b.Navigation("DatabaseUserStack");

                    b.Navigation("FrontendUserStack");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Workshop", b =>
                {
                    b.HasOne("RGO.Repository.Entities.Events", "WorshopEvents")
                        .WithMany()
                        .HasForeignKey("eventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorshopEvents");
                });

            modelBuilder.Entity("RGO.Repository.Entities.FormSubmit", b =>
                {
                    b.Navigation("InputSubmit");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.Navigation("Skills");

                    b.Navigation("UserCertifications");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
