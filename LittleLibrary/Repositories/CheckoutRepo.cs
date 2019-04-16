using LittleLibrary.Data;
using LittleLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.Repositories
{
    public class CheckoutRepo
    {
        private LittleLibraryContext db;
        private UserManager<ApplicationUser> userManager;

        public CheckoutRepo(LittleLibraryContext db,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public ApplicationUser GetUserDetails(string signedInUser)
        {
            ApplicationUser userInfo = userManager.Users.Where(user => user.Id == signedInUser).FirstOrDefault();
            return userInfo;
        }
    }
}
