using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumAble.Models;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleSprint : ScrumAbleSprint
    {
        public static MockScrumAbleSprint GenerateSprint()
        {
            return new MockScrumAbleSprint
            {
                SprintName ="Test Sprint",
                SprintStartDate = new DateTime(2021, 06, 01),
                SprintEndDate = new DateTime(2021, 06, 14),
                Release =  MockScrumAbleRelease.GenerateRelease()
            };
        }

        public static MockScrumAbleSprint GenerateSprint(MockScrumAbleRelease release)
        {
            return new MockScrumAbleSprint
            {
                SprintName = "Test Sprint",
                SprintStartDate = new DateTime(2021, 06, 01),
                SprintEndDate = new DateTime(2021, 06, 14),
                Release = release
            };
        }
    }
}
