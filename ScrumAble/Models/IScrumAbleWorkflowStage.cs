using System.Collections.Generic;

namespace ScrumAble.Models
{
    public interface IScrumAbleWorkflowStage
    {
        int Id { get; set; }
        string WorkflowStageName { get; set; }
        int WorkflowStagePosition { get; set; }
        ScrumAbleTeam Team { get; set; }
        ICollection<ScrumAbleStory> Stories { get; set; }
        ICollection<ScrumAbleTask> Tasks { get; set; }
        ICollection<ScrumAbleDefect> Defects { get; set; }
        public List<ScrumAbleWorkflowStage> AssociatedWorkflowStages { get; set; }
    }
}