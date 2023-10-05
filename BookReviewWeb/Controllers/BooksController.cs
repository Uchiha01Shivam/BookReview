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
using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace BookReviewWeb.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        // GET: Books
        public async Task<IActionResult> Index(string searcher)
        {
            // Retrieve all books by default
            var books = _context.Book.AsQueryable();

            if (!string.IsNullOrEmpty(searcher))
            {
                // Filter books based on the search query for Title or UploaderUsername
                books = books.Where(b => b.Title.Contains(searcher) || b.UploaderUsername.Contains(searcher)|| b.Description.Contains(searcher));
            }

            // Return the filtered list of books to the view
            return View(await books.ToListAsync());
        }


        //[HttpGet]
        //public async Task<IActionResult> GetSuggestions(string query)
        //{
        //    var suggestions = await _context.Book
        //        .Where(b => b.Title.Contains(query) || b.UploaderUsername.Contains(query))
        //        .Select(b => b.Title)
        //        .ToListAsync();

        //    return Json(suggestions);
        //}



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
        public async Task<IActionResult> Create([Bind("Id,Title,Description,UploaderUsername,ImageFile,Author,Publication")] Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.Length > 0)
                {
                    // Define the directory where you want to save the uploaded files
                    var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Ensure the directory exists; if not, create it
                    if (!Directory.Exists(uploadsDirectory))
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                    }

                    // Generate a unique filename for the uploaded image (e.g., using Guid)
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + book.ImageFile.FileName;

                    // Define the path where the image will be saved
                    var imagePath = Path.Combine(uploadsDirectory, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await book.ImageFile.CopyToAsync(stream);
                    }

                    // Update the ImagePath property with the path to the saved image
                    book.Imagepath = Path.Combine("uploads", uniqueFileName);
                }

                book.UploaderUsername = User.Identity.Name;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(book);
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

        public async Task<IActionResult> AddRating(int? id)
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

        [HttpPost]
        public async Task<IActionResult> AddRating(Book book)
        {
            if (ModelState.IsValid)
            {
                // Calculate the Overall rating as the sum of all other properties
                book.OverAll =
                    (book.AuthorCredibility ?? 0) +
                    (book.PublisherCreibility ?? 0) +
                    (book.InGeneral ?? 0) +
                    (book.PhysicalAppearance ?? 0) +
                    (book.SubjectMatter ?? 0) +
                    (book.Language ?? 0) +
                    (book.Illustration ?? 0);

                // Update the book's properties with the submitted ratings
                _context.Update(book);
                await _context.SaveChangesAsync();

                // Redirect back to the book details page or any other appropriate page
                return RedirectToAction("Details", new { id = book.Id });
            }

            return View(book);
        }


        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UploaderUsername,ImageFile,Author,Publication")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if a new image file has been uploaded
                    if (book.ImageFile != null && book.ImageFile.Length > 0)
                    {
                        // Define the directory where you want to save the uploaded files
                        var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                        // Ensure the directory exists; if not, create it
                        if (!Directory.Exists(uploadsDirectory))
                        {
                            Directory.CreateDirectory(uploadsDirectory);
                        }

                        // Generate a unique filename for the uploaded image (e.g., using Guid)
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + book.ImageFile.FileName;

                        // Define the path where the image will be saved
                        var imagePath = Path.Combine(uploadsDirectory, uniqueFileName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await book.ImageFile.CopyToAsync(stream);
                        }

                        // Update the Imagepath property with the path to the saved image
                        book.Imagepath = Path.Combine("uploads", uniqueFileName);
                    }
                    else
                    {
                        // No new image selected, keep the previous image path
                        var existingBook = await _context.Book.FindAsync(book.Id);
                        book.Imagepath = existingBook.Imagepath;

                        // Detach the existing entity from the context
                        _context.Entry(existingBook).State = EntityState.Detached;
                    }

                    // Update the rest of the book properties
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
                return RedirectToAction(nameof(Details), new { id = review.BookId });
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
            return RedirectToAction(nameof(Details), new { id = review.BookId });

        }



        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
