using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrumAble.Areas.Identity.Data;
using ScrumAble.Data;

[assembly: HostingStartup(typeof(ScrumAble.Areas.Identity.IdentityHostingStartup))]
namespace ScrumAble.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ScrumAbleContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ScrumAbleContextConnection")));

                services.AddDefaultIdentity<ScrumAbleUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ScrumAbleContext>();
            });
        }
    }
}