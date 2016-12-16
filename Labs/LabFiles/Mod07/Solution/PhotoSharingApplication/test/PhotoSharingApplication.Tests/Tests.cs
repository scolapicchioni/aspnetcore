using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhotoSharingApplication.Controllers;
using PhotoSharingApplication.Data;
using PhotoSharingApplication.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class PhotoControllerTests
    {
        private static DbContextOptions<PhotoSharingContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<PhotoSharingContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }


        [Fact]
        public void IndexReturnsView()
        {
            var options = CreateNewContextOptions();

            // Run the test against one instance of the context
            using (var context = new PhotoSharingContext(options))
            {
                using (PhotosController controller = new PhotosController(context))
                {
                    var result = (controller.Index()) as ViewResult;
                    Assert.Equal(nameof(controller.Index), result.ViewName);
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetImageReturnsFile(int id)
        {
            var options = CreateNewContextOptions();

            // Run the test against one instance of the context
            using (var context = new PhotoSharingContext(options))
            {
                using (PhotosController controller = new PhotosController(context))
                {
                    var photos = new List<Photo>
                        {
                            new Photo {
                                Title = "Me standing on top of a mountain",
                                Description = "I was very impressed with myself",
                                UserName = "Fred",
                                PhotoFile = new byte[] {255,255,255,255,255,255,255,255},
                                ImageMimeType = "image/jpeg",
                                CreatedDate = DateTime.Today
                            },
                            new Photo {
                                Title = "My New Adventure Works Bike",
                                Description = "It's the bees knees!",
                                UserName = "Fred",
                                PhotoFile = new byte[] {0,0,0,0,0,0,0,0},
                                ImageMimeType = "image/jpeg",
                                CreatedDate = DateTime.Today
                            },
                            new Photo {
                                Title = "View from the start line",
                                Description = "I took this photo just before we started over my handle bars.",
                                UserName = "Sue",
                                PhotoFile = new byte[] {255,255,255,255,0,0,0,0},
                                ImageMimeType = "image/jpeg",
                                CreatedDate = DateTime.Today
                            }
                        };
                    photos.ForEach(s => context.Photos.Add(s));
                    context.SaveChanges();

                    var result = (await controller.GetImage(id)) as ActionResult;

                    Assert.Equal(typeof(FileContentResult), result.GetType());
                }
            }
        }
    }
}
