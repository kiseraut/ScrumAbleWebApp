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
using ScrumAble.Areas.Identity.Data;
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
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testSprint = MockScrumAbleSprint.GenerateSprint();
            var testRelease = MockScrumAbleRelease.GenerateRelease();

            testSprint.IsActiveSprint = true;
            testSprint.Release = testRelease;
            testTask.TaskPoints = 5;
            testTask.Sprint = testSprint;


            context.Add(testSprint);
            context.Add(testRelease);
            context.Add(testUser);
            context.SaveChanges();

            // Act
            scrumAbleRepo.SaveToDb(testTask, testUser);
            MockScrumAbleTask taskDbItems = (MockScrumAbleTask) await context.Tasks.SingleAsync();

            // Assert
            Assert.NotNull(taskDbItems);
            Assert.Equal("Test Task", taskDbItems.TaskName);

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
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();

            context.Add(testUser);
            context.Add(testTask);
            context.SaveChanges();

            testTask.TaskName = "Updated Name";

            // Act
            scrumAbleRepo.SaveToDb(testTask, testUser);
            MockScrumAbleTask taskDbItems = (MockScrumAbleTask)await context.Tasks.SingleAsync();

            // Assert
            Assert.Equal("Updated Name", taskDbItems.TaskName);

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
            var userDbItem = (MockScrumAbleUser)await context.User.SingleAsync();

            // Assert
            Assert.Equal("changed@test.com", userDbItem.UserName);

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

        [Fact]
        public async void UT21_ScrumAbleRepo_SaveToDb_ShouldUpdateTeamInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT20")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var userList = new List<IScrumAbleUser>(){};
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            context.Add(testTeam);
            context.SaveChanges();

            testTeam.TeamName = "Changed";

            // Act
            scrumAbleRepo.SaveToDb(testTeam, userList);

            var teamDbItem = (MockScrumAbleTeam)await context.Teams.SingleAsync();

            // Assert
            Assert.Equal("Changed", testTeam.TeamName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT22_ScrumAbleRepo_DeleteFromDb_ShouldDeleteTeamFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT22")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            context.Add(testTeam);
            context.SaveChanges();


            // Act
            scrumAbleRepo.DeleteFromDb(testTeam);
            var numRecords = context.Teams.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT23_ScrumAbleRepo_IsAuthorized_ShouldNotAllowUnauthorizedUserToAccessElements()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT23")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);

            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser1 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testStory = MockScrumAbleStory.GenerateStory(testSprint);
            var testTask = MockScrumAbleTask.GenerateTask(testStory, testWorkflowStage, testSprint, testUser2);

            context.Add(testUser1);
            context.Add(testUser2);
            context.Add(testTeam);
            context.Add(testUserTeamMapping);
            context.Add(testWorkflowStage);
            context.Add(testRelease);
            context.Add(testSprint);
            context.Add(testStory);
            context.Add(testTask);
            context.SaveChanges();

            // Act
            var teamResult = scrumAbleRepo.IsAuthorized(testTeam, testUser1.Id);
            var releaseResult = scrumAbleRepo.IsAuthorized(testRelease, testUser1.Id);
            var storyResult = scrumAbleRepo.IsAuthorized(testStory, testUser1.Id);
            var taskResult = scrumAbleRepo.IsAuthorized(testTask, testUser1.Id);
            var workflowStageResult = scrumAbleRepo.IsAuthorized(testWorkflowStage, testUser1.Id);

            // Assert
            Assert.False(teamResult);
            Assert.False(releaseResult);
            Assert.False(storyResult);
            Assert.False(taskResult);
            Assert.False(workflowStageResult);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT24_ScrumAbleRepo_IsAuthorized_ShouldAllowAuthorizedUserToAccessElements()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT24")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);

            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser1 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testStory = MockScrumAbleStory.GenerateStory(testSprint);
            var testTask = MockScrumAbleTask.GenerateTask(testStory, testWorkflowStage, testSprint, testUser2);

            context.Add(testUser1);
            context.Add(testUser2);
            context.Add(testTeam);
            context.Add(testUserTeamMapping);
            context.Add(testWorkflowStage);
            context.Add(testRelease);
            context.Add(testSprint);
            context.Add(testStory);
            context.Add(testTask);
            context.SaveChanges();

            // Act
            var teamResult = scrumAbleRepo.IsAuthorized(testTeam, testUser2.Id);
            var releaseResult = scrumAbleRepo.IsAuthorized(testRelease, testUser2.Id);
            var storyResult = scrumAbleRepo.IsAuthorized(testStory, testUser2.Id);
            var taskResult = scrumAbleRepo.IsAuthorized(testTask, testUser2.Id);
            var workflowStageResult = scrumAbleRepo.IsAuthorized(testWorkflowStage, testUser2.Id);

            // Assert
            Assert.True(teamResult);
            Assert.True(releaseResult);
            Assert.True(storyResult);
            Assert.True(taskResult);
            Assert.True(workflowStageResult);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT25_ScrumAbleRepo_SaveToDb_ShouldAddTeamToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT25")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var userList = new List<IScrumAbleUser>() { };
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            testTeam.TeamName = "New Test Team";

            // Act
            scrumAbleRepo.SaveToDb(testTeam, userList);
            MockScrumAbleTeam teamDbItems = (MockScrumAbleTeam)await context.Teams.SingleAsync();

            // Assert
            Assert.NotNull(teamDbItems);
            Assert.Equal("New Test Team", teamDbItems.TeamName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT44_ScrumAbleRepo_SetCurrentRelease_ShouldSetTheCurrentReleaseForTheUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT44")
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

            //Act
            scrumAbleRepo.SetCurrentRelease(testUser.Id, testRelease.Id);
            MockScrumAbleUser dbUser = (MockScrumAbleUser)await context.User.SingleAsync();

            //Assert
            Assert.Equal(testRelease, dbUser.CurrentWorkingRelease);
        }

        [Fact]
        public async void UT45_ScrumAbleRepo_SetCurrentSprint_ShouldSetTheCurrentSprintForTheUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT45")
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

            //Act
            scrumAbleRepo.SetCurrentSprint(testUser.Id, testSprint.Id);
            MockScrumAbleUser dbUser = (MockScrumAbleUser)await context.User.SingleAsync();

            //Assert
            Assert.Equal(testSprint, dbUser.CurrentWorkingSprint);
        }

        [Fact]
        public async void UT46_ScrumAbleRepo_SetCurrentTeam_ShouldSetTheCurrentTeamForTheUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT46")
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

            //Act
            scrumAbleRepo.SetCurrentTeam(testUser.Id, testTeam.Id);
            MockScrumAbleUser dbUser = (MockScrumAbleUser)await context.User.SingleAsync();

            //Assert
            Assert.Equal(testTeam, dbUser.CurrentWorkingTeam);
        }

        [Fact]
        public async void UT47_ScrumAbleRepo_SaveToDb_ShouldUpdateSprintInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT47")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testSprint = MockScrumAbleSprint.GenerateSprint();
            context.Add(testSprint);
            context.SaveChanges();

            testSprint.SprintName = "Updated";

            // Act
            scrumAbleRepo.SaveToDb(testSprint);
            var sprintDbItem = (MockScrumAbleSprint)await context.Sprints.SingleAsync();

            // Assert
            Assert.Equal("Updated", sprintDbItem.SprintName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT48_ScrumAbleRepo_SaveToDb_ShouldAddSprintToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT48")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testSprint = MockScrumAbleSprint.GenerateSprint();

            // Act
            scrumAbleRepo.SaveToDb(testSprint);
            var sprintDbItem = (MockScrumAbleSprint)await context.Sprints.SingleAsync();

            // Assert
            Assert.NotNull(sprintDbItem);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT49_ScrumAbleRepo_DeleteFromDb_ShouldDeleteSprintFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT49")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testSprint = MockScrumAbleSprint.GenerateSprint();
            context.Add(testSprint);
            context.SaveChanges();


            // Act
            scrumAbleRepo.DeleteFromDb(testSprint);
            var numRecords = context.Sprints.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT50_ScrumAbleRepo_DeleteFromDb_ShouldDeleteReleaseFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT50")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            context.Add(testRelease);
            context.SaveChanges();


            // Act
            scrumAbleRepo.DeleteFromDb(testRelease);
            var numRecords = context.Releases.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT51_ScrumAbleRepo_SaveToDb_ShouldUpdateReleaseInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT51")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            context.Add(testRelease);
            context.SaveChanges();

            testRelease.ReleaseName = "Updated";

            // Act
            scrumAbleRepo.SaveToDb(testRelease);
            var sprintDbItem = (MockScrumAbleRelease)await context.Releases.SingleAsync();

            // Assert
            Assert.Equal("Updated", sprintDbItem.ReleaseName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT52_ScrumAbleRepo_SaveToDb_ShouldAddReleaseToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT52")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testRelease = MockScrumAbleRelease.GenerateRelease();

            // Act
            scrumAbleRepo.SaveToDb(testRelease);
            var releaseDbItem = (MockScrumAbleRelease)await context.Releases.SingleAsync();

            // Assert
            Assert.NotNull(releaseDbItem);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT59_ScrumAbleRepo_GetWorkflowStageById_ShouldReturnTheCorrectWorkflowStage()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT59")
                .Options;
            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            context.Add(testWorkflowStage);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetWorkflowStageById(testWorkflowStage.Id);

            // Assert
            Assert.Equal(testWorkflowStage.Id, result.Id);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT60_ScrumAbleRepo_SaveToDb_ShouldAddWorkflowStageToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT60")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var workflowStageSprint = MockScrumAbleWorkflowStage.GenerateWorkflowStage();

            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testWorkflowStage3 = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testWorkflowStage4 = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            context.Add(testUser);
            context.Add(testTeam);
            context.Add(testUserTeamMapping);
            context.Add(testWorkflowStage2);
            context.Add(testWorkflowStage3);
            context.Add(testWorkflowStage4);
            context.SaveChanges();

            testWorkflowStage.NewWorkflowStageOrder = new List<int>() { testWorkflowStage2.Id, testWorkflowStage3.Id, testWorkflowStage4.Id, -1};


            // Act
            var result = scrumAbleRepo.SaveToDb(testWorkflowStage, testUser);
            var numRecords = context.WorkflowStages.Count();
            var workflowStageDbItem = context.WorkflowStages.Where(w => w.Id == testWorkflowStage.Id).SingleOrDefault();

            // Assert
            Assert.NotNull(workflowStageDbItem);
            Assert.Equal(4, numRecords);
            Assert.True(result);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT61_ScrumAbleRepo_SaveToDb_ShouldUpdateWorkflowStageInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT61")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var workflowStageSprint = MockScrumAbleWorkflowStage.GenerateWorkflowStage();

            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testWorkflowStage3 = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            var testWorkflowStage4 = MockScrumAbleWorkflowStage.GenerateWorkflowStage(testTeam);
            context.Add(testUser);
            context.Add(testTeam);
            context.Add(testUserTeamMapping);
            context.Add(testWorkflowStage);
            context.Add(testWorkflowStage2);
            context.Add(testWorkflowStage3);
            context.Add(testWorkflowStage4);
            context.SaveChanges();

            testWorkflowStage.NewWorkflowStageOrder = new List<int>() { testWorkflowStage2.Id, testWorkflowStage3.Id, testWorkflowStage4.Id, -1 };
            testWorkflowStage.WorkflowStageName = "Updated Test Workflow Stage";


            // Act
            var result = scrumAbleRepo.SaveToDb(testWorkflowStage, testUser);
            var numRecords = context.WorkflowStages.Count();
            var workflowStageDbItem = context.WorkflowStages.Where(w => w.Id == testWorkflowStage.Id).SingleOrDefault();

            // Assert
            Assert.NotNull(workflowStageDbItem);
            Assert.Equal("Updated Test Workflow Stage", workflowStageDbItem.WorkflowStageName);
            Assert.Equal(4, numRecords);
            Assert.True(result);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT62_ScrumAbleRepo_DeleteFromDb_ShouldDeleteWorkflowStageFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT62")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testWorkflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            context.Add(testWorkflowStage);
            context.SaveChanges();


            // Act
            scrumAbleRepo.DeleteFromDb(testWorkflowStage);
            var numRecords = context.WorkflowStages.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT80_ScrumAbleRepo_MoveTask_ShouldMoveTaskToWorkflowStage()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT80")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testWorkflowStage1 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testTask = MockScrumAbleTask.GenerateTask();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);

            testWorkflowStage1.WorkflowStagePosition = 0;
            testWorkflowStage1.Team = testTeam;
            testWorkflowStage1.WorkflowStagePosition = 1;
            testWorkflowStage2.Team = testTeam;
            testTask.WorkflowStage = testWorkflowStage1;
            testTask.Sprint = testSprint;

            context.Add(testWorkflowStage1);
            context.Add(testWorkflowStage2);
            context.Add(testTask);
            context.Add(testUser);
            context.Add(testUserTeamMapping);
            context.Add(testRelease);
            context.Add(testSprint);
            context.SaveChanges();

            // Act
            scrumAbleRepo.MoveTask(testTask.Id, testWorkflowStage2.Id, testUser);

            // Assert
            Assert.Equal(testWorkflowStage2, testTask.WorkflowStage);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT81_ScrumAbleRepo_MoveStory_ShouldMoveStoryToWorkflowStage()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT81")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testWorkflowStage1 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testStory = MockScrumAbleStory.GenerateStory();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);

            testWorkflowStage1.WorkflowStagePosition = 0;
            testWorkflowStage1.Team = testTeam;
            testWorkflowStage1.WorkflowStagePosition = 1;
            testWorkflowStage2.Team = testTeam;
            testStory.WorkflowStage = testWorkflowStage1;
            testStory.Sprint = testSprint;
            testSprint.IsActiveSprint = true;
            testStory.StoryCloseDate = DateTime.Today;
            testStory.StoryPoints = 5;
            testUser.CurrentWorkingSprint = testSprint;

            var graphData = string.Format("{0},26,26#{1},25.1,26#{2},24.1,23", DateTime.Today.AddDays(-2).ToString("d"), DateTime.Today.AddDays(-1).ToString("d"), DateTime.Today.ToString("d"));
            testSprint.GraphData = graphData;

            context.Add(testWorkflowStage1);
            context.Add(testWorkflowStage2);
            context.Add(testStory);
            context.Add(testUser);
            context.Add(testUserTeamMapping);
            context.Add(testRelease);
            context.Add(testSprint);
            context.SaveChanges();

            // Act
            scrumAbleRepo.MoveStory(testStory.Id, testWorkflowStage2.Id, testUser);

            // Assert
            Assert.Equal(testWorkflowStage2, testStory.WorkflowStage);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT82_ScrumAbleRepo_MoveDefect_ShouldMoveDefectToWorkflowStage()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT82")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testWorkflowStage1 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testDefect = MockScrumAbleDefect.GenerateDefect();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);

            testWorkflowStage1.WorkflowStagePosition = 0;
            testWorkflowStage1.Team = testTeam;
            testWorkflowStage1.WorkflowStagePosition = 1;
            testWorkflowStage2.Team = testTeam;
            testDefect.WorkflowStage = testWorkflowStage1;
            testDefect.Sprint = testSprint;

            context.Add(testWorkflowStage1);
            context.Add(testWorkflowStage2);
            context.Add(testDefect);
            context.Add(testUser);
            context.Add(testUserTeamMapping);
            context.Add(testRelease);
            context.Add(testSprint);
            context.SaveChanges();

            // Act
            scrumAbleRepo.MoveDefect(testDefect.Id, testWorkflowStage2.Id, testUser);

            // Assert
            Assert.Equal(testWorkflowStage2, testDefect.WorkflowStage);

            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT83_ScrumAbleRepo_GetSprintForDashboard_ShouldReturnACorrectlyPopulatedSprint()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT83")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testWorkflowStage1 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var testStory = MockScrumAbleStory.GenerateStory();
            var testTask = MockScrumAbleTask.GenerateTask();
            var testDefect = MockScrumAbleDefect.GenerateDefect();
            var testUser1 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping1 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser1, testTeam);
            var testUserTeamMapping2 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);

            testWorkflowStage1.WorkflowStagePosition = 0;
            testWorkflowStage1.Team = testTeam;
            testWorkflowStage1.WorkflowStagePosition = 1;
            testWorkflowStage2.Team = testTeam;

            testStory.WorkflowStage = testWorkflowStage1;
            testTask.WorkflowStage = testWorkflowStage2;
            testDefect.WorkflowStage = testWorkflowStage1;

            testDefect.DefectOwner = testUser1;
            testStory.StoryOwner = testUser2;
            testTask.TaskOwner = testUser1;

            testDefect.Sprint = testSprint;
            testStory.Sprint = testSprint;
            testTask.Sprint = testSprint;

            context.Add(testWorkflowStage1);
            context.Add(testWorkflowStage2);
            context.Add(testDefect);
            context.Add(testStory);
            context.Add(testTask);
            context.Add(testUser1);
            context.Add(testUser2);
            context.Add(testUserTeamMapping1);
            context.Add(testUserTeamMapping2);
            context.Add(testRelease);
            context.Add(testSprint);
            context.SaveChanges();

            // Act
            var resultSprint = scrumAbleRepo.GetSprintForDashboard(testSprint.Id);

            // Assert
            Assert.Equal(testStory, resultSprint.Stories.FirstOrDefault());
            Assert.Equal(testTask, resultSprint.Tasks.FirstOrDefault());
            Assert.Equal(testDefect, resultSprint.Defects.FirstOrDefault());
            Assert.Equal(testRelease, resultSprint.Release);
            
            Assert.Equal(2, resultSprint.WorkflowStages.Count);
            Assert.Equal(testWorkflowStage1, resultSprint.WorkflowStages[0]);
            Assert.Equal(testWorkflowStage2, resultSprint.WorkflowStages[1]);

            Assert.Equal(testUser2, resultSprint.WorkflowStages[0].Stories.First().StoryOwner);
            Assert.Equal(testUser1, resultSprint.WorkflowStages[0].Defects.First().DefectOwner);
            Assert.Equal(testUser1, resultSprint.WorkflowStages[1].Tasks.First().TaskOwner);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT85_ScrumAbleRepo_PrepareUserForDashboard_ShouldSetWorkingSprintReleaseAndTeam()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT85")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser1 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser3 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser4 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping1 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser1, testTeam);
            var testUserTeamMapping2 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            var testUserTeamMapping3 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser3, testTeam);
            var testUserTeamMapping4 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser4, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);

            testSprint.SprintEndDate = DateTime.Today;

            testUser1.CurrentWorkingSprint = testSprint;
            testUser1.CurrentWorkingRelease = testRelease;
            testUser1.CurrentWorkingTeam = testTeam;

            testUser3.CurrentWorkingTeam = testTeam;

            testUser4.CurrentWorkingTeam = testTeam;
            testUser4.CurrentWorkingRelease = testRelease;

            context.Add(testUser1);
            context.Add(testUser2);
            context.Add(testUserTeamMapping1);
            context.Add(testUserTeamMapping2);
            context.Add(testUserTeamMapping3);
            context.Add(testUserTeamMapping4);
            context.Add(testRelease);
            context.Add(testSprint);
            context.SaveChanges();

            // Act
            var resultUser1 = scrumAbleRepo.PrepareUserForDashboard(testUser1);
            var resultUser2 = scrumAbleRepo.PrepareUserForDashboard(testUser2);
            var resultUser3 = scrumAbleRepo.PrepareUserForDashboard(testUser3);
            var resultUser4 = scrumAbleRepo.PrepareUserForDashboard(testUser4);

            // Assert
            Assert.Equal(testTeam, testUser1.CurrentWorkingTeam);
            Assert.Equal(testRelease, testUser1.CurrentWorkingRelease);
            Assert.Equal(testSprint, testUser1.CurrentWorkingSprint);
            
            Assert.Equal(testTeam, testUser2.CurrentWorkingTeam);
            Assert.Equal(testRelease, testUser2.CurrentWorkingRelease);
            Assert.Equal(testSprint, testUser2.CurrentWorkingSprint);
            
            Assert.Equal(testTeam, testUser3.CurrentWorkingTeam);
            Assert.Equal(testRelease, testUser3.CurrentWorkingRelease);
            Assert.Equal(testSprint, testUser3.CurrentWorkingSprint);
            
            Assert.Equal(testTeam, testUser4.CurrentWorkingTeam);
            Assert.Equal(testRelease, testUser4.CurrentWorkingRelease);
            Assert.Equal(testSprint, testUser4.CurrentWorkingSprint);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT86_ScrumAbleRepo_UpdateGraphDataActualPoints_ShouldUpdateTheGraphData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT86")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser1 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testUser2 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping1 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser1, testTeam);
            var testUserTeamMapping2 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser2, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint1 = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testSprint2 = MockScrumAbleSprint.GenerateSprint(testRelease);

            testSprint1.GraphData = "6/21/2021,26,26#6/22/2021,25.1,26#6/23/2021,24.1,23";
            testSprint1.SprintEndDate = DateTime.Today;


            var graphData = string.Format("{0},26,26#{1},25.1,26#{2},24.1,23", DateTime.Today.AddDays(-2).ToString("d"), DateTime.Today.AddDays(-1).ToString("d"), DateTime.Today.ToString("d"));
            var graphDataUpdated = string.Format("{0},26,26#{1},25.1,26#{2},24.1,21", DateTime.Today.AddDays(-2).ToString("d"), DateTime.Today.AddDays(-1).ToString("d"), DateTime.Today.ToString("d"));
            testSprint2.GraphData = graphData;
            testSprint2.SprintEndDate = DateTime.Today;

            testUser1.CurrentWorkingSprint = testSprint1;
            testUser1.CurrentWorkingRelease = testRelease;
            testUser1.CurrentWorkingTeam = testTeam;
            
            testUser2.CurrentWorkingSprint = testSprint2;
            testUser2.CurrentWorkingRelease = testRelease;
            testUser2.CurrentWorkingTeam = testTeam;

            context.Add(testUser1);
            context.Add(testUser2);
            context.Add(testUserTeamMapping1);
            context.Add(testUserTeamMapping2);
            context.Add(testRelease);
            context.Add(testSprint1);
            context.Add(testSprint2);
            context.SaveChanges();

            // Act
            scrumAbleRepo.UpdateGraphDataActualPoints(2,DateTime.Parse("06/22/2021"),testUser1);
            scrumAbleRepo.UpdateGraphDataActualPoints(-2,DateTime.Today,testUser2);

            // Assert
            Assert.Equal("6/21/2021,26,26#6/22/2021,25.1,28#6/23/2021,24.1,25", testSprint1.GraphData);
            Assert.Equal(graphDataUpdated, testSprint2.GraphData);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT87_ScrumAbleRepo_GetVelocityData_ShouldReturnCorrectSprintData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT87")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new ScrumAbleRepo(context);
            var testUser1 = MockScrumAbleUser.GenerateScrumAbleUser();
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUserTeamMapping1 = MockScrumAbleUserTeamMapping.GenerateScrumAbleUserTeamMapping(testUser1, testTeam);
            var testRelease = MockScrumAbleRelease.GenerateRelease(testTeam);
            var testSprint1 = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testSprint2 = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testSprint3 = MockScrumAbleSprint.GenerateSprint(testRelease);
            
            var today = DateTime.Today;
            var twoWeeksAgo = DateTime.Today.AddDays(-14);

            
            testSprint1.SprintPlanned = 25;
            testSprint1.SprintActual = 20;
            testSprint1.IsActiveSprint = true;

            testSprint2.SprintPlanned = 35;
            testSprint2.SprintActual = 30;
            testSprint2.IsCompleted = true;

            testSprint3.SprintPlanned = 45;
            testSprint3.SprintActual = 40;
            testSprint3.IsCompleted = false;

            testUser1.CurrentWorkingSprint = testSprint1;
            testUser1.CurrentWorkingRelease = testRelease;
            testUser1.CurrentWorkingTeam = testTeam;

            context.Add(testUser1);
            context.Add(testUserTeamMapping1);
            context.Add(testRelease);
            context.Add(testSprint1);
            context.Add(testSprint2);
            context.Add(testSprint3);
            context.SaveChanges();

            // Act
            var result = scrumAbleRepo.GetVelocityData(testUser1);

            // Assert
            Assert.Equal(testSprint1.SprintName, result[0]["sprintName"]);
            Assert.Equal(testSprint1.SprintPlanned.ToString(), result[0]["planned"]);
            Assert.Equal(testSprint1.SprintActual.ToString(), result[0]["actual"]);
            
            Assert.Equal(testSprint2.SprintName, result[1]["sprintName"]);
            Assert.Equal(testSprint2.SprintPlanned.ToString(), result[1]["planned"]);
            Assert.Equal(testSprint2.SprintActual.ToString(), result[1]["actual"]);

            Assert.Equal(2, result.Count);

            context.Database.EnsureDeleted();
        }



    }
}
