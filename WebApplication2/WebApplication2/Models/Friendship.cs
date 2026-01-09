using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }
        public User? Friend1 { get; set; }
        public User? Friend2 { get; set; }
    }
}
