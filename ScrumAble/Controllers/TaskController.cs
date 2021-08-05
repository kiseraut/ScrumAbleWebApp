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
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ScrumAble.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public TaskController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTask = _scrumAbleRepo.GetTaskById(id);

            if (scrumAbleTask == null)
            {
                return View("TaskNotFound");
            }

            scrumAbleTask = _scrumAbleRepo.PopulateTaskMetadata(scrumAbleTask);

            return View(scrumAbleTask);
        }

        public IActionResult AddTask(ScrumAbleTask scrumAbleTask)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            scrumAbleTask.ViewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ModelState.Clear();
            scrumAbleTask.TaskSprintId = user.CurrentWorkingSprint.Id;

            return View(scrumAbleTask);
        }
        
        public IActionResult EditTask(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTask = _scrumAbleRepo.GetTaskById(id);
            scrumAbleTask = _scrumAbleRepo.PopulateTaskMetadata(scrumAbleTask);
            
            if (scrumAbleTask == null || !_scrumAbleRepo.IsAuthorized(scrumAbleTask, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("TaskNotFound");
            }

            scrumAbleTask.ViewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleTask.TaskStoryId = scrumAbleTask.Story.Id;
            scrumAbleTask.TaskSprintId = scrumAbleTask.Sprint.Id;
            scrumAbleTask.TaskOwnerId = scrumAbleTask.TaskOwner.Email;


            return View(scrumAbleTask);
        }

        [HttpPost]
        public IActionResult CreateTask(ScrumAbleTask scrumAbleTask)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            if (!ModelState.IsValid)
            {
                scrumAbleTask.ViewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("AddTask", scrumAbleTask);
            }

            GetExternalObjectsFromIds(scrumAbleTask);
            scrumAbleTask.WorkflowStage = _scrumAbleRepo.GetTeamById(user.CurrentWorkingTeam.Id).WorkFlowStages
                .SingleOrDefault(w => w.WorkflowStagePosition == 0);

            _scrumAbleRepo.SaveToDb(scrumAbleTask);
            
            return RedirectToAction("Details", "Task", new {id = scrumAbleTask.Id});
        }

        public IActionResult UpdateTask(ScrumAbleTask scrumAbleTask)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            if (!ModelState.IsValid)
            {
                scrumAbleTask.ViewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("EditTask", scrumAbleTask);
            }

            var firstWorkflowStage = _scrumAbleRepo.GetTeamWorkflowStages(user.CurrentWorkingTeam)
                .First(w => w.WorkflowStagePosition == 0);

            if (scrumAbleTask.WorkflowStage == null)
            {
                scrumAbleTask.WorkflowStage = firstWorkflowStage;
            }

            GetExternalObjectsFromIds(scrumAbleTask);

            _scrumAbleRepo.SaveToDb(scrumAbleTask);

            return RedirectToAction("Details", "Task", new { scrumAbleTask.Id });
        }

        public IActionResult DeleteTask(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTask = _scrumAbleRepo.GetTaskById(id);
            _scrumAbleRepo.DeleteFromDb(scrumAbleTask);

            //TODO redirect back to dashboard
            return RedirectToAction("Details", "Sprint", new {id = scrumAbleTask.Sprint.Id});
        }

        private void GetExternalObjectsFromIds(ScrumAbleTask scrumAbleTask)
        {
            if (scrumAbleTask.TaskOwnerId != null && scrumAbleTask.TaskOwnerId != "-1")
            {
                scrumAbleTask.TaskOwner = _scrumAbleRepo.GetUserByUsername(scrumAbleTask.TaskOwnerId);
            }

            if (scrumAbleTask.TaskSprintId != null)
            {
                scrumAbleTask.Sprint = _scrumAbleRepo.GetSprintById((int) scrumAbleTask.TaskSprintId);
            }
            else if(scrumAbleTask.TaskSprintId == -1)
            {
                //TODO assign to team -> release's backlog once backlogs are generated by default with each sprint
            }

            if (scrumAbleTask.TaskStoryId != null && scrumAbleTask.TaskStoryId != -1)
            {
                scrumAbleTask.Story = _scrumAbleRepo.GetStoryById((int)scrumAbleTask.TaskStoryId);
            }
        }



    }
}
