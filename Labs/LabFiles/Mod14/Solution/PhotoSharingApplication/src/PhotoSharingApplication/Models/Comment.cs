using System.ComponentModel.DataAnnotations;

namespace PhotoSharingApplication.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PhotoId { get; set; }
        
        [Required]
        [StringLength(250)]
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public Photo Photo { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
