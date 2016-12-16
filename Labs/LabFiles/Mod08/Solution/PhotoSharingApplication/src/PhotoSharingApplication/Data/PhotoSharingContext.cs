using Microsoft.EntityFrameworkCore;
using PhotoSharingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.Data
{
    public class PhotoSharingContext: DbContext
    {
        //public PhotoSharingContext()
        //{

        //}
        public PhotoSharingContext(DbContextOptions<PhotoSharingContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
