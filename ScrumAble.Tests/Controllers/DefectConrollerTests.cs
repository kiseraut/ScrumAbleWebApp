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
    public class DefectConrollerTests
    {
        [Fact]
        public void UT69_DefectController_Details_ShouldReturnDefectNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT69")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var defectController = new DefectController(scrumAbleRepo, null);
            context.Add(MockScrumAbleDefect.GenerateDefect());
            context.SaveChanges();

            defectController = AddClaimsIdentityToController(defectController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = defectController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("DefectNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT70_DefectController_AddDefect_ShouldReturnDefectObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT70")
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

            var defectController = new DefectController(scrumAbleRepo, null);

            defectController = AddClaimsIdentityToController(defectController, user);

            // Act
            var result = defectController.AddDefect(MockScrumAbleDefect.GenerateDefect()) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScrumAbleDefect>(result.Model);
            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT71_DefectController_EditDefect_ShouldReturnDefectNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT71")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var defectController = new DefectController(scrumAbleRepo, null);
            context.Add(MockScrumAbleDefect.GenerateDefect());
            context.SaveChanges();

            defectController = AddClaimsIdentityToController(defectController, MockScrumAbleUser.GenerateScrumAbleUser());

            // Act
            var result = defectController.EditDefect(5000) as IActionResult;

            // Assert
            Assert.Equal("DefectNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void UT72_DefectController_DeleteDefect_ShouldDeleteDefectFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT72")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var defect = MockScrumAbleDefect.GenerateDefect();
            var story = MockScrumAbleStory.GenerateStory();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var team = MockScrumAbleTeam.GenerateTeam();
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            defect.Release = release;
            context.Add(defect);
            context.Add(story);
            context.Add(sprint);
            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.SaveChanges();

            var defectController = new DefectController(scrumAbleRepo, null);

            defectController = AddClaimsIdentityToController(defectController, user);

            // Act
            defectController.DeleteDefect(defect.Id);
            var numRecords = context.Defects.Count();

            // Assert
            Assert.Equal(0, numRecords);
            context.Database.EnsureDeleted();
        }
        

        [Fact]
        public async void UT73_DefectController_CreateDefect_ShouldAddDefectToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT73")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var story = MockScrumAbleStory.GenerateStory();
            var team = MockScrumAbleTeam.GenerateTeam();
            var defect = MockScrumAbleDefect.GenerateDefect(release, sprint);
            var workfowStage = MockScrumAbleWorkflowStage.GenerateWorkflowStage(team);
            story.Sprint = sprint;
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            context.Add(user);
            context.Add(workfowStage);
            context.Add(release);
            context.Add(team);
            context.Add(sprint);
            context.SaveChanges();

            var defectController = new DefectController(scrumAbleRepo, null);

            defectController = AddClaimsIdentityToController(defectController, user);

            // Act
            IActionResult result = defectController.CreateDefect(defect) as IActionResult;
            ScrumAbleDefect defectDbItems = (ScrumAbleDefect)await context.Defects.SingleAsync();

            // Assert
            Assert.NotNull(defectDbItems);
            Assert.Equal("Test Defect", defectDbItems.DefectName);

            context.Database.EnsureDeleted();
        }
        
        [Fact]
        public async void UT74_DefectController_UpdateDefect_ShouldUpdateDefectInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT74")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var sprint = MockScrumAbleSprint.GenerateSprint();
            var user = MockScrumAbleUser.GenerateScrumAbleUser();
            var release = MockScrumAbleRelease.GenerateRelease();
            var story = MockScrumAbleStory.GenerateStory();
            var team = MockScrumAbleTeam.GenerateTeam();
            var defect = MockScrumAbleDefect.GenerateDefect(release, sprint);
            story.Sprint = sprint;
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingTeam = team;
            user.CurrentWorkingSprint = sprint;
            defect.Sprint = sprint;
            defect.Release = release;
            

            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.Add(sprint);
            context.SaveChanges();

            defect.DefectSprintId = sprint.Id;
            defect.DefectReleaseId = release.Id;
            context.Add(defect);
            context.SaveChanges();

            defect.DefectName = "Updated Test Defect";
            var defectController = new DefectController(scrumAbleRepo, null);

            defectController = AddClaimsIdentityToController(defectController, user);

            // Act
            IActionResult result = defectController.UpdateDefect(defect) as IActionResult;
            ScrumAbleDefect storyDbItems = (ScrumAbleDefect)await context.Defects.SingleAsync();

            // Assert
            Assert.NotNull(storyDbItems);
            Assert.Equal("Updated Test Defect", storyDbItems.DefectName);

            context.Database.EnsureDeleted();
        }
        
        private DefectController AddClaimsIdentityToController(DefectController defectController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var controller = defectController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }


        private DefectController AddClaimsIdentityToController(DefectController defectController)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));


            var controller = defectController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }
    }
}
