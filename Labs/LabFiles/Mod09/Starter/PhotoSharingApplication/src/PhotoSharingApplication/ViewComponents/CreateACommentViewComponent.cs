using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Data;
using PhotoSharingApplication.Models;
using System.Threading.Tasks;

namespace PhotoSharingApplication.ViewComponents
{
    public class CreateACommentViewComponent : ViewComponent
    {
        private PhotoSharingContext _context;
        public CreateACommentViewComponent(PhotoSharingContext context)
        {
            this._context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int PhotoId)
        {
            //Create the new comment
            Comment newComment = new Comment();
            newComment.PhotoId = PhotoId;

            ViewBag.PhotoId = PhotoId;
            return View(newComment);
        }
    }
}
