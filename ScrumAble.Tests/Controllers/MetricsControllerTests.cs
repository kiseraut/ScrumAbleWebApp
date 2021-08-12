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
    public class MetricsControllerTests
    {
        [Fact]
        public void UT84_MetricsController_Index_ShouldCreateBurndownGraphData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ScrumAbleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UT84")
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

            sprint.GraphData = "6/21/2021,26,26#6/22/2021,25.1,26#6/23/2021,24.1,23";

            context.Add(user);
            context.Add(release);
            context.Add(team);
            context.Add(sprint);
            context.SaveChanges();

            var metricsController = new MetricsController(scrumAbleRepo, null);

            metricsController = AddClaimsIdentityToController(metricsController, user);

            // Act
            var result = metricsController.Index() as ViewResult;
            var burndownData = metricsController.ViewBag.BurndownData;

            // Assert
            Assert.Equal("6/21/2021", burndownData[0]["date"]);
            Assert.Equal("26", burndownData[0]["planned"]);
            Assert.Equal("26", burndownData[0]["actual"]);
            context.Database.EnsureDeleted();
        }

        private MetricsController AddClaimsIdentityToController(MetricsController metricsController, MockScrumAbleUser user)
        {
            var claimsUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var controller = metricsController;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsUser }
            };

            return controller;
        }
    }
}
