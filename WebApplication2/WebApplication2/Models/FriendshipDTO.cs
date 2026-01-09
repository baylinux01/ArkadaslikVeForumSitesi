using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class FriendshipDTO
    {
        //[Key]
        public int Id { get; set; }
        public UserDTO? Friend1 { get; set; }
        public UserDTO? Friend2 { get; set; }

        public static FriendshipDTO? ConvertTo(Friendship? u)
        {
            if(u == null) return null;
            return new FriendshipDTO(u);
        }

         public static List<FriendshipDTO>? ConvertTo(List<Friendship>? u)
        {
            if(u == null) return null;
            List<FriendshipDTO> list= new List<FriendshipDTO>();

            foreach(Friendship ag in u)
            {
                list.Add(FriendshipDTO.ConvertTo(ag));
            }
            return list;
        }

        public FriendshipDTO(Friendship f)
        {
            this.Id=f.Id;
            this.Friend1=new UserDTO(f.Friend1);
            this.Friend2=new UserDTO(f.Friend2);
        }
    }
}