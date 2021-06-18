using ScrumAble.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleUserTeamMapping : IScrumAbleUserTeamMapping
    {
        public int Id { get; set; }

        [Required]
        public ScrumAbleUser User { get; set; }

        [Required]
        public ScrumAbleTeam Team { get; set; }
    }
}
