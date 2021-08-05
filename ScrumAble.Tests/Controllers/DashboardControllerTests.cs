using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Controllers;
using ScrumAble.Data;
using ScrumAble.Models;
using ScrumAble.Tests.Models;
using Xunit;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DashboardControllerTests
    {
        [Fact]
        public void UT75_DashboardController_Index_ShouldReturnSprintObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT75")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var dashboardController = new DashboardController(scrumAbleRepo, null);

            dashboardController = AddClaimsIdentityToController(dashboardController, user);

            // Act
            var result = dashboardController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockScrumAbleSprint>(result.Model);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT76_DashboardController_MoveWOrkItem_ShouldReturnFalseIfWorkflowStageIdIsInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT76")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var dashboardController = new DashboardController(scrumAbleRepo, null);

            dashboardController = AddClaimsIdentityToController(dashboardController, user);

            // Act
            var result = dashboardController.MoveWorkItem("story_card_01", "swimlane_abc");

            // Assert
            Assert.False(result);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT77_DashboardController_MoveWOrkItem_ShouldReturnFalseIfWorklistItemIdIsInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT77")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var dashboardController = new DashboardController(scrumAbleRepo, null);

            dashboardController = AddClaimsIdentityToController(dashboardController, user);

            // Act
            var result = dashboardController.MoveWorkItem("story_card_abc", "swimlane_01");

            // Assert
            Assert.False(result);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT78_DashboardController_MoveWOrkItem_ShouldReturnFalseIfWorklistItemTypeIsInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT78")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var dashboardController = new DashboardController(scrumAbleRepo, null);

            dashboardController = AddClaimsIdentityToController(dashboardController, user);

            // Act
            var result = dashboardController.MoveWorkItem("abc_card_01", "swimlane_01");

            // Assert
            Assert.False(result);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT79_DashboardController_MoveWOrkItem_ShouldReturnTrueIfItIsAbleToMoveAWorklistItem()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT79")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var dashboardController = new DashboardController(scrumAbleRepo, null);

            dashboardController = AddClaimsIdentityToController(dashboardController, user);

            // Act
            var storyResult = dashboardController.MoveWorkItem("story_card_01", "swimlane_01");
            var taskResult = dashboardController.MoveWorkItem("task_card_01", "swimlane_01");
            var defectResult = dashboardController.MoveWorkItem("defect_card_01", "swimlane_01");

            // Assert
            Assert.True(storyResult);
            Assert.True(taskResult);
            Assert.True(defectResult);
            context.Database.EnsureDeleted();
        }

        private DashboardController AddClaimsIdentityToController(DashboardController dashboardController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var controller = dashboardController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }
    }
}
