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
            return new FriendRequestDTO(u);
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

        public FriendRequestDTO(FriendRequest fr)
        {
            this.Id=fr.Id;
            this.Sender=new UserDTO(fr.Sender);
            this.Receiver=new UserDTO(fr.Receiver);

        }
    }
}