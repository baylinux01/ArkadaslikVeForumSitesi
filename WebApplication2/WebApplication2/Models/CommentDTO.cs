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
            return new CommentDTO(u);
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

        public CommentDTO(Comment c): this (c,100)
        {

        }

        public CommentDTO(Comment c,int depth)
        {
            this.Id=c.Id;
            this.Owner=new UserDTO(c.Owner);
            this.Content=c.Content;
            this.Topic=new TopicDTO(c.Topic);
            if(c.QuotedComment!=null && depth>0)
            {
                this.QuotedComment=new CommentDTO(c.QuotedComment,depth-1);
            }
        }
    }
}