﻿// <auto-generated />
using System;
using AnalysisData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnalysisData.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240905085510_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AnalysisData.Graph.Model.Category.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Edge.AttributeEdge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AttributeEdges");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Edge.EntityEdge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EntityIDSource")
                        .HasColumnType("integer");

                    b.Property<int>("EntityIDTarget")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EntityIDSource");

                    b.HasIndex("EntityIDTarget");

                    b.ToTable("EntityEdges");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Edge.ValueEdge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("integer");

                    b.Property<int>("EntityId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("ValueEdges");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.File.FileEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UploaderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UploaderId");

                    b.ToTable("FileUploadedDb");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.File.UserFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("FileId")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFiles");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Node.AttributeNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AttributeNodes");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Node.EntityNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("NodeFileReferenceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("NodeFileReferenceId");

                    b.ToTable("EntityNodes");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Node.ValueNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("integer");

                    b.Property<int>("EntityId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("ValueNodes");
                });

            modelBuilder.Entity("AnalysisData.User.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RolePolicy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleName = "admin",
                            RolePolicy = "gold"
                        },
                        new
                        {
                            Id = 2,
                            RoleName = "Data-Analyst",
                            RolePolicy = "bronze"
                        },
                        new
                        {
                            Id = 3,
                            RoleName = "Data-Manager",
                            RolePolicy = "silver"
                        });
                });

            modelBuilder.Entity("AnalysisData.User.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageURL")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1b5e7d3c-f751-4484-8efd-0463fd137f0f"),
                            Email = "admin@gmail.com",
                            FirstName = "admin",
                            LastName = "admin",
                            Password = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=",
                            PhoneNumber = "09131111111",
                            RoleId = 1,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Edge.EntityEdge", b =>
                {
                    b.HasOne("AnalysisData.Graph.Model.Node.EntityNode", "SourceNode")
                        .WithMany()
                        .HasForeignKey("EntityIDSource")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnalysisData.Graph.Model.Node.EntityNode", "TargetNode")
                        .WithMany()
                        .HasForeignKey("EntityIDTarget")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SourceNode");

                    b.Navigation("TargetNode");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Edge.ValueEdge", b =>
                {
                    b.HasOne("AnalysisData.Graph.Model.Edge.AttributeEdge", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnalysisData.Graph.Model.Edge.EntityEdge", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.File.FileEntity", b =>
                {
                    b.HasOne("AnalysisData.Graph.Model.Category.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnalysisData.User.Model.User", "User")
                        .WithMany("UploadData")
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.File.UserFile", b =>
                {
                    b.HasOne("AnalysisData.Graph.Model.File.FileEntity", "FileEntity")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnalysisData.User.Model.User", "User")
                        .WithMany("UserFiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileEntity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Node.EntityNode", b =>
                {
                    b.HasOne("AnalysisData.Graph.Model.File.FileEntity", "FileEntity")
                        .WithMany("EntityNodes")
                        .HasForeignKey("NodeFileReferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileEntity");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.Node.ValueNode", b =>
                {
                    b.HasOne("AnalysisData.Graph.Model.Node.AttributeNode", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnalysisData.Graph.Model.Node.EntityNode", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AnalysisData.User.Model.User", b =>
                {
                    b.HasOne("AnalysisData.User.Model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AnalysisData.Graph.Model.File.FileEntity", b =>
                {
                    b.Navigation("EntityNodes");
                });

            modelBuilder.Entity("AnalysisData.User.Model.User", b =>
                {
                    b.Navigation("UploadData");

                    b.Navigation("UserFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
