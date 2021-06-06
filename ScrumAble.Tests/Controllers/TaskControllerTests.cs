using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ScrumAble.Controllers;
using ScrumAble.Data;
using ScrumAble.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class TaskControllerTests
    {

        [Fact]
        public void UT1_TaskController_ViewShouldLoad()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
            .UseInMemoryDatabase(databaseName: "TaskDatabase")
            .Options;

            var context = new ScrumAbleContext(options);
            var taskController = new TaskController(context);
            // Act
            IActionResult result = taskController.Index() as IActionResult;

            // Assert
            Assert.NotNull(result);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT2_TaskController_ProcessTaskShouldGetData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
            .UseInMemoryDatabase(databaseName: "TaskDatabase")
            .Options;

            var context = new ScrumAbleContext(options);
            var taskController = new TaskController(context);
            var testTask = new ScrumAbleTask();

            testTask.TaskName = "Test Task";
            testTask.Points = 5;
            testTask.DueDate = new DateTime(2021, 6, 5);
            testTask.Description = "This is a tesk Task";
            testTask.Destination = "Sprint";           

            // Act
            IActionResult result = taskController.AddToSprint(testTask) as IActionResult;
            ScrumAbleTask taskDBItems = await context.Tasks.SingleAsync();            

            // Assert
            Assert.NotNull(taskDBItems);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT3_TaskController_ProcessTaskShouldAddSprintTaskToSprint()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
            .UseInMemoryDatabase(databaseName: "TaskDatabase")
            .Options;

            var context = new ScrumAbleContext(options);
            var taskController = new TaskController(context);
            var testTask = new ScrumAbleTask();

            testTask.TaskName = "Test Task";
            testTask.Points = 5;
            testTask.DueDate = new DateTime(2021, 6, 5);
            testTask.Description = "This is a tesk Task";
            testTask.Destination = "Sprint";

            // Act
            IActionResult result = taskController.AddToSprint(testTask) as IActionResult;            

            // Assert
            Assert.NotNull(testTask.SprintId);
            Assert.NotNull(testTask.StageId);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT4_TaskController_ProcessTaskShouldAddBacklogTaskToBacklog()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
            .UseInMemoryDatabase(databaseName: "TaskDatabase")
            .Options;

            var context = new ScrumAbleContext(options);
            var taskController = new TaskController(context);
            var testTask = new ScrumAbleTask();

            testTask.TaskName = "Test Task";
            testTask.Points = 5;
            testTask.DueDate = new DateTime(2021, 6, 5);
            testTask.Description = "This is a tesk Task";
            testTask.Destination = "Backlog";

            // Act
            IActionResult result = taskController.AddToSprint(testTask) as IActionResult;

            // Assert
            Assert.Null(testTask.SprintId);
            Assert.Null(testTask.StageId);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT5_TaskController_ProcessTaskShouldValidateTheForm()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
            .UseInMemoryDatabase(databaseName: "TaskDatabase")
            .Options;

            var context = new ScrumAbleContext(options);
            var taskController = new TaskController(context);
            var testTask = new ScrumAbleTask();

            testTask.TaskName = null;
            testTask.Points = 5;
            testTask.DueDate = new DateTime(2021, 6, 5);
            testTask.Description = "This is a tesk Task";
            testTask.Destination = "Backlog";

            taskController.ModelState.AddModelError("TaskName", "The Task Name field is required.");

            // Act
            IActionResult result = taskController.AddToSprint(testTask) as IActionResult;

            // Assert
            Assert.Equal("Index", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }


    }
}
