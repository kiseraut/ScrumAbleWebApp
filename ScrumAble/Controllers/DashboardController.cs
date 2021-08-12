using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Migrations;
using ScrumAble.Models;

namespace ScrumAble.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public DashboardController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var sprint = new ScrumAbleSprint();
            ViewBag.User = user;

            if (!_scrumAbleRepo.PrepareUserForDashboard(user))
            {
                return View("UserNotInitialized");
            }

            if (user.CurrentWorkingSprint.IsBacklog)
            {
                return Redirect("Backlog");
            }

            sprint = _scrumAbleRepo.GetSprintForDashboard(user.CurrentWorkingSprint.Id);
            ViewBag.ActiveSprint = _scrumAbleRepo.GetActiveSprint(user.CurrentWorkingRelease);


            return View(sprint);
        }

        public IActionResult Backlog()
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            return View(user.CurrentWorkingSprint);
        }

        public IActionResult UserNotInitialized()
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            return View();
        }

        public bool ToggleSprint(int sprintId, DateTime SprintEndDate)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var sprint = _scrumAbleRepo.GetSprintById(sprintId);

            if (sprint.IsActiveSprint)
            {
                sprint.SprintEndDate = DateTime.Now;
                sprint.IsActiveSprint = false;
                sprint.IsCompleted = true;
            }
            else
            {
                sprint.SprintStartDate = DateTime.Now;
                sprint.SprintEndDate = SprintEndDate;
                sprint.IsActiveSprint = true;
                sprint.SprintPlanned = _scrumAbleRepo.GetActiveSprintPoints(sprint);
                sprint.SprintActual = sprint.SprintPlanned;
                var sprintDays = (SprintEndDate - DateTime.Today).TotalDays;
                var graphData = "";
                var dateReductionFactor = sprint.SprintActual / (sprintDays);

                for (var i = 0; i <= sprintDays; i++)
                {
                    var separator = i==0 ? "" : "#";
                    var Actual = i==0 ? sprint.SprintActual.ToString() : "null";

                    var tempPlanned = Math.Round(sprint.SprintActual - (i * dateReductionFactor),1);

                    graphData += string.Format("{0}{1},{2},{3}", separator, DateTime.Now.AddDays(i).ToString("d"), tempPlanned, Actual);
                }

                sprint.GraphData = graphData;

            }

            _scrumAbleRepo.SaveToDb(sprint);


            return true;
        }

        public IActionResult GoToActiveSprint()
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            user.CurrentWorkingSprint = _scrumAbleRepo.GetActiveSprint(user.CurrentWorkingRelease);
            _scrumAbleRepo.SaveToDb(user);

            return RedirectToAction("Index");
        }

        public bool MoveWorkItem(string workItemDivId, string workflowStageDivId)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var workflowStageIdString = workflowStageDivId.Split('_')[1];
            int workflowStageId;

            if (!int.TryParse(workflowStageIdString, out workflowStageId))
            {
                return false;
            }

            var workItemIdString = workItemDivId.Split('_')[2];
            int workItemId;

            if (!int.TryParse(workItemIdString, out workItemId))
            {
                return false;
            }

            var workItemType = workItemDivId.Split('_')[0];
            switch (workItemType)
            {
                case "story":
                    _scrumAbleRepo.MoveStory(workItemId, workflowStageId, user);
                    break;
                case "task":
                    _scrumAbleRepo.MoveTask(workItemId, workflowStageId, user);
                    break;
                case "defect":
                    _scrumAbleRepo.MoveDefect(workItemId, workflowStageId, user);
                    break;
                default:
                    return false;
                    break;
            }

            return true;
        }
    }
}
