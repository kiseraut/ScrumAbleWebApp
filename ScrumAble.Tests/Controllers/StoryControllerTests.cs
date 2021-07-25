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
    public class StoryControllerTests
    {
        [Fact]
        public void UT63_StoryController_Details_ShouldReturnStoryNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT63")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var storyController = new StoryController(scrumAbleRepo, null);
            context.Add(MockScrumAbleStory.GenerateStory());
            context.SaveChanges();

            storyController = AddClaimsIdentityToController(storyController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = storyController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("StoryNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT64_StoryController_AddStory_ShouldReturnStoryObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT64")
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

            var storyController = new StoryController(scrumAbleRepo, null);

            storyController = AddClaimsIdentityToController(storyController, user);

            // Act
            var result = storyController.AddStory(MockScrumAbleStory.GenerateStory()) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockScrumAbleStory>(result.Model);
            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT65_StoryController_EditStory_ShouldReturnStoryNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT65")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var storyController = new StoryController(scrumAbleRepo, null);
            context.Add(MockScrumAbleStory.GenerateStory());
            context.SaveChanges();

            storyController = AddClaimsIdentityToController(storyController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = storyController.EditStory(5000) as IActionResult;

            // Assert
            Assert.Equal("StoryNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT66_StoryController_DeleteStory_ShouldDeleteStoryFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT66")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var story = MockScrumAbleStory.GenerateStory();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            story.Sprint = sprint;
            context.Add(story);
            context.Add(sprint);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var storyController = new StoryController(scrumAbleRepo, null);

            storyController = AddClaimsIdentityToController(storyController, user);

            // Act
            storyController.DeleteStory(story.Id);
            var numRecords = context.Stories.Count();

            // Assert
            Assert.Equal(0, numRecords);
            context.Database.EnsureDeleted();
        }
        
       
        [Fact]
        public async void UT67_StoryController_CreateStory_ShouldAddStoryToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT67")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var story = MockScrumAbleStory.GenerateStory();
            var team = MockScrumAbleTeam.GenerateTeam();
            story.Sprint = sprint;
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.Add(sprint);
            context.SaveChanges();

            var storyController = new StoryController(scrumAbleRepo, null);

            storyController = AddClaimsIdentityToController(storyController, user);

            // Act
            IActionResult result = storyController.CreateStory(story) as IActionResult;
            MockScrumAbleStory storyDbItems = (MockScrumAbleStory)await context.Stories.SingleAsync();

            // Assert
            Assert.NotNull(storyDbItems);
            Assert.Equal("Test Story", storyDbItems.StoryName);

            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public async void UT68_StoryController_UpdateStory_ShouldUpdateStoryInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT68")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var story = MockScrumAbleStory.GenerateStory();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var team = MockScrumAbleTeam.GenerateTeam();
            story.Sprint = sprint;
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            context.Add(story);
            context.Add(sprint);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            story.StoryName = "Updated Test Story";
            var storyController = new StoryController(scrumAbleRepo, null);

            storyController = AddClaimsIdentityToController(storyController, user);

            // Act
            IActionResult result = storyController.UpdateStory(story) as IActionResult;
            MockScrumAbleStory storyDbItems = (MockScrumAbleStory)await context.Stories.SingleAsync();

            // Assert
            Assert.NotNull(storyDbItems);
            Assert.Equal("Updated Test Story", storyDbItems.StoryName);

            context.Database.EnsureDeleted();
        }

        private StoryController AddClaimsIdentityToController(StoryController storyController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var controller = storyController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }


        private StoryController AddClaimsIdentityToController(StoryController storyController)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));


            var controller = storyController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }
    }
}
