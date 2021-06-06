using Microsoft.AspNetCore.Mvc;
using ScrumAble.Data;
using ScrumAble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Controllers
{
    public class TaskController : Controller
    {

        private readonly ScrumAbleContext _context;
        public TaskController(ScrumAbleContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToSprint(ScrumAbleTask formTask)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", formTask);
            }
            
            formTask.ReleaseId = 0;

            if(formTask.Destination == "Sprint")
            {
                //TODO replace hardcoded data
                formTask.StageId = 1;
                formTask.SprintId = 1;
            }
            else
            {
                formTask.StageId = null;
                formTask.SprintId = null;
            }

            _context.Tasks.Add(formTask);
            _context.SaveChanges();

            //TODO redirect back to dashboard
            return RedirectToAction("Index", "Home");
        }
    }
}
