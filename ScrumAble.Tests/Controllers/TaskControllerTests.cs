using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Controllers;
using ScrumAble.Data;
//using ScrumAble.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ScrumAble.Tests.Models;
using Xunit;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class TaskControllerTests
    {

        [Fact]
        public void UT1_TaskController_Details_ShouldReturnTaskNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT1")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            context.Add(MockScrumAbleTask.GenerateTask());
            context.SaveChanges();

            // Act
            var result = taskController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("TaskNotFound", ((ViewResult) result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT2_TaskController_CreateTask_ShouldAddTaskToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT2")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            var testTask = MockScrumAbleTask.GenerateTask();
            testTask.TaskSprintId = -1;

            // Act
            IActionResult result = taskController.CreateTask(testTask) as IActionResult;
            MockScrumAbleTask taskDBItems = (MockScrumAbleTask) await context.Tasks.SingleAsync();

            // Assert
            Assert.NotNull(taskDBItems);
            Assert.Equal("Test Task", taskDBItems.TaskName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT3_TaskController_CreateTask_ShouldValidateUserInput()
        {
            // Arrange
            var testTask = new MockScrumAbleTask
            {
                TaskName = null,
                TaskPoints = null,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint"
            };

            // Act
            var validationContext = new ValidationContext(testTask, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(testTask, validationContext, results, true);

            // Assert
            Assert.False(isModelStateValid);
        }

        [Fact]
        public void UT4_TaskController_EditTask_ShouldReturnTaskNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT4")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            context.Add(MockScrumAbleTask.GenerateTask());
            context.SaveChanges();

            // Act
            IActionResult result = taskController.EditTask(5000) as IActionResult;

            // Assert
            Assert.Equal("TaskNotFound", ((ViewResult) result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT5_TaskController_Details_ShouldReturnCorrectTask()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT5")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            var testTask1 = new MockScrumAbleTask
            {
                TaskName = "Test Task 1",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint"
            };
            var testTask2 = new MockScrumAbleTask
            {
                TaskName = "Test Task 2",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint"
            };
            context.Add(testTask1);
            context.Add(testTask1);
            context.SaveChanges();


            // Act
            var result = taskController.Details(testTask1.Id) as ViewResult;
            MockScrumAbleTask model = result.ViewData.Model as MockScrumAbleTask;

            // Assert
            Assert.Equal(testTask1.Id, model.Id);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT6_TaskController_UpdateTask_ShouldUpdateTaskInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT6")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            var testTask = MockScrumAbleTask.GenerateTask();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            var testSprint = MockScrumAbleSprint.GenerateSprint(testRelease);
            var testStory = MockScrumAbleStory.GenerateStory();
            testTask.TaskOwnerId = testUser.Id;
            testTask.TaskSprintId = testSprint.Id;
            testTask.TaskStoryId = testStory.Id;

            context.Add(testTask);
            context.SaveChanges();

            testTask.TaskName = "Updated Test Task";
            
            // Act
            IActionResult result = taskController.UpdateTask(testTask) as IActionResult;
            MockScrumAbleTask taskDBItems = (MockScrumAbleTask) await context.Tasks.SingleAsync();

            // Assert
            Assert.NotNull(taskDBItems);
            Assert.Equal("Updated Test Task", taskDBItems.TaskName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT7_TaskController_DeleteTask_ShouldDeleteTaskFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT7")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            var testTask = MockScrumAbleTask.GenerateTask();
            context.Add(testTask);
            context.SaveChanges();

            // Act
            IActionResult result = taskController.DeleteTask(testTask.Id) as IActionResult;
            var numRecords = context.Tasks.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        /*
        [Fact]
        public void UT?_TaskController_EditTask_ShouldReturnCorrectTask()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TaskDatabase")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var taskController = new TaskController(scrumAbleRepo, null);
            var testTask1 = new ScrumAbleTask
            {
                TaskName = "Test Task 1",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint"
            };
            var testTask2 = new ScrumAbleTask
            {
                TaskName = "Test Task 2",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint"
            };
            context.Add(testTask1);
            context.Add(testTask1);
            context.SaveChanges();

            


            // Act
            var result = taskController.EditTask(testTask1.Id) as ViewResult;
            ScrumAbleTask model = result.ViewData.Model as ScrumAbleTask;

            // Assert
            Assert.Equal(testTask1.Id, model.Id);
            context.Database.EnsureDeleted();
        }*/

    }

}
