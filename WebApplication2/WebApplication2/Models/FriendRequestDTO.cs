using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class FriendRequestDTO
    {
        //[Key]
        public int Id { get; set; }
        public UserDTO? Sender { get; set; }
        public UserDTO? Receiver { get; set; }

        public static FriendRequestDTO? ConvertTo(FriendRequest? u)
        {
            if(u == null) return null;
            return FriendRequestDTO.ConvertTo(u,3);
        }

        public static FriendRequestDTO? ConvertTo(FriendRequest? u,int depth)
        {
            if(u == null||depth<=0) return null;
            return new FriendRequestDTO(u,depth);
        }

         public static List<FriendRequestDTO>? ConvertTo(List<FriendRequest>? u)
        {
            if(u == null) return null;
            List<FriendRequestDTO> list= new List<FriendRequestDTO>();

            foreach(FriendRequest ag in u)
            {
                list.Add(FriendRequestDTO.ConvertTo(ag));
            }
            return list;
        }

        private FriendRequestDTO(FriendRequest fr,int depth)
        {
            this.Id=fr.Id;
            this.Sender=UserDTO.ConvertTo(fr.Sender,depth-1);
            this.Receiver=UserDTO.ConvertTo(fr.Receiver,depth-1);

        }
         public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(FriendRequestDTO))
                return false;

            var other = (FriendRequestDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }
    }
}