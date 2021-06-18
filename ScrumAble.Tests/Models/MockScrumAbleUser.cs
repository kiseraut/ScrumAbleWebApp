using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            return new MockScrumAbleUser()
            {
                Id = "aaaa-bbbb-1111-2222",
                UserName = "testUser@test.com",
                PasswordHash = "5F4DCC3B5AA765D61D8327DEB882CF99"
            };
        }
    }
}
