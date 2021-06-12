using ScrumAble.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleUserTeamMapping
    {
        public int Id { get; set; }

        public ScrumAbleUser User { get; set; }

        [Required]
        public int UserId { get; set; }

        public ScrumAbleTeam Team { get; set; }

        [Required]
        public int TeamId { get; set; }
    }
}
