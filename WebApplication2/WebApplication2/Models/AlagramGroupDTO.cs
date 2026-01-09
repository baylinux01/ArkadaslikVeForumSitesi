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
            return new AlagramGroupDTO(u);
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

        public AlagramGroupDTO()
        {
            this.Members = new List<UserDTO>();
            this.BannedUsers = new List<UserDTO>();
        }

        public AlagramGroupDTO(AlagramGroup group)
        {
            this.Members = new List<UserDTO>();
            this.BannedUsers = new List<UserDTO>();

            this.Id=group.Id;
            this.Name=group.Name;
            this.Owner=new UserDTO(group.Owner);
            if(group.Members!=null)
            {
                    foreach(User u in group.Members)
                {
                    this.Members.Add(new UserDTO(u));
                }
            }
            if(group.BannedUsers!=null)
            {
                foreach(User u in group.BannedUsers)
                {
                    this.BannedUsers.Add(new UserDTO(u));
                }
            }
        }
    }
}