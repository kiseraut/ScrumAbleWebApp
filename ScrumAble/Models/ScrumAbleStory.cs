using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleStory
    {
        public int Id { get; set; }

        //The display name of the story
        [Required(AllowEmptyStrings = false), Display(Name = "Story Name")]
        public string StoryName { get; set; }

        //The owner of the story
        [Display(Name = "Story Owner")]
        public string StoryOwner { get; set; }

        //The date that the story was started i.e. moved into progress
        [Required(AllowEmptyStrings = false), Display(Name = "Story Start Date"), DataType(DataType.Date)]
        public DateTime StoryStartDate { get; set; }

        //The date the story is due to be completed
        [Required(AllowEmptyStrings = false), Display(Name = "Story Due Date"), DataType(DataType.Date)]
        public DateTime StoryDueDate { get; set; }

        //The date the story was actually finished
        [DataType(DataType.Date)]
        public DateTime StoryCloseDate { get; set; }

        //The number of points assgned to this story
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Points For This Story")]
        public int StoryPoints { get; set; }

        //The description of the story
        [Display(Name = "Description")]
        public String StoryDescription { get; set; }

        //The ID of the stage (swim lane) that the story is in
        public int? WorkflowStageID { get; set; }

        //The ID of the sprint this story is in
        [Display(Name = "Add Task To Sprint")]
        public int? SprintId { get; set; }


    }

}
