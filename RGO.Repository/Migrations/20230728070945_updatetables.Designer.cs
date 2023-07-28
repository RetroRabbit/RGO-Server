﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RGO.Repository;

#nullable disable

namespace RGO.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230728070945_updatetables")]
    partial class updatetables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RGO.Repository.Entities.Events", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("enddate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("eventtype")
                        .HasColumnType("integer");

                    b.Property<int>("groupid")
                        .HasColumnType("integer");

                    b.Property<DateTime>("startdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("usertype")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("events");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Field", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("errormessage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("formid")
                        .HasColumnType("integer");

                    b.Property<string>("label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("required")
                        .HasColumnType("boolean");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("fields");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Form", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("endDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("groupid")
                        .HasColumnType("integer");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("forms");
                });

            modelBuilder.Entity("RGO.Repository.Entities.FormSubmit", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("createDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("formid")
                        .HasColumnType("integer");

                    b.Property<string>("rejectionreason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<int>("userid")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("formsubmits");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Input", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("createDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("fieldId")
                        .HasColumnType("integer");

                    b.Property<int>("formSubmitId")
                        .HasColumnType("integer");

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("input");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Option", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("fieldid")
                        .HasColumnType("integer");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("options");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Stacks", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("stackType")
                        .HasColumnType("integer");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("stacks");
                });

            modelBuilder.Entity("RGO.Repository.Entities.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("groupid")
                        .HasColumnType("integer");

                    b.Property<DateTime>("joindate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("RGO.Repository.Entities.UserGroup", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("usergroups");
                });

            modelBuilder.Entity("RGO.Repository.Entities.UserStacks", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("backendId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("createDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("databaseId")
                        .HasColumnType("integer");

                    b.Property<int>("frontendId")
                        .HasColumnType("integer");

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("userStacks");
                });

            modelBuilder.Entity("RGO.Repository.Entities.Workshop", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("eventId")
                        .HasColumnType("integer");

                    b.Property<string>("presenter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("workshop");
                });
#pragma warning restore 612, 618
        }
    }
}
