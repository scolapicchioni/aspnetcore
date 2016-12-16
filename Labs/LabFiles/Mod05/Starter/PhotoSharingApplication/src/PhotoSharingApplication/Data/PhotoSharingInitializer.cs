using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhotoSharingApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.Data
{
    public static class PhotoSharingInitializer
    {
        public async static Task Seed(this IApplicationBuilder app, string path)
        {
            PhotoSharingContext context = app.ApplicationServices.GetService<PhotoSharingContext>();
            context.Database.Migrate();

            await addPhoto(context, new Photo {
                Title = "Me standing on top of a mountain",
                Description = "I was very impressed with myself",
                UserName = "Fred",
                PhotoFile = getFileBytes($@"{path}\Images\flower.jpg"),
                ImageMimeType = "image/jpeg",
                CreatedDate = DateTime.Today,
                Comments = new List<Comment>(){
                        new Comment {
                            UserName = "Bert",
                            Subject = "A Big Mountain",
                            Body = "That looks like a very high mountain you have climbed"
                        },
                        new Comment {
                            UserName = "Sue",
                            Subject = "So?",
                            Body = "I climbed a mountain that high before breakfast everyday"
                        }
                    }
            });

            await addPhoto(context, new Photo {
                Title = "My New Adventure Works Bike",
                Description = "It's the bees knees!",
                UserName = "Fred",
                PhotoFile = getFileBytes($@"{path}\Images\orchard.jpg"),
                ImageMimeType = "image/jpeg",
                CreatedDate = DateTime.Today,
                Comments = new List<Comment>(){
                        new Comment {
                            UserName = "Fred",
                            Subject = "Jealous",
                            Body = "Wow, that new bike looks great!"
                        }
                    }
            });

            await addPhoto(context, new Photo {
                Title = "View from the start line",
                Description = "I took this photo just before we started over my handle bars.",
                UserName = "Sue",
                PhotoFile = getFileBytes($@"{path}\Images\path.jpg"),
                ImageMimeType = "image/jpeg",
                CreatedDate = DateTime.Today
            });

            await context.SaveChangesAsync();
        }

        private static async Task addPhoto(PhotoSharingContext context, Photo toAdd) {
            var found = await context.Photos.FirstOrDefaultAsync(p => p.Title == toAdd.Title);
            if (found == null) context.Photos.Add(toAdd);
        }

        //This gets a byte array for a file at the path specified
        //The path is relative to the route of the web site
        //It is used to seed images
        private static byte[] getFileBytes(string path){
            FileStream fileOnDisk = new FileStream(path, FileMode.Open);
            byte[] fileBytes;
            using (BinaryReader br = new BinaryReader(fileOnDisk))
            {
                fileBytes = br.ReadBytes((int)fileOnDisk.Length);
            }
            return fileBytes;
        }
    }
}
