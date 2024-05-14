using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public User Owner { get; set; }
    }
}
