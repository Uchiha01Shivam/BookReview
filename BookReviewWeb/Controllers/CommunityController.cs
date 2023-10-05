using BookReviewWeb.Data;
using BookReviewWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookReviewWeb.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunityController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.discussions
         .Include(d => d.Community)
         .Include(d => d.Likes)// Include the related Community for each discussion
         .ToListAsync(); // Retrieve all discussions
            if (discussions == null)
            {
                return NotFound();
            }
            return View(discussions);

        }

        public async Task<IActionResult> EditQues(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var ques = await _context.discussions.FindAsync(id);

            if (ques != null)
            {
                return View(ques);
            }
            else { return NotFound(); }
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id,  Discussion discussion)
        {
            if(id!=discussion.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                var existingques = await _context.discussions.FindAsync(id);
                existingques.question = discussion.question;
              
              
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else { return NotFound(); }
        }
        [HttpGet]

        public async Task<IActionResult> DeleteQues(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var ques = await _context.discussions.FindAsync(id);
            if (ques != null)
            {
                return View(ques);
            }
            else { return NotFound(); }
        }

        [HttpPost,ActionName("DeleteQues")]

        public async Task<IActionResult> DeleteQues(int id, Discussion discussion)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ques = await _context.discussions.FindAsync(id);

            if(ques != null)
            {
                _context.discussions.Remove(ques);
               
            }
           await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> SeeMore(int? id)
        {
            if(id== null || _context.discussions==null)
            {
                return NotFound();
            }
            var comm = await _context.discussions
                .Include(b=>b.Likes)
                .Include(b=>b.Community)
                .ThenInclude(l=>l.AnsLikes)
               
                // Include the associated reviews
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comm == null)
            {
                return NotFound();
            }

            return View(comm);
        }

        [HttpPost]

        public async Task<IActionResult> Create([Bind("Id,question,User")] Discussion discussion)
        {
            if(ModelState.IsValid)
            {
                discussion.User = User.Identity.Name;
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        public async Task<IActionResult> EditAnswer(int? id,string UserName)
        {
            if (id == null || string.IsNullOrEmpty(UserName))
            {
                return NotFound();
            }

            var ans = _context.Community.FirstOrDefault(r => r.Id == id && r.UserName == UserName);

            if (ans == null)
            {
                return NotFound();
            }

            return View(ans);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAnswer(Community community)
        {
            if (ModelState.IsValid)
            {
               
                var existingAnswer = await _context.Community.FindAsync(community.Id);

                if (existingAnswer == null || existingAnswer.UserName != community.UserName)
                {
                    return NotFound();
                }

                // Update the answer content
                existingAnswer.Answer = community.Answer;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Redirect back to the "SeeMore" action with the same question ID
                return RedirectToAction("SeeMore", new { id = existingAnswer.QuestionId });
            }

            // If model state is not valid, return the view with validation errors
            return View(community);
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int?id,string username)
        {
            if(id==null|| string.IsNullOrEmpty(username)) {
                return NotFound();
            }
            var ans = _context.Community.FirstOrDefault(r => r.Id == id && r.UserName == username);
            if (ans == null)
            {
                return NotFound();
            }

            return View(ans);
        }


        [HttpPost, ActionName("DeleteAns")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAns(int id, string username)
        {
            // Find the answer in the database by its ID and username
            var existingAnswer = await _context.Community.FirstOrDefaultAsync(r => r.Id == id && r.UserName == username);

            if (existingAnswer == null)
            {
                return NotFound();
            }

            // Remove the answer from the database
            _context.Community.Remove(existingAnswer);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the question's details page
            return RedirectToAction("SeeMore", new { id = existingAnswer.QuestionId });
        }


        [HttpPost]

        public async Task<IActionResult> CreateAnswer(int questionid, string answer,string username) 
        {
            // Ensure the book with the specified ID exists
            var ans = await _context.discussions
    .Include(b => b.Community) // Include reviews
    .FirstOrDefaultAsync(m => m.Id == questionid);

            if (ans == null)
            {
                return NotFound();
            }

            var comm = new Community
            {
                UserName = User.Identity.Name,
                 Answer = answer,
                 QuestionId = questionid
            };
            _context.Community.Add(comm);
            await _context.SaveChangesAsync();
            return RedirectToAction("SeeMore", new { id = ans.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Like(int discussionId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to a login page or return an error
                return RedirectToAction("Login", "Account"); // Replace with your login action and controller
            }

            // Find the discussion by ID
            var discussion = await _context.discussions
                .Include(d => d.Likes)
                .SingleOrDefaultAsync(d => d.Id == discussionId);

            if (discussion == null)
            {
                return NotFound(); // Discussion not found
            }

            // Check if the user has already liked this discussion
            var userLike = discussion.Likes.FirstOrDefault(like => like.UserName == User.Identity.Name);

            if (userLike != null)
            {
                // User has already liked, toggle the like
                if (userLike.Like == 1)
                {
                    userLike.Like = 0;
                }
                else { userLike.Like = 1;
                    userLike.DisLike = 0;
                }// Toggle the like by negating its current value
            }
            else
            {
                // Add a like for the current user
                discussion.Likes.Add(new Likes
                {
                    UserName = User.Identity.Name,
                    DiscussionId = discussionId,
                    Like = 1
                });
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the discussion page or wherever you prefer
            return RedirectToAction("SeeMore", new { id = discussionId });
        }

        [HttpPost]
        public async Task<IActionResult> DisLike(int discussionId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to a login page or return an error
                return RedirectToAction("Login", "Account"); // Replace with your login action and controller
            }

            // Find the discussion by ID
            var discussion = await _context.discussions
                .Include(d => d.Likes)
                .SingleOrDefaultAsync(d => d.Id == discussionId);

            if (discussion == null)
            {
                return NotFound(); // Discussion not found
            }

            // Check if the user has already liked this discussion
            var userLike = discussion.Likes.FirstOrDefault(like => like.UserName == User.Identity.Name);

            if (userLike != null)
            {
                // User has already liked, toggle the like
                if (userLike.DisLike == 1)
                {
                    userLike.DisLike = 0;
                }
                else
                {
                    userLike.DisLike = 1;
                    userLike.Like = 0; // Remove the like if it exists
                }
            }
            else
            {
                // Add a like for the current user
                discussion.Likes.Add(new Likes
                {
                    UserName = User.Identity.Name,
                    DiscussionId = discussionId,
                    DisLike = 1
                });
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the discussion page or wherever you prefer
            return RedirectToAction("SeeMore", new { id = discussionId });
        }


        [HttpPost]
        public async Task<IActionResult> LikeAnswer(int answerId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to a login page or return an error
                return RedirectToAction("Login", "Account"); // Replace with your login action and controller
            }

            // Find the answer by ID
            var answer = await _context.Community
                .Include(a => a.AnsLikes)
                .SingleOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                return NotFound(); // Answer not found
            }

            // Check if the user has already liked this answer
            var userLike = answer.AnsLikes.FirstOrDefault(like => like.UserName == User.Identity.Name);

            if (userLike != null)
            {
                // User has already liked, toggle the like
                if (userLike.Likes == 1)
                {
                    userLike.Likes = 0;
                }
                else { userLike.Likes = 1;
                    userLike.DisLikes = 0;
                } // Toggle the like by negating its current value
            }
            else
            {
                // Add a like for the current user
                answer.AnsLikes.Add(new AnsLike
                {
                    UserName = User.Identity.Name,
                    answerId = answerId,
                    Likes = 1
                });
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the discussion page or wherever you prefer
            return RedirectToAction("SeeMore", new { id = answer.QuestionId });
        }

        [HttpPost]
        public async Task<IActionResult> DislikeAnswer(int answerId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to a login page or return an error
                return RedirectToAction("Login", "Account"); // Replace with your login action and controller
            }

            // Find the answer by ID
            var answer = await _context.Community
                .Include(a => a.AnsLikes)
                .SingleOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                return NotFound(); // Answer not found
            }

            // Check if the user has already liked this answer
            var userLike = answer.AnsLikes.FirstOrDefault(like => like.UserName == User.Identity.Name);

            if (userLike != null)
            {
                // User has already liked, toggle the like
                if (userLike.DisLikes == 1)
                {
                    userLike.DisLikes = 0;
                }
                else
                {
                    userLike.DisLikes = 1;
                    userLike.Likes = 0; // Remove the like if it exists
                }
            }
            else
            {
                // Add a like for the current user
                answer.AnsLikes.Add(new AnsLike
                {
                    UserName = User.Identity.Name,
                    answerId = answerId,
                    DisLikes = 1
                });
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the discussion page or wherever you prefer
            return RedirectToAction("SeeMore", new { id = answer.QuestionId });
        }


    }
}
