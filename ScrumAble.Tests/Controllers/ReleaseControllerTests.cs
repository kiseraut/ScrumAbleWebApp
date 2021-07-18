using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Controllers;
using ScrumAble.Data;
using ScrumAble.Tests.Models;
using Xunit;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ReleaseControllerTests
    {
        [Fact]
        public void UT26_ReleaseController_Details_ShouldReturnReleaseNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT26")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var releaseController = new ReleaseController(scrumAbleRepo, null);
            context.Add(MockScrumAbleRelease.GenerateRelease());
            context.SaveChanges();

            releaseController = AddClaimsIdentityToController(releaseController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = releaseController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("ReleaseNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT27_ReleaseController_CreateRelease_ShouldAddReleaseToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT27")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var releaseController = new ReleaseController(scrumAbleRepo, null);
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            context.Add(user);
            context.SaveChanges();

            releaseController = AddClaimsIdentityToController(releaseController, user);

            // Act
            IActionResult result = releaseController.CreateRelease(testRelease) as IActionResult;
            MockScrumAbleRelease taskDbItems = (MockScrumAbleRelease)await context.Releases.SingleAsync();

            // Assert
            Assert.NotNull(taskDbItems);
            Assert.Equal("Test Release", taskDbItems.ReleaseName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT28_ReleaseController_UpdateRelease_ShouldUpdateReleaseInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT28")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var releaseController = new ReleaseController(scrumAbleRepo, null);
            releaseController = AddClaimsIdentityToController(releaseController, MockScrumAbleUser.GenerateScrumAbleUser());
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            context.Add(testRelease);
            context.SaveChanges();

            var updatedTestRelease = new MockScrumAbleRelease()
            {
                Id = testRelease.Id,
                ReleaseStartDate = testRelease.ReleaseStartDate,
                ReleaseEndDate = testRelease.ReleaseEndDate,
                ReleaseName = "Updated Test Release"
            };
            
            // Act
            releaseController.UpdateRelease(updatedTestRelease);
            var releaseDbItems = context.Releases.Single();

            // Assert
            Assert.NotNull(releaseDbItems);
            Assert.Equal("Updated Test Release", releaseDbItems.ReleaseName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT34_ReleaseController_EditRelease_ShouldReturnReleaseNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT34")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var releaseController = new ReleaseController(scrumAbleRepo, null);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            context.Add(user);
            context.Add(MockScrumAbleRelease.GenerateRelease());
            context.SaveChanges();

            releaseController = AddClaimsIdentityToController(releaseController, user);

            // Act
            IActionResult result = releaseController.EditRelease(5000) as IActionResult;

            // Assert
            Assert.Equal("ReleaseNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT35_ReleaseController_DeleteRelease_ShouldDeleteReleaseFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT35")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var releaseController = new ReleaseController(scrumAbleRepo, null);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            context.Add(user);
            context.Add(testRelease);
            context.SaveChanges();

            releaseController = AddClaimsIdentityToController(releaseController, user);

            // Act
            IActionResult result = releaseController.DeleteRelease(testRelease.Id) as IActionResult;
            var numRecords = context.Releases.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT43_SprintController_AddRelease_ShouldReturnReleaseObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT43")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var releaseController = new ReleaseController(scrumAbleRepo, null);
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var team = MockScrumAbleTeam.GenerateTeam();
            var testRelease = MockScrumAbleRelease.GenerateRelease();
            user.CurrentWorkingTeam = team;
            context.Add(user);
            context.Add(team);
            context.SaveChanges();

            releaseController = AddClaimsIdentityToController(releaseController, user);

            // Act
            var result = releaseController.AddRelease(MockScrumAbleRelease.GenerateRelease()) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockScrumAbleRelease>(result.Model);
            context.Database.EnsureDeleted();
        }






        private ReleaseController AddClaimsIdentityToController(ReleaseController releaseController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));


            var controller = releaseController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }
    }
}
