using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ScrumAble.Data;
using ScrumAble.Models;

namespace ScrumAble.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ScrumAbleUser class
    public class ScrumAbleUser : IdentityUser, IScrumAbleUser
    {

        [Display(Name = "Display Color")]
        public string UserColor { get; set; }

        public ICollection<ScrumAbleUserTeamMapping> UserTeamMappings { get; set; }
        public ICollection<ScrumAbleStory> Stories { get; set; }
        public ICollection<ScrumAbleTask> Tasks { get; set; }

        public ScrumAbleTeam CurrentWorkingTeam { get; set; }

        public ScrumAbleRelease CurrentWorkingRelease { get; set; }

        public ScrumAbleSprint CurrentWorkingSprint { get; set; }

        [NotMapped]
        public ICollection<ScrumAbleUser> Teammates { get; set; }
        

    }
}
