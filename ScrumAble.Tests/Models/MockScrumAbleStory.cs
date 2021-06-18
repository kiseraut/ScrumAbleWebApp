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
    class MockScrumAbleStory : ScrumAbleStory
    {
        public static ScrumAbleStory GenerateStory()
        {
            return new MockScrumAbleStory
            {
                StoryName = "Test Story",
                StoryPoints = 6
            };
        }
    }
}
