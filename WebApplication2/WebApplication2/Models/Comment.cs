using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; } 

        public User? Owner { get; set; }

        public string Content { get; set; }
        public Topic? Topic { get; set; }

        public Comment? QuotedComment { get; set; }
    }
}
