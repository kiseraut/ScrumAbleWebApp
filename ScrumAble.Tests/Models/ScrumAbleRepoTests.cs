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
            Assert.Equal(testTask.Id, result.Id);
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
            var testStory= MockScrumAbleStory.GenerateStory();
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
        
    }
}
