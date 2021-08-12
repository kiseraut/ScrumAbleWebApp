using ScrumAble.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleStory : IScrumAbleStory
    {
        public int Id { get; set; }

        //The display name of the story
        [Required(AllowEmptyStrings = false), Display(Name = "Story Name")]
        public string StoryName { get; set; }

        //The owner of the story
        [Display(Name = "Story Owner")]
        public ScrumAbleUser StoryOwner { get; set; }

        //The date that the story was started i.e. moved into progress
        [Display(Name = "Story Start Date"), DataType(DataType.Date)]
        public DateTime? StoryStartDate { get; set; }

        //The date the story is due to be completed
        [Display(Name = "Story Due Date"), DataType(DataType.Date)]
        public DateTime? StoryDueDate { get; set; }

        //The date the story was actually finished
        [DataType(DataType.Date)]
        public DateTime? StoryCloseDate { get; set; }

        //The number of points assigned to this story
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Points For This Story")]
        [RegularExpression(@"^[0-9]+$",ErrorMessage = "Story points must be a positive whole number.")]
        public int StoryPoints { get; set; }

        //The description of the story
        [Display(Name = "Description")]
        public String StoryDescription { get; set; }

        //The ID of the stage (swim lane) that the story is in
        public ScrumAbleWorkflowStage WorkflowStage { get; set; }

        //The sprint this story is in
        [Display(Name = "Add Task To Sprint")]
        public ScrumAbleSprint Sprint { get; set; }

        //The ID of the sprint this story is in
        [Display(Name = "Add Task To Sprint")]
        [NotMapped]
        public int StorySprintId { get; set; }

        //The ID of the sprint this story is in
        [Display(Name = "Story Owner")]
        [NotMapped]
        public string StoryOwnerEmail { get; set; }


        public ICollection<ScrumAbleTask> Tasks { get; set; }





    }

}
