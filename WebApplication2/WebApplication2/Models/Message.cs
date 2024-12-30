using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WebApplication2.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public User MessageSender { get; set; }

        public User MessageReceiver { get; set; }

        public string MessageContent { get; set; }

        public bool IsRead { get; set; }=false;

    }
}
