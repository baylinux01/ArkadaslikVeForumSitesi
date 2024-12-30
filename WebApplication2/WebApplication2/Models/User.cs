using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class User
    {
        //string name;
        //string surname;
        //string username;
        //string phonenumber;
        //string password;

        //public string Name { get => name; set => name = value; }
        //public string Surname { get => surname; set => surname = value; }
        //public string Username { get => username; set => username = value; }
        //public string Phonenumber { get => phonenumber; set => phonenumber = value; }
        //public string Password { get => password; set => password = value; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        public string Password { get; set; }
        public List<AlagramGroup>? MemberedGroups { get; set; }

        public List<User>? BannedUsers { get; set; }

        public List<User>? BanningUsers { get; set; }

        public List<AlagramGroup> BanningGroups { get; set; }

        public bool IsAdmin { get; set; } = false;

        public bool IsOwner { get; set; } = false;
        public User()
        {
           
                MemberedGroups = new List<AlagramGroup>();
                BannedUsers = new List<User>();
            

        }

    
    }
}
