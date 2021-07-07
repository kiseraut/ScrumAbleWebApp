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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ScrumAble.Tests.Models;
using Xunit;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class TeamControllerTests
    {
        [Fact]
        public void UT29_TeamController_Details_ShouldReturnTeamNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT29")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var teamController = new TeamController(scrumAbleRepo, null);
            context.Add(MockScrumAbleTeam.GenerateTeam());
            context.SaveChanges();

            teamController = AddClaimsIdentityToController(teamController);

            // Act
            var result = teamController.Details(5000) as IActionResult;

            // Assert
            Assert.Equal("TeamNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT30_TeamController_CreateTeam_ShouldAddTeamToDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT30")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var teamController = new TeamController(scrumAbleRepo, null);
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            context.Add(testUser);
            context.SaveChanges();

            teamController = AddClaimsIdentityToController(teamController, testUser);

            // Act
            IActionResult result = teamController.CreateTeam(testTeam) as IActionResult;
            MockScrumAbleTeam teamDBItems = (MockScrumAbleTeam)await context.Teams.SingleAsync();

            // Assert
            Assert.NotNull(teamDBItems);
            Assert.Equal("Test Team", teamDBItems.TeamName);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async void UT31_TeamController_UpdateTeam_ShouldUpdateTeamInDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT31")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var teamController = new TeamController(scrumAbleRepo, null);
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            var testUser = MockScrumAbleUser.GenerateScrumAbleUser();
            context.Add(testTeam);
            context.Add(testUser);
            context.SaveChanges();

            testTeam.TeamName = "Updated Test Team";

            teamController = AddClaimsIdentityToController(teamController, testUser);

            // Act
            IActionResult result = teamController.UpdateTeam(testTeam) as IActionResult;
            MockScrumAbleTeam taskDBItems = (MockScrumAbleTeam)await context.Teams.SingleAsync();

            // Assert
            Assert.NotNull(taskDBItems);
            Assert.Equal("Updated Test Team", taskDBItems.TeamName);

            context.Database.EnsureDeleted();
        }


        [Fact]
        public void UT32_TeamController_DeleteTeam_ShouldDeleteTeamFromDb()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT32")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var teamController = new TeamController(scrumAbleRepo, null);
            var testTeam = MockScrumAbleTeam.GenerateTeam();
            context.Add(testTeam);
            context.SaveChanges();

            teamController = AddClaimsIdentityToController(teamController);

            // Act
            IActionResult result = teamController.DeleteTeam(testTeam.Id) as IActionResult;
            var numRecords = context.Teams.Count();

            // Assert
            Assert.Equal(0, numRecords);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void UT33_TeamController_EditTeam_ShouldReturnTeamNotFoundViewWhenIdIsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT33")
                .Options;

            var context = new ScrumAbleContext(options);
            var scrumAbleRepo = new MockScrumAbleRepo(context);
            var teamController = new TeamController(scrumAbleRepo, null);
            context.Add(MockScrumAbleTeam.GenerateTeam());
            context.SaveChanges();

            teamController = AddClaimsIdentityToController(teamController);

            // Act
            IActionResult result = teamController.EditTeam(5000) as IActionResult;

            // Assert
            Assert.Equal("TeamNotFound", ((ViewResult)result).ViewName);
            context.Database.EnsureDeleted();
        }

        private TeamController AddClaimsIdentityToController(TeamController teamController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));


            var controller = teamController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }


        private TeamController AddClaimsIdentityToController(TeamController teamController)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));


            var controller = teamController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }
    }
}
