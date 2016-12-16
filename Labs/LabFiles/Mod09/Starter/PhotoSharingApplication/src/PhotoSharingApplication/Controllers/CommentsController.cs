using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Models;
using PhotoSharingApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace PhotoSharingApplication.Controllers
{
    public class CommentsController : Controller
    {
        private PhotoSharingContext _context;
        
        public CommentsController(PhotoSharingContext Context)
        {
            _context = Context;
        }

        //POST: This action creates the comment when the AJAX comment create tool is used
        [HttpPost]
        public async Task<IActionResult> Create(Comment comment, int PhotoId)
        {
            //Save the new comment
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            //Return the view component with the new list of comments
            return ViewComponent("CommentsForPhoto", new { PhotoId = PhotoId});
            
        }

        //
        // GET: /Comment/Delete/5
        public async Task<IActionResult> Delete(int id = 0)
        {
            Comment comment = await _context.Comments.SingleOrDefaultAsync(c=>c.Id == id);
            ViewBag.PhotoId = comment.PhotoId;
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PhotosController.Details), "Photos", new { id = comment.PhotoId });
        }
    }
}

