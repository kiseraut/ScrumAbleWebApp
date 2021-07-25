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
    public class StoryController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public StoryController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
        {
            _scrumAbleRepo = scrumAbleRepo;
            _userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var scrumAbleStory = _scrumAbleRepo.GetStoryById(id);

            if (scrumAbleStory == null || !_scrumAbleRepo.IsAuthorized(scrumAbleStory, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("StoryNotFound");
            }

            return View(scrumAbleStory);
        }

        public IActionResult AddStory(ScrumAbleStory scrumAbleStory)
        {
            ModelState.Clear();

            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleStory.Sprint = user.CurrentWorkingSprint;

            ViewBag.data = _scrumAbleRepo.GetAllUserTeams(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            ViewBag.Sprints = _scrumAbleRepo.GetAllSprintsInRelease(user.CurrentWorkingRelease.Id);

            return View(scrumAbleStory);
        }

        public IActionResult EditStory(int id)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;

            var scrumAbleStory = _scrumAbleRepo.GetStoryById(id);

            if (scrumAbleStory == null || !_scrumAbleRepo.IsAuthorized(scrumAbleStory, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("StoryNotFound");
            }

            scrumAbleStory.StoryOwnerEmail = (scrumAbleStory.StoryOwner == null) ? "-1" : scrumAbleStory.StoryOwner.Email;
            scrumAbleStory.StorySprintId = scrumAbleStory.Sprint.Id;

            ViewBag.data = _scrumAbleRepo.GetAllUserTeams(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Sprints = _scrumAbleRepo.GetAllSprintsInRelease(user.CurrentWorkingRelease.Id);

            return View(scrumAbleStory);
        }

        public IActionResult DeleteStory(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAblestory = _scrumAbleRepo.GetStoryById(id);
            _scrumAbleRepo.DeleteFromDb(scrumAblestory);
            //TODO redirect back to dashboard
            return RedirectToAction("Details", "Sprint", new { id = scrumAblestory.Sprint.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStory(ScrumAbleStory scrumAbleStory)
        {
            if (!ModelState.IsValid) { return View("AddStory", scrumAbleStory); }

            scrumAbleStory.Sprint = _scrumAbleRepo.GetSprintById(scrumAbleStory.StorySprintId);
            scrumAbleStory.StoryOwner = _scrumAbleRepo.GetUserByUsername(scrumAbleStory.StoryOwnerEmail);

            _scrumAbleRepo.SaveToDb(scrumAbleStory);

            return RedirectToAction("Details", "Story", new { id = scrumAbleStory.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStory(ScrumAbleStory scrumAbleStory)
        {
            if (!ModelState.IsValid) { return View("EditStory", scrumAbleStory); }

            scrumAbleStory.Sprint = _scrumAbleRepo.GetSprintById(scrumAbleStory.StorySprintId);
            scrumAbleStory.StoryOwner = _scrumAbleRepo.GetUserByUsername(scrumAbleStory.StoryOwnerEmail);

            _scrumAbleRepo.SaveToDb(scrumAbleStory);

            return RedirectToAction("Details", "Story", new { id = scrumAbleStory.Id });
        }


    }
}
