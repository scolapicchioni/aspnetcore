using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Data;
using PhotoSharingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using PhotoSharingApplication.ActionFilters;
using Microsoft.Extensions.Caching.Memory;
using PhotoSharingApplication.Cache;

namespace PhotoSharingApplication.Controllers
{
    [ServiceFilter(typeof(ValueReporter))]
    public class PhotosController : Controller
    {
        private PhotoSharingContext _context;
        private IMemoryCache _memoryCache;
        public PhotosController(PhotoSharingContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View(nameof(PhotosController.Index));
        }

        [Route("[controller]/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.SingleOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(nameof(PhotosController.Details), photo);
        }

        [Route("[controller]/title/{title}")]
        public async Task<IActionResult> DisplayByTitle(string title)
        {
            Photo photo = await _context.Photos.FirstOrDefaultAsync(p => p.Title == title);
            if (photo == null)
            {
                return NotFound();
            }
            return View(nameof(PhotosController.Details), photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            Photo newPhoto = new Photo();
            newPhoto.CreatedDate = DateTime.Today;
            return View("Create", newPhoto);
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedDate,Description,ImageMimeType,Title,UserName")] Photo photo, IFormFile image)
        {
            photo.CreatedDate = DateTime.Today;
            if (!ModelState.IsValid)
            {
                return View(nameof(PhotosController.Create), photo);
            }
            else
            {
                if (image != null)
                {
                    photo.ImageMimeType = image.ContentType;
                    photo.PhotoFile = new byte[image.Length];
                    await image.OpenReadStream().ReadAsync(photo.PhotoFile, 0, (int)image.Length);
                }
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PhotosController.Index));
            }
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.SingleOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.SingleOrDefaultAsync(m => m.Id == id);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetImage(int id)
        {
            string photoImageIdKey = $"photo-image-{id}";
            Cache.PhotoFile photo;
            if (!_memoryCache.TryGetValue(photoImageIdKey, out photo))
            {
                photo = await _context.Photos
                    .Where(m => m.Id == id)
                    .Select(p => new PhotoFile() { File = p.PhotoFile, ImageMimeType = p.ImageMimeType })
                    .FirstOrDefaultAsync();
                if(photo!=null)
                    _memoryCache.Set(photoImageIdKey, photo,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }
            if (photo != null)
                return File(photo.File, photo.ImageMimeType);
            else
                return null;
        }

        public IActionResult SlideShow() {
            throw new NotImplementedException("Not implemented yet");
        }
    }
}