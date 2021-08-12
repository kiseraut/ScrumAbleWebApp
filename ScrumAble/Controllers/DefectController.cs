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
    public class DefectController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public DefectController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var scrumAbleDefect = _scrumAbleRepo.GetDefectById(id);

            if (scrumAbleDefect == null || !_scrumAbleRepo.IsAuthorized(scrumAbleDefect, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("DefectNotFound");
            }

            return View(scrumAbleDefect);
        }

        public IActionResult AddDefect(ScrumAbleDefect scrumAbleDefect)
        {
            ModelState.Clear();

            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleDefect.Sprint = user.CurrentWorkingSprint;
            scrumAbleDefect.Release = user.CurrentWorkingRelease;

            ViewBag.data = _scrumAbleRepo.GetAllUserTeams(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            ViewBag.Sprints = _scrumAbleRepo.GetAllSprintsInRelease(user.CurrentWorkingRelease.Id);

            return View(scrumAbleDefect);
        }

        public IActionResult EditDefect(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var scrumAbleDefect = _scrumAbleRepo.GetDefectById(id);

            if (scrumAbleDefect == null || !_scrumAbleRepo.IsAuthorized(scrumAbleDefect, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("DefectNotFound");
            }

            scrumAbleDefect.DefectOwnerEmail = (scrumAbleDefect.DefectOwner == null) ? "-1" : scrumAbleDefect.DefectOwner.Email;
            scrumAbleDefect.DefectSprintId = scrumAbleDefect.Sprint.Id;
            scrumAbleDefect.DefectReleaseId = scrumAbleDefect.Release.Id;

            ViewBag.data = _scrumAbleRepo.GetAllUserTeams(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Sprints = _scrumAbleRepo.GetAllSprintsInRelease(user.CurrentWorkingRelease.Id);

            return View(scrumAbleDefect);
        }

        public IActionResult DeleteDefect(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleDefect = _scrumAbleRepo.GetDefectById(id);
            _scrumAbleRepo.DeleteFromDb(scrumAbleDefect);
            return RedirectToAction("Details", "Release", new { id = scrumAbleDefect.Release.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDefect(ScrumAbleDefect scrumAbleDefect)
        {
            if (!ModelState.IsValid) { return View("AddDefect", scrumAbleDefect); }

            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleDefect.Sprint = _scrumAbleRepo.GetSprintById(scrumAbleDefect.DefectSprintId);
            scrumAbleDefect.Release = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier)).CurrentWorkingRelease;
            scrumAbleDefect.DefectOwner = _scrumAbleRepo.GetUserByUsername(scrumAbleDefect.DefectOwnerEmail);
            scrumAbleDefect.WorkflowStage = _scrumAbleRepo.GetTeamById(user.CurrentWorkingTeam.Id).WorkFlowStages
                .SingleOrDefault(w => w.WorkflowStagePosition == 0);

            _scrumAbleRepo.SaveToDb(scrumAbleDefect, user);

            return RedirectToAction("Details", "Defect", new { id = scrumAbleDefect.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDefect(ScrumAbleDefect scrumAbleDefect)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            ViewBag.data = _scrumAbleRepo.GetAllUserTeams(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Sprints = _scrumAbleRepo.GetAllSprintsInRelease(user.CurrentWorkingRelease.Id);
            scrumAbleDefect.Sprint = _scrumAbleRepo.GetSprintById(scrumAbleDefect.DefectSprintId);
            scrumAbleDefect.Release = _scrumAbleRepo.GetReleaseById(scrumAbleDefect.DefectReleaseId);
            scrumAbleDefect.DefectOwner = _scrumAbleRepo.GetUserByUsername(scrumAbleDefect.DefectOwnerEmail);

            scrumAbleDefect.DefectOwnerEmail = (scrumAbleDefect.DefectOwner == null) ? "-1" : scrumAbleDefect.DefectOwner.Email;
            scrumAbleDefect.DefectSprintId = scrumAbleDefect.Sprint.Id;
            scrumAbleDefect.DefectReleaseId = scrumAbleDefect.Release.Id;


            if (!ModelState.IsValid) { return View("EditDefect", scrumAbleDefect); }

            

            _scrumAbleRepo.SaveToDb(scrumAbleDefect, user);

            return RedirectToAction("Details", "Defect", new { id = scrumAbleDefect.Id });
        }
    }
}
