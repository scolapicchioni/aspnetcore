using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace PhotoSharingApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("titles/{title}")]
        public IActionResult Titles(string title)
        {
            List<string> titles;

            if (title == null)
            {
                titles = new List<string>();
            }
            else
            {
                titles = _context.Photos
                    .Where(p => p.Title.ToLower().Contains(title.ToLower()))
                    .Select(p => p.Title)
                    .ToList();
            }

            return new ObjectResult(titles);
        }

        [Route("photos/{title}")]
        public IActionResult Photos(string title)
        {
            var photos =  _context.Photos
                    .Include(p=>p.Owner)
                    .Where(p => p.Title.ToLower().Contains(title.ToLower()))
                    .Select(p=> new {
                        id=p.Id,
                        title = p.Title,
                        description = p.Description,
                        createdDate = p.CreatedDate,
                        ownerName = p.Owner.UserName,
                        image = $"data:{p.ImageMimeType};base64,{Convert.ToBase64String(p.PhotoFile)}"
                    })
                    .ToList();

            return new ObjectResult(new { photos = photos });
        }
    }
}