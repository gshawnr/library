using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LittleLibrary.Models;

namespace LittleLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LittleLibraryContext _context;

        public BooksController(LittleLibraryContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public IEnumerable<Books> GetBooks()
        {
            IEnumerable<Books> bookList = _context.Books.Select(bl => new Books
            {
                Title = bl.Title,
                Price = bl.Price,
                Genre = bl.Genre,
                Summary = bl.Summary,
            });
            return bookList;
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = await _context.Books.FindAsync(id);

            if (books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}