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
            return new TopicDTO(u);
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

        public TopicDTO(Topic t)
        {
            this.Id=t.Id;
            this.Title=t.Title;
            this.Owner=new UserDTO(t.Owner);
        }
    }
}