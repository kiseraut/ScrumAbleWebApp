﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleWorkflowStage : IScrumAbleWorkflowStage
    {
        public int Id { get; set; }

        //The display name of the workflow stage
        [Required(AllowEmptyStrings = false), Display(Name = "Workflow Stage Name")]
        public string WorkflowStageName { get; set; }

        //The position of the workflow stage releative to other workflow stages for this team
        [Required(AllowEmptyStrings = false), Display(Name = "Workflow Stage Position")]
        public int WorkflowStagePosition { get; set; }

        //The Team that will be using this workflow stage
        [Required(AllowEmptyStrings = false), Display(Name = "Team")]
        public ScrumAbleTeam Team { get; set; }

        public ICollection<ScrumAbleStory> Stories { get; set; }

        public ICollection<ScrumAbleTask> Tasks { get; set; }
    }
}
