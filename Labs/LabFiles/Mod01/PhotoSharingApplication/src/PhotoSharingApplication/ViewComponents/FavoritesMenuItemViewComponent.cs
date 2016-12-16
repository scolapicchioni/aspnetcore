using Microsoft.AspNetCore.Mvc;
using PhotoSharingApplication.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.ViewComponents
{
    public class FavoritesMenuItemViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<int> favoriteIds = HttpContext.Session.GetOrCreateFavoriteIds();

            if (favoriteIds.Count > 0)
            {
                return View();
            }
            else
            {
                return View("NoFavorites");
            }
        }
    }
}
