using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class UserDTO
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

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        //public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;

        public bool IsOwner { get; set; } = false;

        public List<AlagramGroupDTO>? MemberedGroups { get; set; }

        public List<AlagramGroupDTO>? BanningGroups { get; set; }

        public List<UserDTO>? BannedUsers { get; set; }

        public List<UserDTO>? BanningUsers { get; set; }

        

        public static UserDTO? ConvertTo(User? u)
        {
            if(u == null) return null;
            return new UserDTO(u);
        }

         public static List<UserDTO>? ConvertTo(List<User>? u)
        {
            if(u == null) return null;
            List<UserDTO> list= new List<UserDTO>();

            foreach(User ag in u)
            {
                list.Add(UserDTO.ConvertTo(ag));
            }
            return list;
        }


        public UserDTO(User u) : this(u,3)
        {
                

        }

        public UserDTO(User u,int depth)
        {
            this.MemberedGroups=new List<AlagramGroupDTO>();
            this.BanningGroups=new List<AlagramGroupDTO>();

            this.BannedUsers=new List<UserDTO>();
            this.BanningUsers=new List<UserDTO>();

            this.Id=u.Id;
            this.Name=u.Name;
            this.Surname=u.Surname;
            this.Username=u.Username;
            this.Phonenumber=u.Phonenumber;
            this.IsAdmin=u.IsAdmin;
            this.IsOwner=u.IsOwner;

            if(u.MemberedGroups!=null)
            {
                foreach(AlagramGroup ag in u.MemberedGroups)
                {
                    this.MemberedGroups.Add(new AlagramGroupDTO(ag));
                }
            }
            if(u.BanningGroups!=null)
            {
                foreach(AlagramGroup ag in u.BanningGroups)
                {
                    this.BanningGroups.Add(new AlagramGroupDTO(ag));
                }
            }
            
                
                if(u.BannedUsers!=null && u.BannedUsers.Count>0
                &&depth>0)
                {
                    foreach(User ag in u.BannedUsers)
                    {
                        if(ag!=null && depth>0)
                        {
                            this.BannedUsers.Add(new UserDTO(ag,depth-1));
                        }
                    }
                }
                if(u.BanningUsers!=null && u.BanningUsers.Count>0
                && depth>0)
                {
                    foreach(User ag in u.BanningUsers)
                    {
                        if(ag!=null && depth>0)
                        {
                            this.BanningUsers.Add(new UserDTO(ag,depth-1));
                        }
                    }
                }
            



        }


    
    }
}