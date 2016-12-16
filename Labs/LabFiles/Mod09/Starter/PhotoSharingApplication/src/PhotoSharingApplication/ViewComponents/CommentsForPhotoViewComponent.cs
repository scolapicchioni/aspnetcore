using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSharingApplication.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.ViewComponents
{
    public class CommentsForPhotoViewComponent : ViewComponent
    {
        private PhotoSharingContext _context;
        public CommentsForPhotoViewComponent(PhotoSharingContext context)
        {
            this._context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int photoId)
        {
            //The comments for a particular photo have been requested. Get those comments.
            var comments = await _context.Comments.Where(c => c.PhotoId == photoId).ToListAsync();
            //Save the Id in the ViewBag because we'll need it in the view
            ViewBag.PhotoId = photoId;
            return View(comments);
        }
    }
}
