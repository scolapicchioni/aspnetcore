using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PhotoSharingApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.Cache
{
    public static class MemoryCacheExtensions
    {
        public static async Task<PhotoFile> GetPhotoFile(this IMemoryCache memoryCache, int id, ApplicationDbContext context)
        {    
            string photoImageIdKey = $"photo-image-{id}";
            Cache.PhotoFile photo;
            if (!memoryCache.TryGetValue(photoImageIdKey, out photo))
            {
                photo = await context.Photos
                    .Where(m => m.Id == id)
                    .Select(p => new PhotoFile() { File = p.PhotoFile, ImageMimeType = p.ImageMimeType })
                    .FirstOrDefaultAsync();
                if (photo != null)
                    memoryCache.Set(photoImageIdKey, photo,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }
            return photo;
        }

    }
}
