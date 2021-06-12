using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleRelease
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
        public string TeamId { get; set; }
    }
}
