using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Data;
using PhotoSharingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace PhotoSharingApplication.ViewComponents
{
    public class PhotoGalleryViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        public PhotoGalleryViewComponent(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int number = 0) {
            List<Photo> photos;

            if (number == 0)
            {
                photos = await _context.Photos.Include(p=>p.Owner).ToListAsync();
            }
            else
            {
                photos = await (from p in _context.Photos
                          orderby p.CreatedDate descending
                          select p).Include(p=>p.Owner).Take(number).ToListAsync();
            }

            return View(photos);
        }
    }
}