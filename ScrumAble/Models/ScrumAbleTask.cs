using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleTask
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false), Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Required(AllowEmptyStrings = false), Display(Name = "Task Due Date"), DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Points for this task")]
        public int Points { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        public int? StageId { get; set; }

        public int? SprintId { get; set; }

        [Required]
        public int ReleaseId { get; set; }

        [NotMapped, Display(Name = "Add Task to")]
        public string Destination { get; set; }


    }
}
