using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class CommentDTO
    {
        //[Key]
        public int Id { get; set; } 

        public UserDTO? Owner { get; set; }

        public string Content { get; set; }
        public TopicDTO? Topic { get; set; }

        public CommentDTO? QuotedComment { get; set; }

        public static CommentDTO? ConvertTo(Comment? u)
        {
            if(u == null) return null;
            return CommentDTO.ConvertTo(u,3);
        }
        public static CommentDTO? ConvertTo(Comment? u,int depth)
        {
            if(u == null || depth<=0) return null;
            return new CommentDTO(u,depth);
        }

         public static List<CommentDTO>? ConvertTo(List<Comment>? u)
        {
            if(u == null) return null;
            List<CommentDTO> list= new List<CommentDTO>();

            foreach(Comment ag in u)
            {
                list.Add(CommentDTO.ConvertTo(ag));
            }
            return list;
        }

       
        
        private CommentDTO(Comment c,int depth)
        {
            this.Id=c.Id;
            this.Owner=UserDTO.ConvertTo(c.Owner,depth-1);
            this.Content=c.Content;
            this.Topic=TopicDTO.ConvertTo(c.Topic,depth-1);
            if(c.QuotedComment!=null && depth>0)
            {
                this.QuotedComment=new CommentDTO(c.QuotedComment,depth-1);
            }
        }

         public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(CommentDTO))
                return false;

            var other = (CommentDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }
    }
}