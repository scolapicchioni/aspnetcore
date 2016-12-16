using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecurityDemo.Data;
using SecurityDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SecurityDemo.AuthorizationHandlers;

namespace SecurityDemo.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhotosController(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Photos.Include(p => p.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.Include(p=>p.Owner).SingleOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        [Authorize]
        // GET: Photos/Create
        public IActionResult Create()
        {
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                photo.ApplicationUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", photo.ApplicationUserId);
            return View(photo);
        }

        [Authorize]
        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            if (await _authorizationService.AuthorizeAsync(User, photo, PolicyNames.PhotoDelete))
            {
                //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", photo.ApplicationUserId);
                return View(photo);
            }
            else
            {
                return new ChallengeResult();
            }
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,Name")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }
            if (await _authorizationService.AuthorizeAsync(User, photo, PolicyNames.PhotoEdit)) {
                if (ModelState.IsValid) {
                    try
                    {
                        photo.ApplicationUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
                        _context.Update(photo);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PhotoExists(photo.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index");
                }
                //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", photo.ApplicationUserId);
                return View(photo);
            }
            else {
                return Challenge();
            }
        }

        // GET: Photos/Delete/5
        [Authorize]
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
            if (await _authorizationService.AuthorizeAsync(User, photo, PolicyNames.PhotoDelete))
            {
                return View(photo);
            }
            else {
                return Challenge();
            }
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.SingleOrDefaultAsync(m => m.Id == id);
            if (await _authorizationService.AuthorizeAsync(User, photo, PolicyNames.PhotoDelete))
            {
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else {
                return Challenge();
            }
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}
