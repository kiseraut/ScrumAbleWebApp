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
    public class SprintControllerTests
    {
        [Fact]
        public void UT36_SprintController_Details_ShouldReturnSprintNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT36")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprintController = new SprintController(scrumAbleRepo, null);
            context.Add(MockScrumAbleSprint.GenerateSprint());
            context.SaveChanges();

            sprintController = AddClaimsIdentityToController(sprintController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = sprintController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("SprintNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT37_SprintController_AddSprint_ShouldReturnSprintObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT37")
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

            var sprintController = new SprintController(scrumAbleRepo, null);

            sprintController = AddClaimsIdentityToController(sprintController, user);

            // Act
            var result = sprintController.AddSprint(MockScrumAbleSprint.GenerateSprint()) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockScrumAbleSprint>(result.Model);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT38_SprintController_EditSprint_ShouldReturnSPrintNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT38")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprintController = new SprintController(scrumAbleRepo, null);
            context.Add(MockScrumAbleSprint.GenerateSprint());
            context.SaveChanges();

            sprintController = AddClaimsIdentityToController(sprintController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = sprintController.EditSprint(5000) as IActionResult;

            // Assert
            Assert.Equal("SprintNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT39_SprintController_AddSprint_ShouldReturnSprintObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT39")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(sprint);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var sprintController = new SprintController(scrumAbleRepo, null);

            sprintController = AddClaimsIdentityToController(sprintController, user);

            // Act
            sprintController.DeleteSprint(sprint.Id);
            var numRecords = context.Sprints.Count();

            // Assert
            Assert.Equal(0, numRecords);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT40_SprintController_SetCurrentSprint_ShouldSetTheUsersCurrentSprint()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT40")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(sprint);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var sprintController = new SprintController(scrumAbleRepo, null);

            sprintController = AddClaimsIdentityToController(sprintController, user);

            // Act
            sprintController.SetCurrentSprint(sprint.Id);

            // Assert
            Assert.Equal(sprint, user.CurrentWorkingSprint);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT41_SprintController_CreateSprint_ShouldAddSprintToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT41")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var sprintController = new SprintController(scrumAbleRepo, null);

            sprintController = AddClaimsIdentityToController(sprintController, user);

            // Act
            IActionResult result = sprintController.CreateSprint(sprint) as IActionResult;
            MockScrumAbleSprint sprintDbItems = (MockScrumAbleSprint)await context.Sprints.SingleAsync();

            // Assert
            Assert.NotNull(sprintDbItems);
            Assert.Equal("Test Sprint", sprintDbItems.SprintName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT42_SprintController_UpdateSprint_ShouldUpdateSprintInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT42")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(sprint);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            sprint.SprintName = "Updated Test Sprint";
            var sprintController = new SprintController(scrumAbleRepo, null);

            sprintController = AddClaimsIdentityToController(sprintController, user);

            // Act
            IActionResult result = sprintController.UpdateSprint(sprint) as IActionResult;
            MockScrumAbleSprint sprintDbItems = (MockScrumAbleSprint)await context.Sprints.SingleAsync();

            // Assert
            Assert.NotNull(sprintDbItems);
            Assert.Equal("Updated Test Sprint", sprintDbItems.SprintName);

            context.Database.EnsureDeleted();
        }



        private SprintController AddClaimsIdentityToController(SprintController sprintController, MockScrumAbleUser user)
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


        private SprintController AddClaimsIdentityToController(SprintController sprintController)
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
