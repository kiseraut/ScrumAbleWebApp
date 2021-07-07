﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;
using ScrumAble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ScrumAble.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IScrumAbleRepo _scrumAbleRepo;
        private readonly UserManager<ScrumAbleUser> _userManager;

        public TaskController(IScrumAbleRepo scrumAbleRepo, UserManager<ScrumAbleUser> userManager)
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
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTask = _scrumAbleRepo.GetTaskById(id);
            scrumAbleTask = _scrumAbleRepo.PopulateTaskMetadata(scrumAbleTask);

            if (scrumAbleTask == null)
            {
                return View("TaskNotFound");
            }

            return View(scrumAbleTask);
        }

        public IActionResult AddTask(ScrumAbleTask scrumAbleTask)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            scrumAbleTask.viewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ModelState.Clear();
            return View(scrumAbleTask);
        }
        
        public IActionResult EditTask(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var scrumAbleTask = _scrumAbleRepo.GetTaskById(id);
            scrumAbleTask = _scrumAbleRepo.PopulateTaskMetadata(scrumAbleTask);

            if (scrumAbleTask == null)
            {
                return View("TaskNotFound");
            }
            scrumAbleTask.viewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(scrumAbleTask);
        }

        [HttpPost]
        public IActionResult CreateTask(ScrumAbleTask scrumAbleTask)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                scrumAbleTask.viewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("AddTask", scrumAbleTask);
            }

            GetExternalObjectsFromIds(scrumAbleTask);

            _scrumAbleRepo.SaveToDb(scrumAbleTask);
            
            return RedirectToAction("Details", "Task", new {id = scrumAbleTask.Id});
        }

        public IActionResult UpdateTask(ScrumAbleTask scrumAbleTask)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                scrumAbleTask.viewModelTaskAggregate = _scrumAbleRepo.GetTaskAggregateData(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("EditTask", scrumAbleTask);
            }

            GetExternalObjectsFromIds(scrumAbleTask);

            _scrumAbleRepo.SaveToDb(scrumAbleTask);

            return RedirectToAction("Details", "Task", new { scrumAbleTask.Id });
        }

        public IActionResult DeleteTask(int id)
        {
            ViewBag.User = _scrumAbleRepo.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _scrumAbleRepo.DeleteFromDb(_scrumAbleRepo.GetTaskById(id));

            //TODO redirect back to dashboard
            return RedirectToAction("Index", "Home");
        }

        private void GetExternalObjectsFromIds(ScrumAbleTask scrumAbleTask)
        {
            if (scrumAbleTask.TaskOwnerId != null && scrumAbleTask.TaskOwnerId != "-1")
            {
                scrumAbleTask.TaskOwner = _scrumAbleRepo.GetUserByUsername(scrumAbleTask.TaskOwnerId);
            }

            if (scrumAbleTask.TaskSprintId != null)
            {
                scrumAbleTask.Sprint = _scrumAbleRepo.GetSprintById((int) scrumAbleTask.TaskSprintId);
            }
            else if(scrumAbleTask.TaskSprintId == -1)
            {
                //TODO assign to team -> release's backlog once backlogs are generated by default with each sprint
            }

            if (scrumAbleTask.TaskStoryId != null && scrumAbleTask.TaskStoryId != -1)
            {
                scrumAbleTask.Story = _scrumAbleRepo.GetStoryById((int)scrumAbleTask.TaskStoryId);
            }
        }



    }
}
