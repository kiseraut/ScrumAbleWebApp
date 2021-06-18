using System.Collections.Generic;
using ScrumAble.Models;

namespace ScrumAble.Areas.Identity.Data
{
    public interface IScrumAbleUser
    {
        string UserColor { get; set; }
        ICollection<ScrumAbleUserTeamMapping> UserTeamMappings { get; set; }
        ICollection<ScrumAbleStory> Stories { get; set; }
        ICollection<ScrumAbleTask> Tasks { get; set; }
        ScrumAbleTeam CurrentWorkingTeam { get; set; }
        ScrumAbleRelease CurrentWorkingRelease { get; set; }
        ScrumAbleSprint CurrentWorkingSprint { get; set; }
        ICollection<ScrumAbleUser> Teammates { get; set; }
        string Id { get; set; }
        string UserName { get; set; }
    }
}