using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Http;
using LittleLibrary.Models;

namespace LittleLibrary.Repositories
{
    public class WishRepo
    {
        private LittleLibraryContext db;
        private IEnumerable<Books> UserBooks { get; set; }
        public int bookid { get; set; }
        public string username { get; set; }

        public WishRepo(LittleLibraryContext db)
        {
            this.db = db;
        }
        public IEnumerable<Books> AddBooksToCart(int id, string signedInUser)
        {
            var book = (from b in db.UsersBooks
                        where b.BookId == id && b.UserName == signedInUser
                        select b).FirstOrDefault();

            var user = (from u in db.Users
                          where u.Email == signedInUser
                          select u).FirstOrDefault();

            if(book == null)
            {
                UsersBooks usersBook = new UsersBooks();

                usersBook.BookId = id;
                usersBook.UserId = user.UserId;
                usersBook.UserName = signedInUser;
                usersBook.IsPurchased = false;

                db.UsersBooks.Add(usersBook);
                db.SaveChanges();
                GetAllBooksFromCart(signedInUser);
                return UserBooks;
            }
            else
            {
                GetAllBooksFromCart(signedInUser);
            }
            return UserBooks;
        }

        public IEnumerable<Books> GetAllBooksFromCart(string signedInUser)
        {
            var user = from u in db.UsersBooks
                       where u.UserName == signedInUser
                       select u;

            UserBooks = from book in db.Books
                        from userBook in user
                        where book.BookId == userBook.BookId
                        select book;

            return UserBooks;
        }
    }
}
