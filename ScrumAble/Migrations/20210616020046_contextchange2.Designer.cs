﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScrumAble.Data;

namespace ScrumAble.Migrations
{
    [DbContext(typeof(ScrumAbleContext))]
    [Migration("20210616020046_contextchange2")]
    partial class contextchange2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ScrumAble.Areas.Identity.Data.ScrumAbleUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrentWorkingReleaseId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentWorkingSprintId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentWorkingTeamId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentWorkingReleaseId");

                    b.HasIndex("CurrentWorkingSprintId");

                    b.HasIndex("CurrentWorkingTeamId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleRelease", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ReleaseEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReleaseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleSprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ReleaseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SprintEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SprintName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SprintStartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReleaseId");

                    b.ToTable("Sprints");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleStory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SprintId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StoryCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StoryDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StoryDueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StoryOwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("StoryPoints")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StoryStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WorkflowStageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SprintId");

                    b.HasIndex("StoryOwnerId");

                    b.HasIndex("WorkflowStageId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SprintId")
                        .HasColumnType("int");

                    b.Property<int?>("StoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TaskCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TaskDueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskOwnerId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("TaskPoints")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime?>("TaskStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WorkflowStageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SprintId");

                    b.HasIndex("StoryId");

                    b.HasIndex("TaskOwnerId1");

                    b.HasIndex("WorkflowStageId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleUserTeamMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTeamMapping");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleWorkflowStage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowStageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkflowStagePosition")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("WorkflowStages");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScrumAble.Areas.Identity.Data.ScrumAbleUser", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleRelease", "CurrentWorkingRelease")
                        .WithMany()
                        .HasForeignKey("CurrentWorkingReleaseId");

                    b.HasOne("ScrumAble.Models.ScrumAbleSprint", "CurrentWorkingSprint")
                        .WithMany()
                        .HasForeignKey("CurrentWorkingSprintId");

                    b.HasOne("ScrumAble.Models.ScrumAbleTeam", "CurrentWorkingTeam")
                        .WithMany()
                        .HasForeignKey("CurrentWorkingTeamId");

                    b.Navigation("CurrentWorkingRelease");

                    b.Navigation("CurrentWorkingSprint");

                    b.Navigation("CurrentWorkingTeam");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleRelease", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleTeam", "Team")
                        .WithMany("Releases")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleSprint", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleRelease", "Release")
                        .WithMany("Sprints")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Release");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleStory", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleSprint", "Sprint")
                        .WithMany("Stories")
                        .HasForeignKey("SprintId");

                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", "StoryOwner")
                        .WithMany("Stories")
                        .HasForeignKey("StoryOwnerId");

                    b.HasOne("ScrumAble.Models.ScrumAbleWorkflowStage", "WorkflowStage")
                        .WithMany("Stories")
                        .HasForeignKey("WorkflowStageId");

                    b.Navigation("Sprint");

                    b.Navigation("StoryOwner");

                    b.Navigation("WorkflowStage");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleTask", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleSprint", "Sprint")
                        .WithMany("Tasks")
                        .HasForeignKey("SprintId");

                    b.HasOne("ScrumAble.Models.ScrumAbleStory", "Story")
                        .WithMany("Tasks")
                        .HasForeignKey("StoryId");

                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", "TaskOwner")
                        .WithMany("Tasks")
                        .HasForeignKey("TaskOwnerId1");

                    b.HasOne("ScrumAble.Models.ScrumAbleWorkflowStage", "WorkflowStage")
                        .WithMany("Tasks")
                        .HasForeignKey("WorkflowStageId");

                    b.Navigation("Sprint");

                    b.Navigation("Story");

                    b.Navigation("TaskOwner");

                    b.Navigation("WorkflowStage");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleUserTeamMapping", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleTeam", "Team")
                        .WithMany("UserTeamMappings")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScrumAble.Areas.Identity.Data.ScrumAbleUser", "User")
                        .WithMany("UserTeamMappings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleWorkflowStage", b =>
                {
                    b.HasOne("ScrumAble.Models.ScrumAbleTeam", "Team")
                        .WithMany("WorkFlowStages")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ScrumAble.Areas.Identity.Data.ScrumAbleUser", b =>
                {
                    b.Navigation("Stories");

                    b.Navigation("Tasks");

                    b.Navigation("UserTeamMappings");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleRelease", b =>
                {
                    b.Navigation("Sprints");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleSprint", b =>
                {
                    b.Navigation("Stories");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleStory", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleTeam", b =>
                {
                    b.Navigation("Releases");

                    b.Navigation("UserTeamMappings");

                    b.Navigation("WorkFlowStages");
                });

            modelBuilder.Entity("ScrumAble.Models.ScrumAbleWorkflowStage", b =>
                {
                    b.Navigation("Stories");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
