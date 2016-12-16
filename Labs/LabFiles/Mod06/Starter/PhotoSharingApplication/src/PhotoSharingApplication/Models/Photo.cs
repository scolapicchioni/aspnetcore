using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoSharingApplication.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name ="Picture")]
        public byte[] PhotoFile { get; set; }
        public string ImageMimeType { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name ="Created Date")]
        //[DisplayFormat(DataFormatString ="{0:MM/dd/yy")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
