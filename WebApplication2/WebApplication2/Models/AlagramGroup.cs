using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class AlagramGroup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public User? Owner { get; set; }
        public List<User>? Members {get; set; }

        public List<User>? BannedUsers { get; set; }

        public AlagramGroup()
        {
            Members = new List<User>();
            BannedUsers = new List<User>();
        }
    }
}
