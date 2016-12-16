using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PhotoSharingApplication.Data;

namespace PhotoSharingApplication.Migrations
{
    [DbContext(typeof(PhotoSharingContext))]
    partial class PhotoSharingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PhotoSharingApplication.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<int>("PhotoId");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PhotoSharingApplication.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("ImageMimeType");

                    b.Property<byte[]>("PhotoFile");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PhotoSharingApplication.Models.Comment", b =>
                {
                    b.HasOne("PhotoSharingApplication.Models.Photo", "Photo")
                        .WithMany("Comments")
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
