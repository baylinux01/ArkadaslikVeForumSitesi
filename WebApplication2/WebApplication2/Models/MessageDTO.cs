using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WebApplication2.Models
{
    public class MessageDTO
    {
        //[Key]
        public int Id { get; set; }

        public UserDTO? MessageSender { get; set; }

        public UserDTO? MessageReceiver { get; set; }

        public string MessageContent { get; set; }

        public bool IsRead { get; set; }=false;


        public static MessageDTO? ConvertTo(Message? u)
        {
            if(u == null) return null;
            return new MessageDTO(u);
        }

         public static List<MessageDTO>? ConvertTo(List<Message>? u)
        {
            if(u == null) return null;
            List<MessageDTO> list= new List<MessageDTO>();

            foreach(Message ag in u)
            {
                list.Add(MessageDTO.ConvertTo(ag));
            }
            return list;
        }


        public MessageDTO(Message m)
        {
            this.Id=m.Id;
            this.MessageSender=new UserDTO(m.MessageSender);
            this.MessageReceiver=new UserDTO(m.MessageReceiver);
            this.MessageContent=m.MessageContent;
            this.IsRead=m.IsRead;
        }

    }
}
