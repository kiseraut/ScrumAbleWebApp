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
    class MockScrumAbleDefect : ScrumAbleDefect
    {
        public static ScrumAbleDefect GenerateDefect()
        {
            return new ScrumAbleDefect
            {
                DefectName = "Test Defect",
                DefectPoints = 6
            };
        }

        public static ScrumAbleDefect GenerateDefect(MockScrumAbleRelease release, MockScrumAbleSprint sprint)
        {
            return new ScrumAbleDefect
            {
                DefectName = "Test Defect",
                DefectPoints = 6,
                Release = release,
                Sprint = sprint
            };
        }
    }
}