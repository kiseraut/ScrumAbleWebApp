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

        //The display name of the task
        [Required(AllowEmptyStrings = false), Display(Name = "Task Name")]
        public string TaskName { get; set; }

        //The owner of the task
        [Display(Name = "Task Owner")]
        public string TaskOwner { get; set; }

        //The date that the task was started i.e. moved into progress
        [Required(AllowEmptyStrings = false), Display(Name = "Task Start Date"), DataType(DataType.Date)]
        public DateTime TaskStartDate { get; set; }

        //The date the task is due to be completed
        [Required(AllowEmptyStrings = false), Display(Name = "Task Due Date"), DataType(DataType.Date)]
        public DateTime TaskDueDate { get; set; }

        //The date the task was actually finished
        [DataType(DataType.Date)]
        public DateTime TaskCloseDate { get; set; }

        //The number of points assgned to this task
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Points For This Task")]
        public int TaskPoints { get; set; }

        //The description of the task
        [Display(Name = "Description")]
        public String TaskDescription { get; set; }

        //The ID of the stage (swim lane) that the task is in
        public int? WorkflowStageID { get; set; }

        //The ID of the sprint this task is in
        [Display(Name = "Add Task To Sprint")]
        public int? SprintId { get; set; }

        //The ID of the story this task is in
        [Display(Name = "Add Task To Story")]
        public int? StoryID { get; set; }

        //Unused?
        [NotMapped, Display(Name = "Add Task To")]
        public string Destination { get; set; }


    }
}
