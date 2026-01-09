using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class AlagramComment
    {
        [Key]
        public int Id { get; set; }
        public User? Owner { get; set; }
        public AlagramGroup? Group { get; set; }
        public string Content { get; set; }
        public AlagramComment? QuotedComment { get; set; }

    }
}
