using System.Text.RegularExpressions;

namespace WebApplication2.Models
{
    public class AlagramGroupDTO
    {
        //[Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDTO? Owner { get; set; }
        public List<UserDTO>? Members {get; set; }

        public List<UserDTO>? BannedUsers { get; set; }

        public static AlagramGroupDTO? ConvertTo(AlagramGroup? u)
        {
            if(u == null) return null;
            return new AlagramGroupDTO(u,3);
        }
        public static AlagramGroupDTO? ConvertTo(AlagramGroup? u,int depth)
        {
            if(u == null|| depth<=0) return null;
            return new AlagramGroupDTO(u,depth);
        }
        public static List<AlagramGroupDTO>? ConvertTo(List<AlagramGroup>? u)
        {
            if(u == null) return null;
            List<AlagramGroupDTO> list= new List<AlagramGroupDTO>();

            foreach(AlagramGroup ag in u)
            {
                list.Add(AlagramGroupDTO.ConvertTo(ag));
            }
            return list;
        }
        
        // public AlagramGroupDTO()
        // {
        //     this.Members = new List<UserDTO>();
        //     this.BannedUsers = new List<UserDTO>();
        // }
        
        private AlagramGroupDTO(AlagramGroup group,int depth)
        {
            if(group==null || depth<=0)
            {
                return;
            }
            this.Members = new List<UserDTO>();
            this.BannedUsers = new List<UserDTO>();

            this.Id=group.Id;
            this.Name=group.Name;
            this.Owner=UserDTO.ConvertTo(group.Owner,depth-1);
            if(group.Members!=null)
            {
                    foreach(User u in group.Members)
                {
                    this.Members.Add(UserDTO.ConvertTo(u,depth-1));
                }
            }
            if(group.BannedUsers!=null)
            {
                foreach(User u in group.BannedUsers)
                {
                    this.BannedUsers.Add(UserDTO.ConvertTo(u,depth-1));
                }
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(AlagramGroupDTO))
                return false;

            var other = (AlagramGroupDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }
    }
}