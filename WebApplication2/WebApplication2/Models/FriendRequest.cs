using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
