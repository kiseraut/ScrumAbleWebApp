using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Models;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleUserTeamMapping : ScrumAbleUserTeamMapping
    {

        public static MockScrumAbleUserTeamMapping GenerateScrumAbleUserTeamMapping(MockScrumAbleUser user, MockScrumAbleTeam team)
        {
            return new MockScrumAbleUserTeamMapping()
            {
                User = user,
                Team = team
            };
        }

    }
}
