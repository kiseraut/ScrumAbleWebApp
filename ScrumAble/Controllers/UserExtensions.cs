using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace ScrumAble.Controllers
{

    public class UserExtensions : Controller
    {

        private readonly UserManager<ScrumAbleUser> _userManager;
        private readonly ScrumAbleContext _context;

        public UserExtensions(ScrumAbleContext context, UserManager<ScrumAbleUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public string Thisisatest()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(u => u.Id == userId)
                .Include(u => u.UserTeamMappings)
                    .ThenInclude(utm => utm.Team)
                .SingleOrDefault();


            return user.CurrentWorkingTeam.TeamName;
        }
    }
}
