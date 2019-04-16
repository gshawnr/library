using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LittleLibrary.Models
{
    public partial class Books
    {
        public Books()
        {
            UsersBooks = new HashSet<UsersBooks>();
        }

        public int BookId { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public string Genre { get; set; }
        public DateTime? DatePublished { get; set; }
        public decimal? Price { get; set; }
        public string Summary { get; set; }
        public byte[] BookCover { get; set; }
        public byte[] BookContent { get; set; }

        public Authors Author { get; set; }
        public ICollection<UsersBooks> UsersBooks { get; set; }
    }
}
