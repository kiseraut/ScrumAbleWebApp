using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Models;

namespace ScrumAble.Controllers
{
    public class MetricsController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public MetricsController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            //ViewBag.SprintName = _scrumAbleRepo.GetActiveSprint(user.CurrentWorkingRelease).SprintName;
            ViewBag.SprintName = user.CurrentWorkingSprint.SprintName;

            _scrumAbleRepo.UpdateGraphDataForViewing(user);


            var burndownData = _scrumAbleRepo.GetBurndownData(user);
            if (burndownData != null)
            {
                burndownData.OrderBy(b => DateTime.Parse(b["date"].ToString()));
            }

            ViewBag.BurndownData = burndownData;

            ViewBag.ReleaseName = user.CurrentWorkingRelease.ReleaseName;

            var velocityData = _scrumAbleRepo.GetVelocityData(user);
            ViewBag.VelocityData = velocityData;

            return View();
        }
    }
}
