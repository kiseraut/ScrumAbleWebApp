using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Models;

namespace ScrumAble.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public ReleaseController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var scrumAbleRelease = _scrumAbleRepo.GetReleaseById(id);

            if (scrumAbleRelease == null || !_scrumAbleRepo.IsAuthorized(scrumAbleRelease, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("ReleaseNotFound");
            }

            scrumAbleRelease.CurrentUser = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            

            return View(scrumAbleRelease);
        }

        public IActionResult SetCurrentRelease(int id)
        {
            _scrumAbleRepo.SetCurrentRelease(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            return RedirectToAction("Details", "Release", new { id = id });
        }

        public IActionResult AddRelease(ScrumAbleRelease scrumAbleRelease)
        {
            ModelState.Clear();
            scrumAbleRelease.ReleaseStartDate = DateTime.Today;
            scrumAbleRelease.ReleaseEndDate = DateTime.Today.AddMonths(3);
            
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleRelease.Team = user.CurrentWorkingTeam;
            scrumAbleRelease.ReleaseTeamId = user.CurrentWorkingTeam.Id;

            ViewBag.data = _scrumAbleRepo.getAllUserTeams(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(scrumAbleRelease);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRelease(ScrumAbleRelease scrumAbleRelease)
        {
            if (ModelState.ErrorCount > 1)
            {
                return View("AddRelease", scrumAbleRelease);
            }

            scrumAbleRelease.Team = _scrumAbleRepo.GetTeamById(scrumAbleRelease.ReleaseTeamId);

            _scrumAbleRepo.SaveToDb(scrumAbleRelease);
            _scrumAbleRepo.SetCurrentRelease(User.FindFirstValue(ClaimTypes.NameIdentifier), scrumAbleRelease.Id);
            return RedirectToAction("Details", "Release", new { id = scrumAbleRelease.Id });
        }

        public IActionResult EditRelease(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var scrumAbleRelease = _scrumAbleRepo.GetReleaseById(id);

            if (scrumAbleRelease == null || !_scrumAbleRepo.IsAuthorized(scrumAbleRelease, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("ReleaseNotFound");
            }

            scrumAbleRelease.Team = user.CurrentWorkingTeam;
            scrumAbleRelease.ReleaseTeamId = user.CurrentWorkingTeam.Id;
            return View(scrumAbleRelease);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRelease(ScrumAbleRelease scrumAbleRelease)
        {
            if (ModelState.ErrorCount > 1)
            {
                return View("AddRelease", scrumAbleRelease);
            }

            scrumAbleRelease.Team = _scrumAbleRepo.GetTeamById(scrumAbleRelease.ReleaseTeamId);

            _scrumAbleRepo.SaveToDb(scrumAbleRelease);

            return RedirectToAction("Details", "Release", new { id = scrumAbleRelease.Id });
        }
        public IActionResult DeleteRelease(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _scrumAbleRepo.DeleteFromDb(_scrumAbleRepo.GetReleaseById(id));
            //TODO redirect back to dashboard
            return RedirectToAction("Index", "Home");
        }
    }
}
