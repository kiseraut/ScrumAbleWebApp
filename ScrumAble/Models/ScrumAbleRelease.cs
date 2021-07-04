using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public class ScrumAbleRelease : IScrumAbleRelease
    {
        public int Id { get; set; }

        //The display name of the release
        [Required(AllowEmptyStrings = false), Display(Name = "Release Name")]
        public string ReleaseName { get; set; }

        //The date that the release was started
        [Required(AllowEmptyStrings = false), Display(Name = "Release Start Date"), DataType(DataType.Date)]
        public DateTime ReleaseStartDate { get; set; }

        //The date the release is completed
        [DataType(DataType.Date)]
        public DateTime ReleaseEndDate { get; set; }

        //The Team that will be working on this release
        [Required(AllowEmptyStrings = false), Display(Name = "Team")]
        public ScrumAbleTeam Team { get; set; }

        public ICollection<ScrumAbleSprint> Sprints { get; set; }

        //The ID of the team this task is in
        [Display(Name = "Add Release To Team")]
        [NotMapped]
        public int ReleaseTeamId { get; set; }

        [NotMapped]
        public ScrumAbleUser CurrentUser { get; set; }

    }
}
