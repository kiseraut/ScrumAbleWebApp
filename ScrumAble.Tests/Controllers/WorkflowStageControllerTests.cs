using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScrumAble.Controllers;
using ScrumAble.Data;
using ScrumAble.Tests.Models;
using Xunit;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class WorkflowStageControllerTests
    {
        [Fact]
        public void UT53_WorkflowStageController_Details_ShouldReturnWorkflowStageNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT53")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var workflowStageController = new WorkflowStageController(scrumAbleRepo, null);
            context.Add(MockScrumAbleWorkflowStage.GenerateWorkflowStage());
            context.SaveChanges();

            workflowStageController = AddClaimsIdentityToController(workflowStageController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = workflowStageController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("WorkflowStageNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT54_WorkflowStageController_AddWorkflowStage_ShouldReturnWorkflowStageObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT54")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var workflowStageController = new WorkflowStageController(scrumAbleRepo, null);

            workflowStageController = AddClaimsIdentityToController(workflowStageController, user);

            // Act
            var result = workflowStageController.AddWorkflowStage(MockScrumAbleWorkflowStage.GenerateWorkflowStage()) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockScrumAbleWorkflowStage>(result.Model);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT55_WorkflowStageController_EditWorkflowStage_ShouldReturnWorkflowStageNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT53")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var workflowStageController = new WorkflowStageController(scrumAbleRepo, null);
            context.Add(MockScrumAbleWorkflowStage.GenerateWorkflowStage());
            context.SaveChanges();

            workflowStageController = AddClaimsIdentityToController(workflowStageController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = workflowStageController.EditWorkflowStage(5000) as IActionResult;

            // Assert
            Assert.Equal("WorkflowStageNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT56_WorkflowStageController_DeleteWorkflowStage_ShouldDeleteWorkflowStageFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT56")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var workflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            // var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            // user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(workflowStage);
            context.Add(user);
            // context.Add(release);
            // context.Add(team);
            context.SaveChanges();

            var workflowStageController = new WorkflowStageController(scrumAbleRepo, null);

            workflowStageController = AddClaimsIdentityToController(workflowStageController, user);

            // Act
            workflowStageController.DeleteWorkflowStage(workflowStage.Id);
            var numRecords = context.WorkflowStages.Count();

            // Assert
            Assert.Equal(0, numRecords);
            context.Database.EnsureDeleted();
        }
        /// <summary>
        /// ///////////////////////
        /// </summary>
        [Fact]
        public async void UT57_WorkflowStageController_CreateWorkflowStage_ShouldAddWorkflowStageToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT57")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var workflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage();

            var workflowStageController = new WorkflowStageController(scrumAbleRepo, null);

            workflowStageController = AddClaimsIdentityToController(workflowStageController, user);

            // Act
            IActionResult result = workflowStageController.CreateWorkflowStage(workflowStage) as IActionResult;
            MockScrumAbleWorkflowStage workflowStageDbItems = (MockScrumAbleWorkflowStage)await context.WorkflowStages.SingleAsync();

            // Assert
            Assert.NotNull(workflowStageDbItems);
            Assert.Equal("Test Workflow Stage", workflowStageDbItems.WorkflowStageName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT58_WorkflowStageController_UpdateWorkflowStage_ShouldUpdateWorkflowStageInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT58")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var workflowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(workflowStage);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            workflowStage.WorkflowStageName = "Updated Test Workflow Stage";
            var workflowStageController = new WorkflowStageController(scrumAbleRepo, null);

            workflowStageController = AddClaimsIdentityToController(workflowStageController, user);

            // Act
            IActionResult result = workflowStageController.UpdateWorkflowStage(workflowStage) as IActionResult;
            MockScrumAbleWorkflowStage workflowStageDbItems = (MockScrumAbleWorkflowStage)await context.WorkflowStages.SingleAsync();

            // Assert
            Assert.NotNull(workflowStageDbItems);
            Assert.Equal("Updated Test Workflow Stage", workflowStageDbItems.WorkflowStageName);

            context.Database.EnsureDeleted();
        }

        private WorkflowStageController AddClaimsIdentityToController(WorkflowStageController sprintController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var controller = sprintController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }


        private WorkflowStageController AddClaimsIdentityToController(WorkflowStageController sprintController)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));


            var controller = sprintController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }

    }
}
