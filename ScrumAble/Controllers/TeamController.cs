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
    public class TeamController : Controller
    {

        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public TeamController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
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

            var scrumAbleTeam = _scrumAbleRepo.GetTeamById(id);

            if (!_scrumAbleRepo.IsAuthorized(scrumAbleTeam, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("TeamNotFound");
            }

            return View(scrumAbleTeam);

            /*var scrumAbleTask = _scrumAbleRepo.GetTaskById(id);
            scrumAbleTask = _scrumAbleRepo.PopulateTaskMetadata(scrumAbleTask);

            if (scrumAbleTask == null)
            {
                return View("TaskNotFound");
            }
            return View(scrumAbleTask);*/
        }

        public IActionResult EditTeam(int id)
        {

            var scrumAbleTeam = _scrumAbleRepo.GetTeamById(id);

            if (!_scrumAbleRepo.IsAuthorized(scrumAbleTeam, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("TeamNotFound");
            }

            return View(scrumAbleTeam);
        }

        public IActionResult DeleteTeam(int id)
        {
            return View(null);
        }

        public IActionResult UpdateTeam(ScrumAbleTeam team)
        {
            var test = "something";
            //throw new NotImplementedException();
            return View(null);
        }
    }
}
