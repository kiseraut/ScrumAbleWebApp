using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleSprint : IScrumAbleSprint
    {
        public int Id { get; set; }

        //The display name of the sprint
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Name")]
        public string SprintName { get; set; }

        //The date that the sprint is started
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Start Date"), DataType(DataType.Date)]
        public DateTime SprintStartDate { get; set; }

        //The date the sprint is over
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint End Date"), DataType(DataType.Date)]
        public DateTime SprintEndDate { get; set; }

        //The Releases that this sprint it in
        [Required(AllowEmptyStrings = false), Display(Name = "Release")]
        public ScrumAbleRelease Release { get; set; }

        public ICollection<ScrumAbleStory> Stories { get; set; }

        public ICollection<ScrumAbleTask> Tasks { get; set; }

        public ICollection<ScrumAbleDefect> Defects { get; set; }

        [NotMapped]
        public List<ScrumAbleWorkflowStage> WorkflowStages { get; set; }

        public bool IsBacklog { get; set; }

        public bool IsActiveSprint { get; set; }

        public bool IsCompleted { get; set; }
        public int SprintPlanned { get; set; }
        public int SprintActual { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string GraphData { get; set; }

        [Display(Name = "Release")]
        [NotMapped]
        public int SprintReleaseId { get; set; }
    }
}
