using System;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IScrumAbleDefect
    {
        int Id { get; set; }
        string DefectName { get; set; }
        ScrumAbleUser DefectOwner { get; set; }
        DateTime? DefectStartDate { get; set; }
        DateTime? DefectDueDate { get; set; }
        DateTime? DefectCloseDate { get; set; }
        int DefectPoints { get; set; }
        String DefectDescription { get; set; }
        ScrumAbleWorkflowStage WorkflowStage { get; set; }
        ScrumAbleSprint Sprint { get; set; }
        int DefectSprintId { get; set; }
        ScrumAbleRelease Release { get; set; }
        int DefectReleaseId { get; set; }
        string DefectOwnerEmail { get; set; }
    }
}