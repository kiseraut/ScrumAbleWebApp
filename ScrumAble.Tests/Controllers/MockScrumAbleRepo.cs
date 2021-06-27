using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;
using ScrumAble.Models;

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

        public ScrumAbleUser GetUserById(string id)
        {
            return _context.Users.Where(u => u.Id == id)
                .SingleOrDefault();
        }

        public ScrumAbleUser GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Email == username)
                .SingleOrDefault();
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

        public bool IsAuthorized(ScrumAbleSprint sprint, string userId)
        {
            throw new NotImplementedException();
        }

        public ScrumAbleStory GetStoryById(int id)
        {
            return _context.Stories.Where(s => s.Id == id)
                .SingleOrDefault(); ;
        }

        public bool IsAuthorized(ScrumAbleStory story, string userId)
        {
            throw new NotImplementedException();
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
            foreach (var mapping in team.UserTeamMappings)
            {
                if (mapping.User.Id == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAuthorized(ScrumAbleRelease release, string userId)
        {
            throw new NotImplementedException();
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
                    var DBCheck = _context.UserTeamMapping.Where(utm => utm.Team == team && utm.User == user)
                        .SingleOrDefault();

                    if (DBCheck == null)
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
                    var DBCheck = _context.UserTeamMapping.Where(utm => utm.Team == team && utm.User == user)
                        .SingleOrDefault();

                    if (DBCheck == null)
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
