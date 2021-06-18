using System;
using System.Collections.Generic;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IScrumAbleStory
    {
        int Id { get; set; }
        string StoryName { get; set; }
        ScrumAbleUser StoryOwner { get; set; }
        DateTime? StoryStartDate { get; set; }
        DateTime? StoryDueDate { get; set; }
        DateTime? StoryCloseDate { get; set; }
        int StoryPoints { get; set; }
        String StoryDescription { get; set; }
        ScrumAbleWorkflowStage WorkflowStage { get; set; }
        ScrumAbleSprint Sprint { get; set; }
        ICollection<ScrumAbleTask> Tasks { get; set; }
    }
}