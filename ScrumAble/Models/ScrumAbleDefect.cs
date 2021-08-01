using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public class ScrumAbleDefect : IScrumAbleDefect
    {
        public int Id { get; set; }

        //The display name of the Defect
        [Required(AllowEmptyStrings = false), Display(Name = "Defect Name")]
        public string DefectName { get; set; }

        //The owner of the Defect
        [Display(Name = "Defect Owner")]
        public ScrumAbleUser DefectOwner { get; set; }

        //The date that the Defect was started i.e. moved into progress
        [Display(Name = "Defect Start Date"), DataType(DataType.Date)]
        public DateTime? DefectStartDate { get; set; }

        //The date the Defect is due to be completed
        [Display(Name = "Defect Due Date"), DataType(DataType.Date)]
        public DateTime? DefectDueDate { get; set; }

        //The date the Defect was actually finished
        [DataType(DataType.Date)]
        public DateTime? DefectCloseDate { get; set; }

        //The number of points assigned to this Defect
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Points For This Defect")]
        public int DefectPoints { get; set; }

        //The description of the Defect
        [Display(Name = "Description")]
        public String DefectDescription { get; set; }

        //The ID of the stage (swim lane) that the Defect is in
        public ScrumAbleWorkflowStage WorkflowStage { get; set; }

        //The sprint this Defect is in
        [Display(Name = "Add Defect To Sprint")]
        public ScrumAbleSprint Sprint { get; set; }

        //The ID of the sprint this Defect is in
        [Display(Name = "Add Defect To Sprint")]
        [NotMapped]
        public int DefectSprintId { get; set; }

        //The Release this Defect is in
        [Display(Name = "Add Defect To Sprint")]
        public ScrumAbleRelease Release { get; set; }

        //The ID of the Release this Defect is in
        [Display(Name = "Add Defect To Sprint")]
        [NotMapped]
        public int DefectReleaseId { get; set; }

        //The ID of the sprint this Defect is in
        [Display(Name = "Defect Owner")]
        [NotMapped]
        public string DefectOwnerEmail { get; set; }
    }
}
