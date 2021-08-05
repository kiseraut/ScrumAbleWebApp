using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;
using ScrumAble.Models;
using ScrumAble.Tests.Models;

namespace ScrumAble.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleRepo : IScrumAbleRepo
    {
        private readonly ScrumAbleContext _context;

        public MockScrumAbleRepo(ScrumAbleContext context)
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
            return task;
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
            throw new NotImplementedException();
        }

        public void MoveTask(int taskId, int workflowStageId, ScrumAbleUser user)
        {
            //move task
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

            /*if (user.CurrentWorkingTeam != null)
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

            user.TeamsJoined = getAllUserTeams(user.Id);*/

            return user;
        }

        public ScrumAbleUser GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Email == username)
                .SingleOrDefault();
        }

        public void SaveToDb(ScrumAbleUser user)
        {
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
           //do nothing in mock

        }

        public void SetCurrentSprint(string userId, int sprintId)
        {
            var user = GetUserById(userId);
            user.CurrentWorkingSprint = GetSprintById(sprintId);
            SaveToDb(user);
        }

        public void SetCurrentTeam(string userId, int teamId)
        {
            //do nothing in mock
        }

        public ScrumAbleSprint GetSprintById(int id)
        {
            return _context.Sprints.Where(s => s.Id == id)
                .SingleOrDefault();
        }

        public ScrumAbleSprint GetSprintForDashboard(int id)
        {
            var sprint = _context.Sprints.Where(s => s.Id == id)
                .Include(s => s.Stories).ThenInclude(s => s.WorkflowStage)
                .Include(s => s.Tasks).ThenInclude(t => t.WorkflowStage)
                .Include(s => s.Defects).ThenInclude(d => d.WorkflowStage)
                .Include(s => s.Release)
                .SingleOrDefault();

            sprint.WorkflowStages = _context.WorkflowStages.Where(w => w.Team.Id == sprint.Release.Team.Id)
                .Include(w => w.Stories).ThenInclude(s => s.StoryOwner)
                .Include(w => w.Tasks).ThenInclude(t => t.TaskOwner)
                .Include(w => w.Defects).ThenInclude(d => d.DefectOwner)
                .ToList();


            return sprint;
        }

        public bool IsAuthorized(ScrumAbleSprint sprint, string userId)
        {
            return true;
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

        public List<ScrumAbleSprint> GetAllSprintsInRelease(int releaseId)
        {
            return _context.Sprints.Where(s => s.Release.Id == releaseId)
                .Where(s => s.SprintEndDate >= DateTime.Today)
                .ToList();
        }

        public ScrumAbleStory GetStoryById(int id)
        {
            return _context.Stories.Where(s => s.Id == id)
                .SingleOrDefault(); ;
        }

        public bool IsAuthorized(ScrumAbleStory story, string userId)
        {
            return true;
        }

        public void SaveToDb(ScrumAbleStory story)
        {
            if (story.Id == 0)
            {
                _context.Stories.Add(story);
                _context.SaveChanges();
            }
            else
            {
                var dbStory = _context.Stories.First(s => s.Id == story.Id);
                _context.Entry(dbStory).CurrentValues.SetValues(story);
                _context.SaveChanges();
            }
        }

        public void DeleteFromDb(ScrumAbleStory story)
        {
            _context.Stories.Remove(story);
            _context.SaveChanges();
        }

        public void MoveStory(int storyId, int workflowStageId, ScrumAbleUser user)
        {
            //move story
        }

        public ViewModelTaskAggregate GetTaskAggregateData(string userId)
        {
            var user = new ScrumAbleUser();

            //get all of the current and future sprints for the current working team and release
            var sprints = new ScrumAbleSprint();

            //get all of the stories in the current and 

            //add all metadata to the viewmodelTaskAgregate object
            var viewmodelTaskAggregate = new ViewModelTaskAggregate();
            viewmodelTaskAggregate.Add(user.Teammates);

            return viewmodelTaskAggregate;

        }

        public ScrumAbleTeam GetTeamById(int id)
        {
            var team = _context.Teams.Where(t => t.Id == id)
                .Include(t => t.UserTeamMappings)
                .ThenInclude(utm => utm.User)
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
            return true;
        }

        public List<ScrumAbleTeam> GetAllUserTeams(string userId)
        {
            return null;
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
            throw new NotImplementedException();
        }

        public List<ScrumAbleRelease> GetAllTeamReleases(int teamId)
        {
            return _context.Releases.ToList();
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
                        //_context.Database.ExecuteSqlRaw("INSERT INTO UserTeamMapping (UserId, TeamId) VALUES ({0}, {1})", user.Id, team.Id);
                        //_context.SaveChanges();
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
                        //_context.Database.ExecuteSqlRaw("INSERT INTO UserTeamMapping (UserId, TeamId) VALUES ({0}, {1})", user.Id, team.Id);
                        //_context.SaveChanges();
                    }
                }
            }
        }

        public void DeleteFromDb(ScrumAbleTeam team)
        {
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        public ScrumAbleWorkflowStage GetWorkflowStageById(int id)
        {
            var workflowStage = _context.WorkflowStages.Where(w => w.Id == id)
                .Include(w => w.Team)
                .Include(w => w.Stories)
                .Include(w => w.Tasks)
                .SingleOrDefault();

            if (workflowStage != null)
            {
                workflowStage.AssociatedWorkflowStages = GetTeamWorkflowStages(workflowStage.Team);
            }

            return workflowStage;
        }

        public bool IsAuthorized(ScrumAbleWorkflowStage workflowStage, string userId)
        {
            return true;
        }

        public bool SaveToDb(ScrumAbleWorkflowStage workflowStage, ScrumAbleUser user)
        {
            if (workflowStage.Id == 0)
            {
                _context.Add(workflowStage);
                _context.SaveChanges();
            }
            else
            {
                var dbWorkflowStage = _context.WorkflowStages.First(w => w.Id == workflowStage.Id);
                _context.Entry(dbWorkflowStage).CurrentValues.SetValues(workflowStage);
                _context.SaveChanges();

            }

            return true;
        }

        public void DeleteFromDb(ScrumAbleWorkflowStage workflowStage)
        {
            _context.WorkflowStages.Remove(workflowStage);
            _context.SaveChanges();
        }

        public List<ScrumAbleWorkflowStage> GetTeamWorkflowStages(ScrumAbleTeam team)
        {
            var mockWorkflowStage1 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var mockWorkflowStage2 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();
            var mockWorkflowStage3 = MockScrumAbleWorkflowStage.GenerateWorkflowStage();

            mockWorkflowStage1.WorkflowStagePosition = 0;
            mockWorkflowStage2.WorkflowStagePosition = 1;
            mockWorkflowStage3.WorkflowStagePosition = 2;

            return new List<ScrumAbleWorkflowStage>() {mockWorkflowStage1, mockWorkflowStage2, mockWorkflowStage3};
        }

        public bool IsFinalWorkflowStage(ScrumAbleWorkflowStage workflowStage)
        {
            throw new NotImplementedException();
        }

        public ScrumAbleDefect GetDefectById(int id)
        {
            return _context.Defects.Where(d => d.Id == id)
                .FirstOrDefault();
        }

        public bool IsAuthorized(ScrumAbleDefect defect, string userId)
        {
            return IsAuthorized(defect.Sprint, userId);
        }

        public void SaveToDb(ScrumAbleDefect defect)
        {
            if (defect.Id == 0)
            {
                _context.Defects.Add(defect);
                _context.SaveChanges();
            }
            else
            {
                var dbDefect = _context.Defects.First(d => d.Id == defect.Id);
                _context.Entry(dbDefect).CurrentValues.SetValues(defect);
                dbDefect.Sprint = defect.Sprint;
                _context.SaveChanges();
            }
        }

        public void DeleteFromDb(ScrumAbleDefect defect)
        {
            _context.Defects.Remove(defect);
            _context.SaveChanges();
        }

        public void MoveDefect(int defectId, int workflowStageId, ScrumAbleUser user)
        {
            //move defect
        }
    }
}
