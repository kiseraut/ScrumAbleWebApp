using System.Diagnostics.CodeAnalysis;
using ScrumAble.Models;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleTeam : ScrumAbleTeam
    {
        public static MockScrumAbleTeam GenerateTeam()
        {
            return new MockScrumAbleTeam()
            {
                TeamName = "Test Team"
            };
        }
    }
}