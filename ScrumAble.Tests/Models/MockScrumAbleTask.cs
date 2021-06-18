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
    public class MockScrumAbleTask : ScrumAbleTask
    {
        public static MockScrumAbleTask GenerateTask()
        {
            return new MockScrumAbleTask
            {
                TaskName = "Test Task",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint"
            };
        }

        public static MockScrumAbleTask GenerateTask(MockScrumAbleUser user)
        {
            return new MockScrumAbleTask
            {
                TaskName = "Test Task",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint",
                TaskOwner = user
            };
        }

        public static MockScrumAbleTask GenerateTask(ScrumAbleStory story, MockScrumAbleWorkflowStage workflowStage, MockScrumAbleSprint sprint, MockScrumAbleUser user)
        {
            return new MockScrumAbleTask
            {
                TaskName = "Test Task",
                TaskPoints = 5,
                TaskDueDate = new DateTime(2021, 6, 5),
                TaskDescription = "This is a test Task",
                Destination = "Sprint",
                Story = story,
                WorkflowStage = workflowStage,
                Sprint = sprint,
                TaskOwner = user
            };
        }
    }
}
