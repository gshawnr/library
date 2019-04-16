using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleLibrary.Data;
using LittleLibrary.Models;
using LittleLibrary.Models.Repositories;
using LittleLibrary.Repositories;
using LittleLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace LittleLibrary.Controllers
{
    // This annotation can be used at the class or method level.
    // The annotation could include a comma separated list or different
    // roles.
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private ApplicationDbContext _context;
        private LittleLibraryContext _db;
        private IServiceProvider _serviceProvider;

        public UserRoleController(ApplicationDbContext context, LittleLibraryContext db,
                                    IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _db = db;
        }

        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo(_context);
            var users = userRepo.All();
            return View(users);
        }

        // Show all roles for a specific user.
        public async Task<IActionResult> Detail(string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);
            var roles = await userRoleRepo.GetUserRoles(userName);
            ViewBag.UserName = userName;
            return View(roles);
        }

        // Present user with ability to assign roles to a user.
        // It gives two drop downs - the first contains the user names with
        // the requested user selected. The second drop down contains all
        // possible roles.
        public ActionResult Assign(string userName)
        {
            // Store the email address of the Identity user
            // which is their user name.
            ViewBag.SelectedUser = userName;

            // Build SelectList with role data and store in ViewBag.
            RoleRepo roleRepo = new RoleRepo(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            // There may be a better way but I have always found using the
            // .NET dropdown lists to be a challenge. Here is a way to make
            // it work if you can get the data in the proper format.

            // 1. Preparation for 'Roles' drop down.
            // a) Build a list of SelectListItem objects which have 'Value' and
            // 'Text' properties.
            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                   .ToList();
            // b) Store the SelectListItem objects in a SelectList object
            // with 'Value' and 'Text' properties set specifically.
            var roleList = new SelectList(preRoleList, "Value", "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.RoleSelectList = roleList;

            // 2. Preparation for 'Users' drop down list.
            // a) Build a list of SelectListItem objects which have 'Value' and
            // 'Text' properties.
            var userList = _context.Users.ToList();

            // b) Store the SelectListItem objects in a SelectList object
            // with 'Value' and 'Text' properties set specifically.
            var preUserList = userList.Select(u => new SelectListItem
            { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList, "Value", "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
        [HttpPost]
        public async Task<IActionResult> Assign(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);

            if (ModelState.IsValid)
            {
                var addUR = await userRoleRepo.AddUserRole(userRoleVM.Email,
                                                            userRoleVM.Role);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                       new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> DeleteUserRole(string email, string roleName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);

            var UserManager = _serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var user = await UserManager.FindByEmailAsync(email);

            var userRole = await UserManager.GetRolesAsync(user);

            if (ModelState.IsValid)
            {
                var delUser = await userRoleRepo.RemoveUserRole(email, roleName);

                if (delUser == true && userRole.Count >= 1)
                {
                    var getRole = await UserManager.GetRolesAsync(user);
                    if(getRole.Count > 0)
                    {
                        if (getRole.Contains("Admin"))
                        {
                            roleName = "Admin";
                        }
                        else if (getRole.Contains("Member"))
                        {
                            roleName = "Member";
                        }

                        var deleteUser = await userRoleRepo.RemoveUserRole(email, roleName);
                    }
                }

                if (userRole.Contains("Author") && userRole.Contains("Member") || userRole.Contains("Member"))
                {

                    var getAuthor = (from author in _db.Authors
                                     where author.Email == email
                                     select author).FirstOrDefault();

                    var getBooks = (from book in _db.Books
                                    where book.Author.Email == email
                                    select book).ToList();

                    var getUserFromCart = (from userCart in _db.UsersBooks
                                           where userCart.UserName == email
                                           select userCart).ToList();

                    var getUserFromUserTable = (from u in _db.Users
                                                where u.Email == email
                                                select u).FirstOrDefault();

                    if (getUserFromUserTable != null)
                    {
                        _db.Users.Remove(getUserFromUserTable);
                        _db.SaveChanges();
                    }

                    if (getAuthor != null)
                    {
                        _db.Authors.Remove(getAuthor);
                        _db.SaveChanges();
                    }

                    if (getBooks.Count != 0)
                    {
                        _db.Books.RemoveRange(getBooks);
                        _db.SaveChanges();
                    }

                    if (getUserFromCart.Count != 0)
                    {
                        _db.UsersBooks.RemoveRange(getUserFromCart);
                        _db.SaveChanges();
                    }

                    var removeUser = await UserManager.DeleteAsync(user);
                }
            }
            try
            {
                user = await UserManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Detail", "UserRole",
                       new { userName = email });
            }
            catch
            {
                return View();
            }
        }
    }

}