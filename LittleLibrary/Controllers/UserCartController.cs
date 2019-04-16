using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleLibrary.Data;
using LittleLibrary.Models;
using LittleLibrary.Repositories;
using LittleLibrary.Services;
using LittleLibrary.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LittleLibrary.Controllers
{
    public class UserCartController : Controller
    {
        private LittleLibraryContext db;
        public ApplicationDbContext userdb;
        private UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string signedInUser { get; set; }

        public UserCartController(LittleLibraryContext db,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor _httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._signInManager = signInManager;
            this._httpContextAccessor = _httpContextAccessor;
            this.userManager = userManager;
        }
        public String GetUser()
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);
            if (_signInManager.IsSignedIn(User))
            {
                string userID = userManager.GetUserId(User);
                HttpContext.Session.SetString("UserID", userID);
                signedInUser = HttpContext.Session.GetString("UserID");
            }
            return signedInUser;
        }

        public String GetUsername()
        {
            string userName = "";
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request, Response);

            if (_signInManager.IsSignedIn(User))
            {
                string userID = userManager.GetUserId(User);
                userName = userManager.GetUserName(User);

                HttpContext.Session.SetString("UserID", userID);
                HttpContext.Session.SetString("Username", userName);

                signedInUser = HttpContext.Session.GetString("UserID");
            }
            return userName;
        }

        public IActionResult AddToWishList(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }
            var signedInUser = GetUsername();
            if(id == 0)
            {
                id = Int32.Parse(HttpContext.Session.GetString("bookId"));
            }
            var query = new WishRepo(db).AddBooksToCart(id, signedInUser);
            return View(query);
        }

        public IActionResult MyWishList()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }
            var signedInUser= GetUsername();
            var query = new WishRepo(db).GetAllBooksFromCart(signedInUser);
            return View("AddToWishList", query);
        }
        public IActionResult Delete(int? id)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);

            var signedInUser = GetUsername();

            var query = (from book in db.UsersBooks
                         where book.BookId == id && book.UserName == signedInUser
                         select book).FirstOrDefault();

            if(query != null)
            {
                db.UsersBooks.Remove(query);
                db.SaveChanges();
            }
            return View(query);
        }

        public IActionResult Checkout(int? id)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request, Response);            

            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }

            var userName = GetUsername();
            Users user = db.Users.Where(un => un.Email == userName).FirstOrDefault();

            HttpContext.Session.SetString("bookId", id.ToString());
            HttpContext.Session.SetString("Username", userName);

            return View(user);
        }
    }
}