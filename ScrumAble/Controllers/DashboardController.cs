using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ScrumAble.Areas.Identity.Data;
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

            if (user.CurrentWorkingSprint != null)
            {
                sprint = _scrumAbleRepo.GetSprintForDashboard(user.CurrentWorkingSprint.Id);
            }
            return View(sprint);
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
