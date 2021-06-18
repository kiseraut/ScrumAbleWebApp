using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IScrumAbleUserTeamMapping
    {
        int Id { get; set; }
        ScrumAbleUser User { get; set; }
        ScrumAbleTeam Team { get; set; }
    }
}