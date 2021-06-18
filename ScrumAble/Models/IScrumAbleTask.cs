using System;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IScrumAbleTask
    {
        int Id { get; set; }
        string TaskName { get; set; }
        ScrumAbleUser TaskOwner { get; set; }
        DateTime? TaskStartDate { get; set; }
        DateTime? TaskDueDate { get; set; }
        DateTime? TaskCloseDate { get; set; }
        int? TaskPoints { get; set; }
        String TaskDescription { get; set; }
        ScrumAbleWorkflowStage WorkflowStage { get; set; }
        ScrumAbleSprint Sprint { get; set; }
        ScrumAbleStory Story { get; set; }
        ViewModelTaskAggregate viewModelTaskAggregate { get; set; }
        string? TaskOwnerId { get; set; }
        int? TaskSprintId { get; set; }
        int? TaskStoryId { get; set; }
        string Destination { get; set; }
    }
}