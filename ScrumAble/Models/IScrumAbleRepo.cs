using System.Collections.Generic;
using System.Threading.Tasks;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IScrumAbleRepo
    {
        //Task methods
        public ScrumAbleTask GetTaskById(int id);
        public ScrumAbleTask PopulateTaskMetadata(ScrumAbleTask task);
        public void SaveToDb(ScrumAbleTask task);
        public void DeleteFromDb(ScrumAbleTask task);
        public bool IsAuthorized(ScrumAbleTask task, string userId);


        //User methods
        public ScrumAbleUser GetUserById(string id);
        public ScrumAbleUser GetUserByUsername(string username);
        public void SaveToDb(ScrumAbleUser user);
        public void DeleteFromDb(ScrumAbleUser user);

        //Sprint methods
        public ScrumAbleSprint GetSprintById(int id);
        public bool IsAuthorized(ScrumAbleSprint sprint, string userId);

        //Story methods
        public ScrumAbleStory GetStoryById(int id);
        public bool IsAuthorized(ScrumAbleStory story, string userId);

        //ViewModelTaskAggregate methods
        public ViewModelTaskAggregate GetTaskAggregateData(string userId);

        //Team methods
        public ScrumAbleTeam GetTeamById(int id);
        public bool IsAuthorized(ScrumAbleTeam team, string userId);

        //Release methods
        public bool IsAuthorized(ScrumAbleRelease release, string userId);

        //UserTeamMapping methods
        public void SaveToDb(ScrumAbleTeam team, List<IScrumAbleUser> users);
        public void DeleteFromDb(ScrumAbleTeam team);


    }
}