using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleLibrary.Models;
using LittleLibrary.Services;
using LittleLibrary.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace LittleLibrary.Controllers
{
    public class PaymentController : Controller
    {
        private LittleLibraryContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(LittleLibraryContext db, IHttpContextAccessor _httpContextAccessor)
        {
            this.db = db;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public IActionResult Index()
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request, Response);

            string bookId = HttpContext.Session.GetString("bookId");
            string userName = HttpContext.Session.GetString("Username");

            var query = (from book in db.Books
                         where book.BookId == Int32.Parse(bookId)
                         select book).FirstOrDefault();

            ViewBag.TotalPrice = query.Price;
            ViewBag.Email = userName;

            var items = from usr in db.UsersBooks
                        where usr.UserName == userName && usr.BookId == Int32.Parse(bookId)
                        select usr;

            List<PaymentVM> pymtVMs = new List<PaymentVM>();

            foreach(var item in items)
            {
                PaymentVM pymtVM = new PaymentVM();
                var book = db.Books.Where(b => b.BookId == item.BookId).FirstOrDefault();
                pymtVM.bookTitle = book.Title;
                pymtVM.firstName = item.Firstname;
                pymtVM.lastName = item.Lastname;
                pymtVM.amount = item.amount;
                pymtVM.bookID = (Int32)item.BookId;
                pymtVM.payPalEmail = item.PaypalEmail;
                pymtVMs.Add(pymtVM);
            }

            return View(pymtVMs);
        }


        [HttpPost]
        public JsonResult PaySuccess([FromBody]PaymentVM pymt)
        {

            int bookId = Convert.ToInt32(HttpContext.Session.GetString("bookId"));
            pymt.bookID = bookId;
            var purchasedBook = db.Books.Where(b => b.BookId == bookId).FirstOrDefault();
            pymt.bookTitle = purchasedBook.Title;

            string userName = HttpContext.Session.GetString("Username");

            try
            {
                UsersBooks userbook = db.UsersBooks.Where(ub =>
                                    ub.UserName == userName &&
                                    ub.BookId == bookId).FirstOrDefault();

                userbook.Firstname = pymt.firstName;
                userbook.Lastname = pymt.lastName;
                userbook.IsPurchased = true;
                userbook.intent = pymt.intent;
                userbook.paymentId = pymt.paymentId;
                userbook.paymentState = pymt.paymentState;
                userbook.amount = pymt.amount;
                userbook.PaypalEmail = pymt.payPalEmail;
                userbook.paymentMethod = "Paypal";

                db.UsersBooks.Update(userbook);
                db.SaveChanges();

        }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            var items = db.UsersBooks.Where(usr => usr.UserName == userName);
            List<PaymentVM> pymtVMs = new List<PaymentVM>();
            PaymentVM pymtVM = new PaymentVM();
            foreach (var item in items)
            {
                var book = db.Books.Where(b => b.BookId == item.BookId).FirstOrDefault();
                pymtVM.firstName = item.Firstname;
                pymtVM.lastName = item.Lastname;
                pymtVM.amount = item.amount;
                pymtVM.bookID = (Int32)item.BookId;
                pymtVM.payPalEmail = item.PaypalEmail;
                pymtVM.bookTitle = book.Title;
                pymtVMs.Add(pymtVM);
            }
            return Json(pymtVMs);
}

        [HttpGet]
        public IActionResult GetAccountDetails()
        {
            var query = from user in db.UsersBooks
                         where user.UserName == HttpContext.Session.GetString("Username")
                         && user.IsPurchased == true
                         select user;

            var getBook = (from user in db.UsersBooks
                        where user.UserName == HttpContext.Session.GetString("Username")
                        && user.IsPurchased == true
                        select user).FirstOrDefault();

            if(getBook != null)
            {
                var bookName = (from book in db.Books
                                where book.BookId == getBook.BookId
                                select book).FirstOrDefault();

                ViewData["BookName"] = bookName.Title;
            }

            if(query == null)
            {
                return View();
            }

            return View(query);
        }

    }
}