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
            return FriendshipDTO.ConvertTo(u,3);
        }

        public static FriendshipDTO? ConvertTo(Friendship? u,int depth)
        {
            if(u == null|| depth<=0) return null;
            return new FriendshipDTO(u,depth);
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

        private FriendshipDTO(Friendship f,int depth)
        {
            if(depth<=0)
            {
                return;
            }
            this.Id=f.Id;
            this.Friend1=UserDTO.ConvertTo(f.Friend1,depth-1);
            this.Friend2=UserDTO.ConvertTo(f.Friend2,depth-1);
        }
         public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(FriendshipDTO))
                return false;

            var other = (FriendshipDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }
    }
}