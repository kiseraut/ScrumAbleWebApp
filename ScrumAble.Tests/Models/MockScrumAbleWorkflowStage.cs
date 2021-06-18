using System;
using System.Diagnostics.CodeAnalysis;
using ScrumAble.Models;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleWorkflowStage : ScrumAbleWorkflowStage
    {
        public static MockScrumAbleWorkflowStage GenerateWorkflowStage()
        {
            return new MockScrumAbleWorkflowStage()
            {
                WorkflowStageName = "Test Workflow Stage",
                WorkflowStagePosition = 0,
                Team = MockScrumAbleTeam.GenerateTeam()
            };
        }

        public static MockScrumAbleWorkflowStage GenerateWorkflowStage(MockScrumAbleTeam team)
        {
            return new MockScrumAbleWorkflowStage()
            {
                WorkflowStageName = "Test Workflow Stage",
                WorkflowStagePosition = 0,
                Team = team
            };
        }
    }
}