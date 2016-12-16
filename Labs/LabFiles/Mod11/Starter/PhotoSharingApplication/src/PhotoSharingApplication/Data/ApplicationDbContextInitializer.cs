using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
    public static class ApplicationDbContextInitializer
    {
        public static async Task<ApplicationUser> createUser(string userName, UserManager<ApplicationUser> userManager) {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userName };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
            return user;
        }

        public async static Task Seed(this IApplicationBuilder app, string path)
        {
            ApplicationDbContext context = app.ApplicationServices.GetService<ApplicationDbContext>();
            UserManager<ApplicationUser> userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            context.Database.Migrate();

            var fred = await createUser("fred@fred.fr", userManager); 
            var bert = await createUser("bert@bert.be", userManager);
            var sue = await createUser("sue@sue.su", userManager);

            await addPhoto(context, new Photo {
                Title = "Me standing on top of a mountain",
                Description = "I was very impressed with myself",
                ApplicationUserId = fred.Id,
                PhotoFile = getFileBytes($@"{path}\Images\flower.jpg"),
                ImageMimeType = "image/jpeg",
                CreatedDate = DateTime.Today,
                Comments = new List<Comment>(){
                        new Comment {
                            ApplicationUserId = bert.Id,
                            Subject = "A Big Mountain",
                            Body = "That looks like a very high mountain you have climbed"
                        },
                        new Comment {
                            ApplicationUserId = sue.Id,
                            Subject = "So?",
                            Body = "I climbed a mountain that high before breakfast everyday"
                        }
                    }
            });

            await addPhoto(context, new Photo {
                Title = "My New Adventure Works Bike",
                Description = "It's the bees knees!",
                ApplicationUserId = bert.Id,
                PhotoFile = getFileBytes($@"{path}\Images\orchard.jpg"),
                ImageMimeType = "image/jpeg",
                CreatedDate = DateTime.Today,
                Comments = new List<Comment>(){
                        new Comment {
                            ApplicationUserId = fred.Id,
                            Subject = "Jealous",
                            Body = "Wow, that new bike looks great!"
                        }
                    }
            });

            await addPhoto(context, new Photo {
                Title = "View from the start line",
                Description = "I took this photo just before we started over my handle bars.",
                ApplicationUserId = sue.Id,
                PhotoFile = getFileBytes($@"{path}\Images\path.jpg"),
                ImageMimeType = "image/jpeg",
                CreatedDate = DateTime.Today
            });

            await context.SaveChangesAsync();
        }

        private static async Task addPhoto(ApplicationDbContext context, Photo toAdd) {
            var found = await context.Photos.FirstOrDefaultAsync(p => p.Title == toAdd.Title);
            if (found == null) context.Photos.Add(toAdd);
        }

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
