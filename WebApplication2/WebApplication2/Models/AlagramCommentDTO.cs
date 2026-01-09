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
            return AlagramCommentDTO.ConvertTo(u,3);
        }
        public static AlagramCommentDTO? ConvertTo(AlagramComment? u,int depth)
        {
            if(u == null|| depth<=0) return null;
            return new AlagramCommentDTO(u,depth);
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

        
        private AlagramCommentDTO(AlagramComment ac,int depth)
        {
           this.Id=ac.Id;
           this.Owner=UserDTO.ConvertTo(ac.Owner,depth-1);
           this.Group=AlagramGroupDTO.ConvertTo(ac.Group,depth-1);
           this.Content=ac.Content;
           
           
           if(ac.QuotedComment!=null && depth>0)
            {
                
                this.QuotedComment=
                        new AlagramCommentDTO(ac.QuotedComment,depth-1);
            }
            
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(AlagramCommentDTO))
                return false;

            var other = (AlagramCommentDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }
        

    }
}