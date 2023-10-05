using BookReviewWeb.Data;
using BookReviewWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReviewWeb.Controllers
{
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FAQController(ApplicationDbContext context)
        {
           _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var faqs = await _dbContext.FAQs.AsQueryable().ToListAsync();
            return View(faqs);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
      

        public async Task<IActionResult> Create([Bind("Id,question,answer")] FAQ faq)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Add(faq);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.FAQs == null)
            {
                return NotFound();
            }

            var faq = await _dbContext.FAQs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        [HttpPost, ActionName("DeleteFAQ")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            if (_dbContext.FAQs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book'  is null.");
            }
            var faq = await _dbContext.FAQs.FindAsync(id);
            if (faq != null)
            {
                _dbContext.FAQs.Remove(faq);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.FAQs == null)
            {
                return NotFound();
            }
            var faq = await _dbContext.FAQs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("Id,answer,question")] FAQ faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing review from the database
                    var existingfaq = await _dbContext.FAQs.FindAsync(id);

                    if (existingfaq == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing review with the new values
                    existingfaq.question = faq.question;
                    existingfaq.answer = faq.answer;

                    // Save the changes to the database
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict here (e.g., show an error message or redirect)
                    // You can add custom logic to handle concurrency conflicts
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                    return View(faq);
                }
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, redisplay the form with validation errors
            return View(faq);
        }
    }

    }

