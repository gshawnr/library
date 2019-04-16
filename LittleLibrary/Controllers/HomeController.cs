using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LittleLibrary.Models;
using LittleLibrary.Services;
using Microsoft.Extensions.Options;
using LittleLibrary.Repositories;
using System.Security.Claims;
using LittleLibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace LittleLibrary.Controllers
{
    public class HomeController : Controller
    {

        private EmailSettings _emailSettings;
        private LittleLibraryContext db;
        public ApplicationDbContext userdb;
        private UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IOptions<EmailSettings> _emailSettings,
            LittleLibraryContext db,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor _httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._emailSettings = _emailSettings.Value;
            this._signInManager = signInManager;
            this._httpContextAccessor = _httpContextAccessor;
            this.userManager = userManager;
        }
        public IActionResult Index(string searchString)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                             Response);


            if (_signInManager.IsSignedIn(User))
            {
                string userID = userManager.GetUserId(User);
                string userName = userManager.GetUserName(User);
                cookieHelper.Set("UserID", userID, 1);
                HttpContext.Session.SetString("UserID", userID);
                HttpContext.Session.SetString("Username", userName);
            }
            string searchBook = String.IsNullOrEmpty(searchString) ? "" : searchString;
            ViewData["CurrentFilter"] = searchBook;
            var queryWithSearchString = new BookRepo(db).GetAll(searchString);
            return View(queryWithSearchString);
        }

        public IActionResult BookSearch(string searchString)
        {
            string searchBook = String.IsNullOrEmpty(searchString) ? "" : searchString;

            ViewData["CurrentFilter"] = searchBook;

            var queryWithSearchString = new BookRepo(db).GetAll(searchString);

            return View(queryWithSearchString);
        }

        public IActionResult Details(int? id)
        {
            var book = (from b in db.Books
                        where b.BookId == id
                        select b).FirstOrDefault();
            return View(book);
        }

        [Authorize(Roles="Admin")]

        public IActionResult DeleteBook(int? id)
        {
            var userBooks = db.UsersBooks.Where(b => b.BookId == id);
            db.UsersBooks.RemoveRange(userBooks);
            var bookToDelete = db.Books.Where(b => b.BookId == id).FirstOrDefault();
            db.Books.Remove(bookToDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ViewModels.ContactVM ctVM)
        {
            if (new EmailHelper(_emailSettings).SendMail(ctVM._email, "Thank you.", "", ctVM.textBox))
                return RedirectToAction("Index");
            return RedirectToAction("Error");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
