using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LittleLibrary.Models.Repositories;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using LittleLibrary.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using LittleLibrary.Data;
using LittleLibrary.Services;

namespace LittleLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        private LittleLibraryContext db;
        private IServiceProvider _serviceProvider;
        private IHostingEnvironment hostingEnvironment;
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorsController(LittleLibraryContext db, IServiceProvider serviceProvider,
                       IHostingEnvironment _hostingEnvironment, UserManager<ApplicationUser> userManager,
                       SignInManager<ApplicationUser> signInManager, IHttpContextAccessor _httpContextAccessor)
        {
            this.db = db;
            _serviceProvider = serviceProvider;
            hostingEnvironment = _hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            this._httpContextAccessor = _httpContextAccessor;
        }
        public IActionResult Index(string searchString)
        {
            string searchAuthor = String.IsNullOrEmpty(searchString) ? "" : searchString;

            ViewData["CurrentFilter"] = searchAuthor.Trim();
            var authorlist = new AuthorRepo(db).GetAuthors(searchAuthor.Trim());
            return View(authorlist);


        }


        public IActionResult Details(int? id)
        {
            var author = (from au in db.Authors
                          where au.AuthorId == id
                          select au).FirstOrDefault();

            ViewData["AuthorFirstName"] = author.Firstname;
            ViewData["AuthorLastName"] = author.Lastname;
            ViewData["AuthorProfile"] = author.authorProfile;

            var authorBooks = from book in db.Books
                              where book.AuthorId == author.AuthorId
                              select book;

            if(authorBooks == null)
            {
                return View();
            }

            return View(authorBooks);
        }

        [Authorize(Roles = "Author")]
        public IActionResult UploadBook()
        {
            return View();
        }

        private Authors getAuthor()
        {
            _signInManager.IsSignedIn(User);

                string userID = _userManager.GetUserId(User);
                var userName = _userManager.GetUserName(User);
                Authors author = (from au in db.Authors where au.Email == userName select au).FirstOrDefault();
                return author;

        }

        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<ActionResult> UploadAndSaveBook(string title, string genre, DateTime datePublished, decimal price, string summary, IFormFile BookImageFile, IFormFile BookContentFile)
        {

            if (title == null || genre == null || price <= 0 || summary == null)
            {
                Console.WriteLine("All Fields need to be filled in");
            }

            string webRoot = hostingEnvironment.WebRootPath;
            string bookCoverPath = webRoot + "/bookuploads/bookcovers";
            string bookContentPath = webRoot + "/bookuploads/bookcontents";

            Books book = new Books();

            var imagePath = Path.GetTempFileName();

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await BookImageFile.CopyToAsync(stream);
            }
            byte[] imageBookBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageBase64String = Convert.ToBase64String(imageBookBytes);
            book.BookCover = Convert.FromBase64String(imageBase64String);


            if (BookContentFile == null || BookContentFile.Length == 0)
                return Content("file not selected");

            var path = Path.GetTempFileName();

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await BookContentFile.CopyToAsync(stream);
            }

            byte[] bookBytes = System.IO.File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(bookBytes);
            book.BookContent = Convert.FromBase64String(base64String);

            Authors author = getAuthor();

            book.Title = title;
            book.Genre = genre;
            book.DatePublished = datePublished;
            book.Price = price;
            book.Summary = summary;
            book.AuthorId = author.AuthorId;

            Console.WriteLine(book.BookContent);

            db.Books.Add(book);
            db.SaveChanges();

            return View();
        }

        public IActionResult DownLoadFile(int id)
        {
            var query = (from book in db.Books
                         where book.BookId == id
                         select book).FirstOrDefault();

            return File(query.BookContent, "application/pdf", (query.Title) + ".pdf");
        }

        public IActionResult DownloadBook(int id)
        {
            _signInManager.IsSignedIn(User);

            UsersBooks books = new UsersBooks();
            var getBook = (from b in db.UsersBooks
                           where b.BookId == id && b.UserName == HttpContext.Session.GetString("Username")
                           select b).FirstOrDefault();

            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }
            else if (getBook != null && getBook.IsPurchased == false) {
                HttpContext.Session.SetString("bookId", (getBook.BookId).ToString());
                return RedirectToAction("Index", "Payment");
            }
            else if(getBook == null)
            {
                HttpContext.Session.SetString("bookId", (id).ToString());
                return RedirectToAction("AddToWishList", "UserCart");
            }
            else{
                return DownLoadFile(id);
            }
        }

        public ActionResult RenderImage(int id)
        {
            var query = (from book in db.Books
                         where book.BookId == id
                         select book).FirstOrDefault();

            if (query != null)
            {
                byte[] image = query.BookCover;
                return File(image, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult RegisterAuthor()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> RegisterAuthor(Authors author)
        {
            // Should be done asynch incase of db errors
            db.Authors.Add(author);
            db.SaveChanges();
            UserRoleRepo ur = new UserRoleRepo(_serviceProvider);
            bool success = await ur.AddUserRole(author.Email, "Author");

            if (success)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
