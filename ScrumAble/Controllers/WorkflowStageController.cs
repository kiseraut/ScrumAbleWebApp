using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Models;

namespace ScrumAble.Controllers
{
    public class WorkflowStageController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public WorkflowStageController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var scrumAbleWorkflowStage = _scrumAbleRepo.GetWorkflowStageById(id);

            if (scrumAbleWorkflowStage == null || !_scrumAbleRepo.IsAuthorized(scrumAbleWorkflowStage, user.Id))
            {
                return View("WorkflowStageNotFound");
            }

            return View(scrumAbleWorkflowStage);
        }

        public IActionResult AddWorkflowStage(ScrumAbleWorkflowStage scrumAbleWorkflowStage)
        {
            ModelState.Clear();

            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewBag.AssociatedWorkflowStages = _scrumAbleRepo.GetTeamWorkflowStages(user.CurrentWorkingTeam);
            ViewBag.data = new List<ScrumAbleWorkflowStage>();
            scrumAbleWorkflowStage.AssociatedWorkflowStages = _scrumAbleRepo.GetTeamWorkflowStages(user.CurrentWorkingTeam);

            //set the default workflow stage name if it is null
            scrumAbleWorkflowStage.WorkflowStageName ??= "New Workflow Stage";
            
            List<int> indexes = new List<int>();
            foreach (var workflowStage in scrumAbleWorkflowStage.AssociatedWorkflowStages)
            {
                indexes.Add(workflowStage.Id);
            }

            //add index for the new workflow stage
            indexes.Add(-1);
            scrumAbleWorkflowStage.NewWorkflowStageOrder = indexes;
            
            return View(scrumAbleWorkflowStage);
        }

        public IActionResult EditWorkflowStage(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var scrumAbleWorkflowStage = _scrumAbleRepo.GetWorkflowStageById(id);

            if (scrumAbleWorkflowStage == null || !_scrumAbleRepo.IsAuthorized(scrumAbleWorkflowStage, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("WorkflowStageNotFound");
            }

            List<int> associatedIds = new List<int>();
            foreach (var workflowStage in scrumAbleWorkflowStage.AssociatedWorkflowStages)
            {
                associatedIds.Add(workflowStage.Id);
            }
            scrumAbleWorkflowStage.NewWorkflowStageOrder = associatedIds;

            return View(scrumAbleWorkflowStage);
        }

        public IActionResult DeleteWorkflowStage(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            _scrumAbleRepo.DeleteFromDb(_scrumAbleRepo.GetWorkflowStageById(id));
            //TODO redirect back to dashboard
            return RedirectToAction("Details", "Team", new { id = user.CurrentWorkingTeam.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWorkflowStage(ScrumAbleWorkflowStage scrumAbleWorkflowStage)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleWorkflowStage.Team = user.CurrentWorkingTeam;

            if (ModelState.ErrorCount > 1) { return View("AddWorkflowStage", scrumAbleWorkflowStage); }

            if (!_scrumAbleRepo.SaveToDb(scrumAbleWorkflowStage, user)) { return View("WorkflowStageNotFound"); }

            return RedirectToAction("Details", "WorkflowStage", new { id = scrumAbleWorkflowStage.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateWorkflowStage(ScrumAbleWorkflowStage scrumAbleWorkflowStage)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleWorkflowStage.Team = user.CurrentWorkingTeam;
            if (ModelState.ErrorCount > 1) { return View("AddWorkflowStage", scrumAbleWorkflowStage); }

            _scrumAbleRepo.SaveToDb(scrumAbleWorkflowStage, _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return RedirectToAction("Details", "WorkflowStage", new { id = scrumAbleWorkflowStage.Id });
        }
    }
}
