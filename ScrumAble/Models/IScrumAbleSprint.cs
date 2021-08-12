using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace ScrumAble.Models
{
    public interface IScrumAbleSprint
    {
        int Id { get; set; }
        string SprintName { get; set; }
        DateTime SprintStartDate { get; set; }
        DateTime SprintEndDate { get; set; }
        ScrumAbleRelease Release { get; set; }
        ICollection<ScrumAbleStory> Stories { get; set; }
        ICollection<ScrumAbleTask> Tasks { get; set; }
        public ICollection<ScrumAbleDefect> Defects { get; set; }
        public List<ScrumAbleWorkflowStage> WorkflowStages { get; set; }
        bool IsBacklog { get; set; }
        bool IsActiveSprint { get; set; }
        bool IsCompleted { get; set; }
        int SprintPlanned { get; set; }
        int SprintActual { get; set; }
        string GraphData { get; set; }
    }
}