using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class AlagramCommentDTO
    {
        //[Key]
        public int Id { get; set; }
        public UserDTO Owner { get; set; }
        public AlagramGroupDTO Group { get; set; }
        public string Content { get; set; }
        public AlagramCommentDTO? QuotedComment { get; set; }

        public static AlagramCommentDTO? ConvertTo(AlagramComment? u)
        {
            if(u == null) return null;
            return new AlagramCommentDTO(u);
        }

        public static List<AlagramCommentDTO>? ConvertTo(List<AlagramComment>? u)
        {
            if(u == null) return null;
            List<AlagramCommentDTO> list= new List<AlagramCommentDTO>();

            foreach(AlagramComment ag in u)
            {
                list.Add(AlagramCommentDTO.ConvertTo(ag));
            }
            return list;
        }

        public AlagramCommentDTO(AlagramComment ac) : this (ac,100)
        {
            
            
        }
        public AlagramCommentDTO(AlagramComment ac,int depth)
        {
           this.Id=ac.Id;
           this.Owner=new UserDTO(ac.Owner);
           this.Group=new AlagramGroupDTO(ac.Group);
           this.Content=ac.Content;
           
           
           if(ac.QuotedComment!=null && depth>0)
            {
                
                this.QuotedComment=
                        new AlagramCommentDTO(ac.QuotedComment,depth-1);
            }
            
        }
        

    }
}