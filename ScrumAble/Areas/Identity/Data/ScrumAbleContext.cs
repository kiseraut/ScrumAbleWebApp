using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Migrations;
using ScrumAble.Models;

namespace ScrumAble.Data
{
    public class ScrumAbleContext : IdentityDbContext<ScrumAbleUser>
    {
        public ScrumAbleContext(DbContextOptions<ScrumAbleContext> options)
            : base(options)
        {
        }        
        public DbSet<ScrumAble.Models.ScrumAbleRelease> Releases { get; set; }
        public DbSet<ScrumAble.Models.ScrumAbleSprint> Sprints { get; set; }
        public DbSet<ScrumAble.Models.ScrumAbleStory> Stories { get; set; }
        public DbSet<ScrumAble.Models.ScrumAbleTask> Tasks { get; set; }
        public DbSet<ScrumAble.Models.ScrumAbleTeam> Teams { get; set; }
        public DbSet<ScrumAble.Models.ScrumAbleWorkflowStage> WorkflowStages { get; set; }
        public DbSet<ScrumAble.Models.ScrumAbleUserTeamMapping> UserTeamMapping { get; set; }
        public DbSet<Areas.Identity.Data.ScrumAbleUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

      
    }
}
