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
    public class SprintController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public SprintController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var scrumAbleSprint = _scrumAbleRepo.GetSprintById(id);

            if (scrumAbleSprint == null || !_scrumAbleRepo.IsAuthorized(scrumAbleSprint, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("SprintNotFound");
            }

            return View(scrumAbleSprint);
        }

        public IActionResult AddSprint(ScrumAbleSprint scrumAbleSprint)
        {
            ModelState.Clear();
            scrumAbleSprint.SprintStartDate = DateTime.Today;
            scrumAbleSprint.SprintEndDate = DateTime.Today.AddDays(14);

            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleSprint.Release = user.CurrentWorkingRelease;


            ViewBag.data = _scrumAbleRepo.GetAllTeamReleases(user.CurrentWorkingTeam.Id);
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(scrumAbleSprint);
        }

        public IActionResult EditSprint(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var scrumAbleSprint = _scrumAbleRepo.GetSprintById(id);

            if (scrumAbleSprint == null || !_scrumAbleRepo.IsAuthorized(scrumAbleSprint, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("SprintNotFound");
            }

            return View(scrumAbleSprint);
        }

        public IActionResult DeleteSprint(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _scrumAbleRepo.DeleteFromDb(_scrumAbleRepo.GetSprintById(id));
            //TODO redirect back to dashboard
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SetCurrentSprint(int id)
        {
            _scrumAbleRepo.SetCurrentSprint(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            return RedirectToAction("Details", "Sprint", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSprint(ScrumAbleSprint scrumableSprint)
        {
            if (ModelState.ErrorCount > 1)
            {
                return View("AddSprint", scrumableSprint);
            }

            scrumableSprint.Release = _scrumAbleRepo.GetReleaseById(scrumableSprint.SprintReleaseId);
            //scrumAbleRelease.Team = _scrumAbleRepo.GetTeamById(scrumAbleRelease.ReleaseTeamId);

            _scrumAbleRepo.SaveToDb(scrumableSprint);
            _scrumAbleRepo.SetCurrentSprint(User.FindFirstValue(ClaimTypes.NameIdentifier), scrumableSprint.Id);
            return RedirectToAction("Details", "Sprint", new { id = scrumableSprint.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSprint(ScrumAbleSprint scrumAbleSprint)
        {
            if (ModelState.ErrorCount > 1)
            {
                return View("AddSprint", scrumAbleSprint);
            }

            _scrumAbleRepo.SaveToDb(scrumAbleSprint);

            return RedirectToAction("Details", "Sprint", new { id = scrumAbleSprint.Id });
        }
    }

    
}
