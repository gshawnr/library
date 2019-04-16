using LittleLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.Repositories
{
    public class BookRepo
    {
        private LittleLibraryContext db;

        public BookRepo(LittleLibraryContext db)
        {
            this.db = db;
        }
        public IQueryable<Books> GetAll(string searchString)
        {

            if (!String.IsNullOrEmpty(searchString))
            {             

                var query = from book in db.Books
                            where (book.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
                            select book;
                
                return query;

              
            }
            else
            {
                var query = from book in db.Books
                            select book;
                return query;
            }
        }

    }
}
