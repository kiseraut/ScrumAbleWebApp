using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;

namespace ScrumAble.Models
{
    public class ScrumAbleRepo : IScrumAbleRepo
    {
        private readonly ScrumAbleContext _context;

        public ScrumAbleRepo(ScrumAbleContext context)
        {
            _context = context;
        }


        public ScrumAbleTask GetTaskById(int id)
        {
            return _context.Tasks.Where(t => t.Id == id)
                .SingleOrDefault();
        }

        public ScrumAbleTask PopulateTaskMetadata(ScrumAbleTask task)
        {
            return _context.Tasks.Where(t => t.Id == task.Id)
                .Include(t => t.Story)
                .Include(t => t.WorkflowStage)
                .Include(t => t.Sprint)
                .Include(t => t.TaskOwner)
                .SingleOrDefault();
        }

        public void SaveToDb(ScrumAbleTask task)
        {
            if (task.Id == 0)
            {
                _context.Tasks.Add(task);
            }
            else
            {
                _context.Tasks.Update(task);
            }
            
            _context.SaveChanges();
        }

        public void DeleteFromDb(ScrumAbleTask task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public ScrumAbleUser GetUserById(string id)
        {
            var user = _context.User.Where(u => u.Id == id)
                .Include(u => u.CurrentWorkingTeam)
                .Include(u => u.CurrentWorkingRelease)
                .Include(u => u.CurrentWorkingSprint)
                .Include(u => u.Stories)
                .Include(u => u.UserTeamMappings)
                .ThenInclude(utm => utm.Team)
                .FirstOrDefault();

            var teammates = _context.UserTeamMapping.Where(utm => utm.Team.Id == user.CurrentWorkingTeam.Id)
                .Include(utm => utm.User)
                .ToList();

            //add the teammates to the user object
            ICollection<ScrumAbleUser> userTeammates = new List<ScrumAbleUser>();

            foreach (var teammate in teammates)
            {
                userTeammates.Add(teammate.User);
            }

            user.Teammates = userTeammates;

            return user;
        }

        public ScrumAbleUser GetUserByUsername(string username)
        {
            var user = _context.Users.Where(u => u.Email == username)
                .Include(u => u.CurrentWorkingTeam)
                .Include(u => u.CurrentWorkingRelease)
                .Include(u => u.CurrentWorkingSprint)
                .Include(u => u.Stories)
                .Include(u => u.UserTeamMappings)
                .ThenInclude(utm => utm.Team)
                .FirstOrDefault();

            var teammates = _context.UserTeamMapping.Where(utm => utm.Team.Id == user.CurrentWorkingTeam.Id)
                .Include(utm => utm.User)
                .ToList();

            //add the teammates to the user object
            ICollection<ScrumAbleUser> userTeammates = new List<ScrumAbleUser>();

            foreach (var teammate in teammates)
            {
                userTeammates.Add(teammate.User);
            }

            user.Teammates = userTeammates;

            return user;
        }

        public ScrumAbleUser PopulateUserMetadata(ScrumAbleUser user)
        {
            throw new NotImplementedException();
        }

        public void SaveToDb(ScrumAbleUser user)
        {
            if (user.Id == "0")
            {
                //new users are handled by Identity framework
            }
            else
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            
        }

        public void DeleteFromDb(ScrumAbleUser user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public ScrumAbleSprint GetSprintById(int id)
        {
            return _context.Sprints.Where(s => s.Id == id)
                .SingleOrDefault();
        }

        public ScrumAbleStory GetStoryById(int id)
        {
            return _context.Stories.Where(s => s.Id == id)
                .SingleOrDefault(); ;
        }

        public ViewModelTaskAggregate GetTaskAggregateData(string userId)
        {
            var user = GetUserById(userId);

            //get all of the current and future sprints for the current working team and release
            var sprints = _context.Sprints.Where(s => s.Release.Id == user.CurrentWorkingRelease.Id)
                .Where(s => s.SprintEndDate >= DateTime.Today)
                .ToList();

            //get all of the stories in the current and future sprints
            var stories = _context.Stories
                .FromSqlRaw("SELECT st.Id ,st.StoryName ,st.StoryStartDate ,st.StoryDueDate ,st.StoryCloseDate ,st.StoryPoints ,st.StoryDescription ,st.WorkflowStageId ,st.SprintId ,st.StoryOwnerId FROM Stories st INNER JOIN Sprints sp ON st.SprintId = sp.Id WHERE st.StoryStartDate < GETDATE() AND (st.StoryCloseDate > GETDATE() OR st.StoryCloseDate is NULL) AND sp.ReleaseId = {0}", user.CurrentWorkingRelease.Id)
                .ToList();

            //add all metadata to the viewmodelTaskAgregate object
            var viewmodelTaskAggregate = new ViewModelTaskAggregate();
            viewmodelTaskAggregate.Add(user.Teammates);
            viewmodelTaskAggregate.Add(sprints);
            viewmodelTaskAggregate.Add(stories);

            return viewmodelTaskAggregate;

        }

    }
}
