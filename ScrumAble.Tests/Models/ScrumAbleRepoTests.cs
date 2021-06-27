using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Controllers;
using ScrumAble.Data;
using ScrumAble.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ScrumAble.Tests.Controllers;
using Xunit;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class ScrumAbleRepoTests
    {
        [Fact]
        public void UT8_ScrumAbleRepo_GetTaskById_ShouldReturnTheCorrectTask()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT8")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testTask = MockScrumAbleTask.GenerateTask();
            context.Add(testTask);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetTaskById(testTask.Id);

            // Assert
            Assert.Equal(testTask, result);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT9_ScrumAbleRepo_PopulateTaskMetadata_ShouldReturnTaskWithMetadata()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT9")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);

            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testStory = MockScrumAbleStory.GenerateStory();
            var testTask = MockScrumAbleTask.GenerateTask(testStory, testWorkflowStage, testSprint, testUser);
            context.Add(testTeam);
            context.Add(testWorkflowStage);
            context.Add(testRelease);
            context.Add(testSprint);
            context.Add(testStory);
            context.Add(testTask);
            context.SaveChanges();

            var testTaskNewCall = scrumAbleRepo.GetTaskById(testTask.Id);

            // Act
            var result = scrumAbleRepo.PopulateTaskMetadata(testTaskNewCall);

            // Assert
            Assert.Equal(testStory.Id, result.Story.Id);
            Assert.Equal(testWorkflowStage.Id, result.WorkflowStage.Id);
            Assert.Equal(testSprint.Id, result.Sprint.Id);
            Assert.Equal(testUser.Id, result.TaskOwner.Id);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT10_ScrumAbleRepo_SaveToDb_ShouldAddTaskToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT10")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testTask = MockScrumAbleTask.GenerateTask();

            // Act
            scrumAbleRepo.SaveToDb(testTask);
            MockScrumAbleTask taskDBItems = (MockScrumAbleTask) await context.Tasks.SingleAsync();

            // Assert
            Assert.NotNull(taskDBItems);
            Assert.Equal("Test Task", taskDBItems.TaskName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT11_ScrumAbleRepo_SaveToDb_ShouldUpdateTaskInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT11")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testTask = MockScrumAbleTask.GenerateTask();
            context.Add(testTask);
            context.SaveChanges();

            testTask.TaskName = "Updated Name";

            // Act
            scrumAbleRepo.SaveToDb(testTask);
            MockScrumAbleTask taskDBItems = (MockScrumAbleTask)await context.Tasks.SingleAsync();

            // Assert
            Assert.Equal("Updated Name", taskDBItems.TaskName);

            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public async void UT12_ScrumAbleRepo_DeleteFromDb_ShouldDeleteTaskFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT12")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testTask = MockScrumAbleTask.GenerateTask();
            context.Add(testTask);
            context.SaveChanges();


            // Act
            scrumAbleRepo.DeleteFromDb(testTask);
            var numRecords = context.Tasks.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT13_ScrumAbleRepo_GetUserById_ShouldReturnTheCorrectUserFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT13")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testUserTeamMapping2 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            testUser.CurrentWorkingTeam = testTeam;
            testUser2.CurrentWorkingTeam = testTeam;

            context.Add(testUser);
            context.Add(testUser2);
            context.Add(testTeam);
            context.Add(testUserTeamMapping);
            context.Add(testUserTeamMapping2);
            context.SaveChanges();


            // Act
            var testUserReturned = scrumAbleRepo.GetUserById(testUser.Id);

            // Assert
            Assert.Equal(testUser, testUserReturned);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT14_ScrumAbleRepo_GetUserByUserName_ShouldReturnTheCorrectUserFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT14")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testUserTeamMapping2 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            testUser.CurrentWorkingTeam = testTeam;
            testUser2.CurrentWorkingTeam = testTeam;

            context.Add(testUser);
            context.Add(testUser2);
            context.Add(testTeam);
            context.Add(testUserTeamMapping);
            context.Add(testUserTeamMapping2);
            context.SaveChanges();


            // Act
            var testUserReturned = scrumAbleRepo.GetUserByUsername(testUser.UserName);

            // Assert
            Assert.Equal(testUser.UserName, testUserReturned.UserName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT15_ScrumAbleRepo_SaveToDb_ShouldUpdateUserInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT15")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            context.Add(testUser);
            context.SaveChanges();

            testUser.UserName = "changed@test.com";

            // Act
            scrumAbleRepo.SaveToDb(testUser);
            var UserDBItem = (MockScrumAbleUser)await context.User.SingleAsync();

            // Assert
            Assert.Equal("changed@test.com", UserDBItem.UserName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT16_ScrumAbleRepo_DeleteFromDb_ShouldDeleteUserFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT16")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            context.Add(testUser);
            context.SaveChanges();


            // Act
            scrumAbleRepo.DeleteFromDb(testUser);
            var numRecords = context.Tasks.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT17_ScrumAbleRepo_GetSprintById_ShouldReturnTheCorrectSprint()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT17")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testSprint = MockScrumAbleSprint.GenerateSprint();
            context.Add(testSprint);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetSprintById(testSprint.Id);

            // Assert
            Assert.Equal(testSprint.Id, result.Id);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT18_ScrumAbleRepo_GetStoryById_ShouldReturnTheCorrectStory()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT18")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testStory = MockScrumAbleStory.GenerateStory();
            context.Add(testStory);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetStoryById(testStory.Id);

            // Assert
            Assert.Equal(testStory.Id, result.Id);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT19_ScrumAbleRepo_GetTaskAggregateData_ShouldReturnAggregateData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT19")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);

            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testStory = MockScrumAbleStory.GenerateStory();
            var testTask = MockScrumAbleTask.GenerateTask(testStory, testWorkflowStage, testSprint, testUser);

            testUser.CurrentWorkingRelease = testRelease;
            testUser.CurrentWorkingTeam = testTeam;
            testUser.CurrentWorkingSprint = testSprint;

            context.Add(testTeam);
            context.Add(testWorkflowStage);
            context.Add(testRelease);
            context.Add(testSprint);
            context.Add(testStory);
            context.Add(testTask);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetTaskAggregateData(testUser.Id);

            // Assert
            Assert.NotNull(result.Users);
            Assert.NotNull(result.Sprints);
            Assert.NotNull(result.Stories);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT20_ScrumAbleRepo_GetTeamById_ShouldReturnTheCorrectTeam()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT20")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            context.Add(testTeam);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetTeamById(testTeam.Id);

            // Assert
            Assert.Equal(testTeam.Id, result.Id);
            context.Database.EnsureDeleted();
        }




    }
}
