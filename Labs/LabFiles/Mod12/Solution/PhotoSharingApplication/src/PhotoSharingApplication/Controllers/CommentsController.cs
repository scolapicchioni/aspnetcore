using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Models;
using PhotoSharingApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PhotoSharingApplication.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;


        public CommentsController(ApplicationDbContext Context, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService)
        {
            _context = Context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        //POST: This action creates the comment when the AJAX comment create tool is used
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Comment comment, int PhotoId)
        {
            comment.ApplicationUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            //Save the new comment
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            //Return the view component with the new list of comments
            return ViewComponent("CommentsForPhoto", new { PhotoId = PhotoId});
            
        }

        //
        // GET: /Comment/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id = 0)
        {
            Comment comment = await _context.Comments
                .Include(c=>c.Photo)
                .ThenInclude(c=>c.Owner)
                .SingleOrDefaultAsync(c=>c.Id == id);
            if (comment == null)
                return NotFound();
            
            if (await _authorizationService.AuthorizeAsync(User, comment, "CommentDelete"))
            {
                ViewBag.PhotoId = comment.PhotoId;
                return View(comment);
            }
            else
                return Challenge();
        }

        //
        // POST: /Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
            if (await _authorizationService.AuthorizeAsync(User, comment, "CommentDelete"))
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(PhotosController.Details), "Photos", new { id = comment.PhotoId });
            }
            else
                return Challenge();
        }
    }
}

