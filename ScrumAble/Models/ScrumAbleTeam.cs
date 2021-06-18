using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleTeam : IScrumAbleTeam
    {
        public int Id { get; set; }

        //The display name of the team
        [Required(AllowEmptyStrings = false), Display(Name = "Team Name")]
        public string TeamName { get; set; }

        public ICollection<ScrumAbleRelease> Releases { get; set; }

        public ICollection<ScrumAbleUserTeamMapping> UserTeamMappings { get; set; }

        public ICollection<ScrumAbleWorkflowStage> WorkFlowStages { get; set; }
    }
}
