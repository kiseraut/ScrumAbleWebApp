using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MockScrumAbleUser : ScrumAbleUser
    {
        public static MockScrumAbleUser GenerateScrumAbleUser()
        {
            var rand = new Random();
            var username = "testUser" + rand.Next(1000).ToString() + "@test.com";


            return new MockScrumAbleUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = username,
                Email =  username,
                PasswordHash = "5F4DCC3B5AA765D61D8327DEB882CF99"
            };
        }
    }
}
