using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        public ScrumAbleStory GetStoryById(int id)
        {
            return _context.Stories.Where(s => s.Id == id)
                .SingleOrDefault(); ;
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

    }
}
