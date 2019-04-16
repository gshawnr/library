using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.Models.Repositories
{
    public class AuthorRepo
    {
        private LittleLibraryContext _db;

        public AuthorRepo(LittleLibraryContext context)
        {
            _db = context;
        }

        public IQueryable<Authors> GetAuthors(string authorName)
        {
            IQueryable<Authors> authorList;

            authorList = _db.Authors.Where(au =>
                         au.Firstname.Contains(authorName,StringComparison.CurrentCultureIgnoreCase) ||
                         au.Lastname.Contains(authorName, StringComparison.CurrentCultureIgnoreCase));

            return authorList;
        }






    }
}
