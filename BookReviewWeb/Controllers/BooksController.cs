using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookReviewWeb.Data;
using BookReviewWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookReviewWeb.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
              return _context.Book != null ? 
                          View(await _context.Book.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Book'  is null.");
        }

        // GET: Books/Details/5
        // GET: New/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Reviews) // Include the associated reviews
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,UploaderUsername")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.UploaderUsername = User.Identity.Name;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return BadRequest(ModelState);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UploaderUsername")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // AddReview action to create a new review for a book

        // GET: Books/AddReview/5
        // GET: Books/AddReview/5



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int bookId, int rating, string comment)
        {
            // Ensure the book with the specified ID exists
            var book = await _context.Book
    .Include(b => b.Reviews) // Include reviews
    .FirstOrDefaultAsync(m => m.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            // Create a new review
            var review = new Review
            {
                Username = User.Identity.Name,
                Rating = rating,
                Comment = comment,
                BookId = bookId
            };

            // Add the review to the context and save changes
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Redirect back to the book details page
            return RedirectToAction(nameof(Details), new { id = bookId });
        }



        public IActionResult EditReview(int? id, string username)
        {
            // Log the values for debugging
            Console.WriteLine($"id: {id}, username: {username}");

            if (id == null || string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var review = _context.Reviews.FirstOrDefault(r => r.BookId == id && r.Username == username);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing review from the database
                    var existingReview = await _context.Reviews.FindAsync(id);

                    if (existingReview == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing review with the new values
                    existingReview.Rating = review.Rating;
                    existingReview.Comment = review.Comment;

                    // Save the changes to the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict here (e.g., show an error message or redirect)
                    // You can add custom logic to handle concurrency conflicts
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                    return View(review);
                }
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, redisplay the form with validation errors
            return View(review);
        }

        public IActionResult DeleteReview(int? id, string username)
        {
            // Log the values for debugging
           

            if (id == null || string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var review = _context.Reviews.FirstOrDefault(r => r.BookId == id && r.Username == username);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Books/Delete/5
        // POST: Books/DeleteReview
        [HttpPost, ActionName("DeleteReviews")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id, string username)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id && r.Username == username);
            Console.WriteLine($"id: {id}, username: {username}");
            if (review == null)
            {
                return NotFound();
            }

            try
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Handle any exceptions that may occur during deletion
                // You can add custom error handling logic here
                ModelState.AddModelError("", "An error occurred while deleting the review.");
                return RedirectToAction(nameof(Index));
            }

            // Redirect back to the book details page
            return RedirectToAction(nameof(Index));
        }



        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
