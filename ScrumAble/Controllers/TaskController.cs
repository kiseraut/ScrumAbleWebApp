using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;
using ScrumAble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScrumAble.Controllers
{
    public class TaskController : Controller
    {

        private readonly UserManager<ScrumAbleUser> _userManager;
        private readonly ScrumAbleContext _context;

        public TaskController(ScrumAbleContext context, UserManager<ScrumAbleUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) // will give the user's userId
           // var userName = User.FindFirstValue(ClaimTypes.Name) // will give the user's userName

            // For ASP.NET Core >= 5.0
          //  var userEmail = User.FindFirstValue(ClaimTypes.Email) // will give the user's Email
    

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var tasks = _context.Tasks.Where(t => t.Id == id)
                .Include(t => t.Story)
                .Include(t => t.WorkflowStage)
                .Include(t => t.Sprint)
                .Include(t => t.TaskOwner)
               .SingleOrDefault();

            return View(tasks);
        }

        public IActionResult AddTask(ScrumAbleTask scrumAbleTask)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(u => u.Id == userId)
                .Include(u => u.UserTeamMappings)
                    .ThenInclude(utm => utm.Team)
                .SingleOrDefault();

            //set the current working team for this user if it is not set already
            if (user.currentWorkingTeam == null && user.UserTeamMappings.ElementAt(0).Team != null)
            {
                user.currentWorkingTeam = user.UserTeamMappings.ElementAt(0).Team;
                
            }

            //set the current working release for this user if it is not set already
            var releases = _context.Releases.Where(r => r.Team == user.currentWorkingTeam)
                    .ToList();

            if (user.currentWorkingRelease == null && releases != null)
            {
                user.currentWorkingRelease = releases.ElementAt(0);
                
            }

            _context.Users.Update(user);
            _context.SaveChanges();


            scrumAbleTask.viewModelTaskAggregate = getTaskAggregateData(user);
            return View(scrumAbleTask);
        }

        [HttpPost]
        public IActionResult AddToSprint(ScrumAbleTask scrumAbleTask)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.Where(u => u.Id == userId)
                    .Include(u => u.UserTeamMappings)
                        .ThenInclude(utm => utm.Team)
                    .SingleOrDefault();
                scrumAbleTask.viewModelTaskAggregate = getTaskAggregateData(user);
                return View("AddTask", scrumAbleTask);
            }

            populateTaskMetaData(scrumAbleTask);

            _context.Tasks.Add(scrumAbleTask);
            _context.SaveChanges();

            //TODO redirect back to dashboard
            return RedirectToAction("Index", "Home");
        }

        private void populateTaskMetaData(ScrumAbleTask scrumAbleTask)
        {
            if (scrumAbleTask.TaskOwnerId != null && scrumAbleTask.TaskOwnerId != "-1")
            {
                var taskOwner = _context.Users.Where(u => u.Email == scrumAbleTask.TaskOwnerId)
                     .SingleOrDefault();
                scrumAbleTask.TaskOwner = taskOwner;
            }

            if (scrumAbleTask.TaskSprintId != null)
            {
                var taskSprint = _context.Sprints.Where(s => s.Id == scrumAbleTask.TaskSprintId)
                .SingleOrDefault();
                scrumAbleTask.Sprint = taskSprint;
            }
            else if(scrumAbleTask.TaskSprintId == -1)
            {
                //TODO assign to team/release backlog
            }

            if (scrumAbleTask.TaskStoryId != null && scrumAbleTask.TaskStoryId != -1)
            {
                var taskStory = _context.Stories.Where(s => s.Id == scrumAbleTask.TaskStoryId)
                .SingleOrDefault();
                scrumAbleTask.Story = taskStory;
            }
        }

        private ViewModelTaskAggregate getTaskAggregateData(ScrumAbleUser user)
        {
            //***need to figure out why the mappings are not being brought in from the db
            if (user.currentWorkingTeam == null && user.UserTeamMappings.ElementAt(0).Team != null)
            {
                user.currentWorkingTeam = user.UserTeamMappings.ElementAt(0).Team;

            }

            //set the current working release for this user if it is not set already
            var releases = _context.Releases.Where(r => r.Team == user.currentWorkingTeam)
                    .ToList();

            if (user.currentWorkingRelease == null && releases != null)
            {
                user.currentWorkingRelease = releases.ElementAt(0);

            }
            //***end need to figure out


            //get all of the other users (teammates) that in the current working team
            var teammates = _context.UserTeamMapping.Where(utm => utm.Team.Id == user.currentWorkingTeam.Id)
                    .Include(utm => utm.User)
                    .ToList();

            //add the teammates to the user object
            ICollection<ScrumAbleUser> userTeammates = new List<ScrumAbleUser>();

            foreach (var teammate in teammates)
            {
                userTeammates.Add(teammate.User);
            }

            user.Teammates = userTeammates;

            //get all of the current and future sprints for the current working team and release
            var sprints = _context.Sprints.Where(s => s.Release.Id == user.currentWorkingRelease.Id)
                .Where(s => s.SprintStartDate >= DateTime.Today)
                .ToList();

            //get all of the stories in the current and future sprints
            var stories = _context.Stories
                .FromSqlRaw("SELECT st.Id ,st.StoryName ,st.StoryStartDate ,st.StoryDueDate ,st.StoryCloseDate ,st.StoryPoints ,st.StoryDescription ,st.WorkflowStageId ,st.SprintId ,st.StoryOwnerId FROM Stories st INNER JOIN Sprints sp ON st.SprintId = sp.Id WHERE st.StoryStartDate < GETDATE() AND (st.StoryCloseDate > GETDATE() OR st.StoryCloseDate is NULL) AND sp.ReleaseId = {0}", user.currentWorkingRelease.Id)
                .ToList();

            //add all metadata to the viewmodelTaskAgregate object
            ViewModelTaskAggregate viewmodelTaskAggregate = new ViewModelTaskAggregate();
            viewmodelTaskAggregate.Add(user.Teammates);
            viewmodelTaskAggregate.Add(sprints);
            viewmodelTaskAggregate.Add(stories);

            return viewmodelTaskAggregate;

        }
    }
}
