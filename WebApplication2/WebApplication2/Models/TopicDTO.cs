using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class TopicDTO
    {
        //[Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public UserDTO? Owner { get; set; }


        public static TopicDTO? ConvertTo(Topic? u)
        {
            if(u == null) return null;
            return TopicDTO.ConvertTo(u,3);
        }

         public static TopicDTO? ConvertTo(Topic? u,int depth)
        {
            if(u == null||depth<=0) return null;
            return new TopicDTO(u,depth);
        }

         public static List<TopicDTO>? ConvertTo(List<Topic>? u)
        {
            if(u == null) return null;
            List<TopicDTO> list= new List<TopicDTO>();

            foreach(Topic ag in u)
            {
                list.Add(TopicDTO.ConvertTo(ag));
            }
            return list;
        }

        private TopicDTO(Topic t,int depth)
        {
            this.Id=t.Id;
            this.Title=t.Title;
            this.Owner=UserDTO.ConvertTo(t.Owner,depth-1);
        }
        
         public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(TopicDTO))
                return false;

            var other = (TopicDTO)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            
            return Id.GetHashCode();
        }
    }
}