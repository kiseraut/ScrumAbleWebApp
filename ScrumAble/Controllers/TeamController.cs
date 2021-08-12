using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
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
        
        public IActionResult Details(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTeam = _scrumAbleRepo.GetTeamById(id);

            if (scrumAbleTeam == null || !_scrumAbleRepo.IsAuthorized(scrumAbleTeam, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {

                return View("TeamNotFound");
            }

            return View(scrumAbleTeam);
        }

        public IActionResult EditTeam(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTeam = _scrumAbleRepo.GetTeamById(id);

            if (scrumAbleTeam == null || !_scrumAbleRepo.IsAuthorized(scrumAbleTeam, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return View("TeamNotFound");
            }

            return View(scrumAbleTeam);
        }

        public IActionResult DeleteTeam(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _scrumAbleRepo.DeleteFromDb(_scrumAbleRepo.GetTeamById(id));
            //TODO redirect back to dashboard
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult UpdateTeam(ScrumAbleTeam team)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var badUsers = new List<string>();
            var goodUsers = new List<IScrumAbleUser>();
            if (team.TeammatesText == null)
            {
                team.TeammatesText += "\r\n" + User.FindFirstValue(ClaimTypes.Email);
            }
            var addedUsersWithDups = team.TeammatesText.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);
            var addedUsers = addedUsersWithDups.Distinct();

            foreach (var user in addedUsers)
            {
                var foundUser = _scrumAbleRepo.GetUserByUsername(user);
                if (foundUser == null)
                {
                    badUsers.Add(user);
                    ModelState.AddModelError("Teammates", "Teammates are invalid");
                }
                else
                {
                    goodUsers.Add(foundUser);
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.data = badUsers;
                return View("EditTeam", team);
            }
            
            _scrumAbleRepo.SaveToDb(team, goodUsers);
            return RedirectToAction("Details", "Team", new { id = team.Id });
        }

        public IActionResult AddTeam(ScrumAbleTeam scrumAbleTeam)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ModelState.Clear();
            return View(scrumAbleTeam);
        }

        [HttpPost]
        public IActionResult CreateTeam(ScrumAbleTeam team)
        {
            var user = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.User = user;
            var badUsers = new List<string>();
            var goodUsers = new List<IScrumAbleUser>();
            team.TeammatesText += "\r\n" + User.FindFirstValue(ClaimTypes.Email);

            if (team.TeammatesText != null)
            {
                
                var addedUsersWithDups = team.TeammatesText.Split(Environment.NewLine,
                    StringSplitOptions.RemoveEmptyEntries);
                var addedUsers = addedUsersWithDups.Distinct();

                foreach (var teammate in addedUsers)
                {
                    var foundUser = _scrumAbleRepo.GetUserByUsername(teammate);
                    if (foundUser == null)
                    {
                        badUsers.Add(teammate);
                        ModelState.AddModelError("Teammates", "Teammates are invalid");
                    }
                    else
                    {
                        goodUsers.Add(foundUser);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.data = badUsers;
                return View("AddTeam", team);
            }

            _scrumAbleRepo.SaveToDb(team, goodUsers);

            var orderList = new List<int>() {-1};

            var openWFStage = new ScrumAbleWorkflowStage
            {
                WorkflowStageName = "Planned",
                Team = team,
                WorkflowStagePosition = 0,
                NewWorkflowStageOrder = orderList
            };

            _scrumAbleRepo.SaveToDb(openWFStage, user);
            orderList.Clear();
            orderList.Add(openWFStage.Id);
            orderList.Add(-1);

            var inProgressWFStage = new ScrumAbleWorkflowStage
            {
                WorkflowStageName = "In Progress",
                Team = team,
                WorkflowStagePosition = 1,
                NewWorkflowStageOrder = orderList
            };

            _scrumAbleRepo.SaveToDb(inProgressWFStage, user);
            orderList.Clear();
            orderList.Add(openWFStage.Id);
            orderList.Add(inProgressWFStage.Id);
            orderList.Add(-1);

            var ClosedWFStage = new ScrumAbleWorkflowStage
            {
                WorkflowStageName = "Closed",
                Team = team,
                WorkflowStagePosition = 2,
                NewWorkflowStageOrder = orderList
            };

            _scrumAbleRepo.SaveToDb(ClosedWFStage, user);

            _scrumAbleRepo.SetCurrentTeam(user.Id, team.Id);

            return RedirectToAction("Details", "Team", new { id = team.Id });
        }

        public IActionResult SetCurrentTeam(int id)
        {
            _scrumAbleRepo.SetCurrentTeam(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            return RedirectToAction("Details", "Team", new { id = id });
        }
    }
}
