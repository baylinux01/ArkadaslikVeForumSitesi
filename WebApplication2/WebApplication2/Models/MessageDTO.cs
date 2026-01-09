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
            return MessageDTO.ConvertTo(u,3);
        }

        public static MessageDTO? ConvertTo(Message? u,int depth)
        {
            if(u == null|| depth<=0) return null;
            return new MessageDTO(u,depth);
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


        private MessageDTO(Message m,int depth)
        {
            this.Id=m.Id;
            this.MessageSender=UserDTO.ConvertTo(m.MessageSender,depth-1);
            this.MessageReceiver=UserDTO.ConvertTo(m.MessageReceiver,depth-1);
            this.MessageContent=m.MessageContent;
            this.IsRead=m.IsRead;
        }

         public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(MessageDTO))
                return false;

            var other = (MessageDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }

    }
}
