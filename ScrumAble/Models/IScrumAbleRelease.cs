using System;
using System.Collections.Generic;

namespace ScrumAble.Models
{
    public interface IScrumAbleRelease
    {
        int Id { get; set; }
        string ReleaseName { get; set; }
        DateTime ReleaseStartDate { get; set; }
        DateTime ReleaseEndDate { get; set; }
        ScrumAbleTeam Team { get; set; }
        ICollection<ScrumAbleSprint> Sprints { get; set; }
        ICollection<ScrumAbleDefect> Defects { get; set; }
    }
}