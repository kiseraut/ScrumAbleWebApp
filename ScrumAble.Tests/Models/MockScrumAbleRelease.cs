using System;
using System.Diagnostics.CodeAnalysis;
using ScrumAble.Models;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleRelease : ScrumAbleRelease
    {
        public static MockScrumAbleRelease GenerateRelease()
        {
            return new MockScrumAbleRelease
            {
                ReleaseName = "Test Release",
                ReleaseStartDate = new DateTime(2021, 06, 01),
                Team = MockScrumAbleTeam.GenerateTeam()
            };
        }

        public static MockScrumAbleRelease GenerateRelease(MockScrumAbleTeam team)
        {
            return new MockScrumAbleRelease
            {
                ReleaseName = "Test Release",
                ReleaseStartDate = new DateTime(2021, 06, 01),
                Team = team
            };
        }
    }
}