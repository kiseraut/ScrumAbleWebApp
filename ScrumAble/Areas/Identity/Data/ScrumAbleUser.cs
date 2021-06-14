using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Data;
using ScrumAble.Models;

namespace ScrumAble.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ScrumAbleUser class
    public class ScrumAbleUser : IdentityUser
    {


        [Display(Name = "Display Color")]
        public string UserColor { get; set; }

        public ICollection<ScrumAbleUserTeamMapping> UserTeamMappings { get; set; }
        public ICollection<ScrumAbleStory> Stories { get; set; }
        public ICollection<ScrumAbleTask> Tasks { get; set; }

        public ScrumAbleTeam currentWorkingTeam { get; set; }

        public ScrumAbleRelease currentWorkingRelease { get; set; }

        public ScrumAbleSprint currentWorkingSprint { get; set; }

        [NotMapped]
        public ICollection<ScrumAbleUser> Teammates { get; set; }

        


    }
}
