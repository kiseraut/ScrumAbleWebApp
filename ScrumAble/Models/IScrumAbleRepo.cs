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


        //User methods
        public ScrumAbleUser GetUserById(string id);
        public ScrumAbleUser GetUserByUsername(string username);
        public ScrumAbleUser PopulateUserMetadata(ScrumAbleUser user);
        public void SaveToDb(ScrumAbleUser user);
        public void DeleteFromDb(ScrumAbleUser user);

        //Sprint methods
        public ScrumAbleSprint GetSprintById(int id);

        //Story methods
        public ScrumAbleStory GetStoryById(int id);

        //ViewModelTaskAggregate methods
        public ViewModelTaskAggregate GetTaskAggregateData(string userId);


    }
}