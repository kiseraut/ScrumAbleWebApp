using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
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

        public bool IsAuthorized(ScrumAbleTask task, string userId)
        {
            return IsAuthorized(task.Story, userId);
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
                .ThenInclude(t => t.Releases)
                .ThenInclude(r => r.Sprints)
                .FirstOrDefault();

            if (user.CurrentWorkingTeam != null)
            {
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
            }

            user.TeamsJoined = GetAllUserTeams(user.Id);

            return user;
        }

        public ScrumAbleUser GetUserByUsername(string username)
        {
            var user = _context.Users.Where(u => u.UserName == username)
                .Include(u => u.CurrentWorkingTeam)
                .Include(u => u.CurrentWorkingRelease)
                .Include(u => u.CurrentWorkingSprint)
                .Include(u => u.Stories)
                .Include(u => u.UserTeamMappings)
                .ThenInclude(utm => utm.Team)
                .FirstOrDefault();

            if (user != null && user.CurrentWorkingTeam != null)
            {
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
            }

            return user;
        }

        public void SaveToDb(ScrumAbleUser user)
        {
            //new users (identified by a 0 ID) are handled by Identity framework
            if (user.Id != "0")
            {
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public void DeleteFromDb(ScrumAbleUser user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void SetCurrentRelease(string userId, int releaseId)
        {
            var user = GetUserById(userId);
            var release = GetReleaseById(releaseId);

            var sprint = _context.Sprints
                .FirstOrDefault(s => s.Release.Id == release.Id);
            
            user.CurrentWorkingRelease = release;
            user.CurrentWorkingSprint = sprint;
            
            SaveToDb(user);

        }

        public void SetCurrentSprint(string userId, int sprintId)
        {
            var user = GetUserById(userId);
            user.CurrentWorkingSprint = GetSprintById(sprintId);
            SaveToDb(user);
        }

        public void SetCurrentTeam(string userId, int teamId)
        {
            var user = GetUserById(userId);
            var team = GetTeamById(teamId);

            var release = _context.Releases
                .FirstOrDefault(r => r.Team.Id == teamId);

            if (release != null)
            {
                var sprint = _context.Sprints
                    .FirstOrDefault(s => s.Release.Id == release.Id);
                user.CurrentWorkingSprint = sprint;
            }
            else
            {
                user.CurrentWorkingSprint = null;
            }

            user.CurrentWorkingTeam = team;
            user.CurrentWorkingRelease = release;
            
            SaveToDb(user);
        }

        public ScrumAbleSprint GetSprintById(int id)
        {
            var sprint = _context.Sprints.Where(s => s.Id == id)
                .Include(s => s.Stories)
                .Include(s => s.Tasks)
                .Include(s => s.Release)
                .SingleOrDefault();

            return sprint;
        }

        public bool IsAuthorized(ScrumAbleSprint sprint, string userId)
        {
            return IsAuthorized(sprint.Release, userId);
        }

        public void SaveToDb(ScrumAbleSprint sprint)
        {
            if (sprint.Id == 0)
            {
                _context.Sprints.Add(sprint);
                _context.SaveChanges();
            }
            else
            {
                var dbSprint = _context.Sprints.First(s => s.Id == sprint.Id);
                _context.Entry(dbSprint).CurrentValues.SetValues(sprint);
                _context.SaveChanges();
            }
        
        }

        public void DeleteFromDb(ScrumAbleSprint sprint)
        {
            _context.Sprints.Remove(sprint);
            _context.SaveChanges();
        }

        public ScrumAbleStory GetStoryById(int id)
        {
            return _context.Stories.Where(s => s.Id == id)
                .SingleOrDefault(); ;
        }

        public bool IsAuthorized(ScrumAbleStory story, string userId)
        {
            return IsAuthorized(story.Sprint, userId);
        }

        public ViewModelTaskAggregate GetTaskAggregateData(string userId)
        {
            var user = GetUserById(userId);
            var viewmodelTaskAggregate = new ViewModelTaskAggregate();


            if (user.CurrentWorkingRelease != null)
            {
                //get all of the current and future sprints for the current working team and release
                var sprints = _context.Sprints.Where(s => s.Release.Id == user.CurrentWorkingRelease.Id)
                    .Where(s => s.SprintEndDate >= DateTime.Today)
                    .ToList();
                
                //get all of the stories in the current and future sprints
                var stories = _context.Stories
                    .FromSqlRaw(
                        "SELECT st.Id ,st.StoryName ,st.StoryStartDate ,st.StoryDueDate ,st.StoryCloseDate ,st.StoryPoints ,st.StoryDescription ,st.WorkflowStageId ,st.SprintId ,st.StoryOwnerId FROM Stories st INNER JOIN Sprints sp ON st.SprintId = sp.Id WHERE st.StoryStartDate < GETDATE() AND (st.StoryCloseDate > GETDATE() OR st.StoryCloseDate is NULL) AND sp.ReleaseId = {0}",
                        user.CurrentWorkingRelease.Id)
                    .ToList();

                viewmodelTaskAggregate.Add(sprints);
                viewmodelTaskAggregate.Add(stories);
            }
            
            viewmodelTaskAggregate.Add(user.Teammates);

            return viewmodelTaskAggregate;

        }

        public ScrumAbleTeam GetTeamById(int id)
        {
            var team = _context.Teams.Where(t => t.Id == id)
                .Include(t => t.UserTeamMappings)
                .ThenInclude(utm => utm.User)
                .Include(t => t.Releases)
                .SingleOrDefault();

            if (team != null)
            {
                foreach (var mapping in team.UserTeamMappings)
                {
                    team.TeammatesText += mapping.User.UserName + "\n";
                }
            }

            return team;
        }

        public bool IsAuthorized(ScrumAbleTeam team, string userId)
        {
            foreach (var mapping in team.UserTeamMappings)
            {
                if (mapping.User.Id == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ScrumAbleTeam> GetAllUserTeams(string userId)
        {
            var userTeamMappings = _context.UserTeamMapping.Where(utm => utm.User.Id == userId)
                .Include(utm => utm.Team)
                .ToList();

            var teams = new List<ScrumAbleTeam>();

            foreach (var mapping in userTeamMappings)
            {
                teams.Add(mapping.Team);
            }

            return teams;
        }

        public void DeleteFromDb(ScrumAbleRelease release)
        {
            _context.Releases.Remove(release);
            _context.SaveChanges();
        }

        public ScrumAbleRelease GetReleaseById(int id)
        {
            var release = _context.Releases.Where(r => r.Id == id)
                .Include(r => r.Sprints)
                .Include(r => r.Team)
                .SingleOrDefault();

            return release;
        }

        public bool IsAuthorized(ScrumAbleRelease release, string userId)
        {
            if (release.Team != null)
            {
                var team = GetTeamById(release.Team.Id);
                return IsAuthorized(team, userId);
            }

            return false;
        }

        public List<ScrumAbleRelease> GetAllTeamReleases(int teamId)
        {
            var releaseList = _context.Releases.Where(r => r.Team.Id == teamId)
                .ToList();

            return releaseList;
        }

        public void SaveToDb(ScrumAbleRelease release)
        {
            if (release.Id == 0)
            {
                _context.Releases.Add(release);
                _context.SaveChanges();
            }
            else
            {
                var dbRelease = _context.Releases.First(r => r.Id == release.Id);
                _context.Entry(dbRelease).CurrentValues.SetValues(release);
                _context.SaveChanges();
            }
        }

        public void SaveToDb(ScrumAbleTeam team, List<IScrumAbleUser> users)
        {
            if (team.Id == 0)
            {
                _context.Teams.Add(team);
                _context.SaveChanges();

                while (team.Id == 0)
                {
                    Thread.Sleep(1000);
                }

                foreach (var user in users)
                {
                    var dbCheck = _context.UserTeamMapping.Where(utm => utm.Team == team && utm.User == user)
                        .SingleOrDefault();

                    if (dbCheck == null)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO UserTeamMapping (UserId, TeamId) VALUES ({0}, {1})", user.Id, team.Id);
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                var dbTeam = _context.Teams.First(t => t.Id == team.Id);
                _context.Entry(dbTeam).CurrentValues.SetValues(team);
                _context.SaveChanges();

                while (team.Id == 0)
                {
                    Thread.Sleep(1000);
                }

                var teamMappings = _context.UserTeamMapping.Where(utm => utm.Team.Id == team.Id)
                    .ToList();

                foreach (var teamMapping in teamMappings)
                {
                    _context.Remove(teamMapping);
                    _context.SaveChanges();
                }

                foreach (var user in users)
                {
                    var dbCheck = _context.UserTeamMapping.Where(utm => utm.Team == team && utm.User == user)
                        .SingleOrDefault();

                    if (dbCheck == null)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO UserTeamMapping (UserId, TeamId) VALUES ({0}, {1})", user.Id, team.Id);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void DeleteFromDb(ScrumAbleTeam team)
        {
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }
    }
}
