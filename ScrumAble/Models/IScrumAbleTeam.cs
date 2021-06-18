using System.Collections.Generic;

namespace ScrumAble.Models
{
    public interface IScrumAbleTeam
    {
        int Id { get; set; }
        string TeamName { get; set; }
        ICollection<ScrumAbleRelease> Releases { get; set; }
        ICollection<ScrumAbleUserTeamMapping> UserTeamMappings { get; set; }
        ICollection<ScrumAbleWorkflowStage> WorkFlowStages { get; set; }
    }
}