using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LittleLibrary.ViewModels
{
    public class BookVM
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public string Genre { get; set; }
        public DateTime? DatePublished { get; set; }
        public decimal? Price { get; set; }
        public string Summary { get; set; }
        public IFormFile BookImageFile { get; set; }
        public IFormFile BookContentFile { get; set; }

    }
}
