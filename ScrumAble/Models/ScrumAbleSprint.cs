﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ScrumAbleSprint
    {
        public int Id { get; set; }

        //The display name of the sprint
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Name")]
        public string SprintName { get; set; }

        //The date that the sprint is started
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint Start Date"), DataType(DataType.Date)]
        public DateTime SprintStartDate { get; set; }

        //The date the sprint is over
        [Required(AllowEmptyStrings = false), Display(Name = "Sprint End Date"), DataType(DataType.Date)]
        public DateTime SprintEndDate { get; set; }

        //The Releaes that this sprint it in
        [Required(AllowEmptyStrings = false), Display(Name = "Release")]
        public string ReleaseId { get; set; }
    }
}
