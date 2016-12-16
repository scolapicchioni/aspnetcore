using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SecurityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SecurityDemo.Data;
using SecurityDemo.AuthorizationHandlers;

namespace SecurityDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _context = context;
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            //User.AddIdentity(new ClaimsIdentity(new Claim[] { new Claim("SimoType", "Value 1") }));
            // ((ClaimsIdentity)User.Identity).AddClaim(new Claim(ClaimTypes.DateOfBirth, new DateTime(1974,02,15).ToString(), ClaimValueTypes.Date, "SIMO_ISSUER"));
            //ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            //user.City

            ViewData["Message"] = "Identity added to the current user";

            return View();
        }

        [Authorize(Policy = PolicyNames.RomeOnly)]
        public IActionResult RomeOnly()
        {
            //User.AddIdentity(new ClaimsIdentity(new Claim[] { new Claim("SimoType", "Value 1") }));
            // ((ClaimsIdentity)User.Identity).AddClaim(new Claim(ClaimTypes.DateOfBirth, new DateTime(1974,02,15).ToString(), ClaimValueTypes.Date, "SIMO_ISSUER"));
            //ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            //user.City

            ViewData["Message"] = "So, you come from Rome, uh?";
            //ApplicationUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            
            return View();
        }

        public async Task<IActionResult> Edit(int photoId)
        {
            Photo photo = _context.Photos.FirstOrDefault(p=>p.Id==photoId);

            if (photo == null)
            {
                return NotFound();
            }

            if (await _authorizationService.AuthorizeAsync(User, photo, "DeletePhoto"))
            {
                return View(photo);
            }
            else
            {
                return new ChallengeResult();
            }
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
