namespace WebApplication2.Models
{
    public sealed class ConverterToDTO
    {   
        public AlagramCommentDTO? ConvertTo(AlagramComment model)
        {
            int depth=3;
            
            
             AlagramCommentDTO dto=AlagramCommentDTO.ConvertTo(model,depth);
            
             return dto;
        }
         public AlagramGroupDTO? ConvertTo(AlagramGroup model)
        {
            int depth=3;
            
            
             AlagramGroupDTO dto=AlagramGroupDTO.ConvertTo(model,depth);
            
             return dto;
        }
         public CommentDTO? ConvertTo(Comment model)
        {
            int depth=3;
            
            
             CommentDTO dto=CommentDTO.ConvertTo(model,depth);
            
             return dto;
        }
        public FriendRequestDTO? ConvertTo(FriendRequest model)
        {
            int depth=3;
            
            
             FriendRequestDTO dto=FriendRequestDTO.ConvertTo(model,depth);
            
             return dto;
        }
        public FriendshipDTO? ConvertTo(Friendship model)
        {
            int depth=3;
            
            
             FriendshipDTO dto=FriendshipDTO.ConvertTo(model,depth);
            
             return dto;
        }

         public MessageDTO? ConvertTo(Message model)
        {
            int depth=3;
            
            
             MessageDTO dto=MessageDTO.ConvertTo(model,depth);
            
             return dto;
        }
         public TopicDTO? ConvertTo(Topic model)
        {
            int depth=3;
            
            
             TopicDTO dto=TopicDTO.ConvertTo(model,depth);
            
             return dto;
        }
        public UserDTO? ConvertTo(User model)
        {
            int depth=3;
            
            
             UserDTO dto=UserDTO.ConvertTo(model,depth);
            
             return dto;
        }

        public List<AlagramCommentDTO>? ConvertTo(List<AlagramComment> model)
        {
            
            if(model == null) return null;
            List<AlagramCommentDTO> list= new List<AlagramCommentDTO>();

            foreach(AlagramComment ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }
         public List<AlagramGroupDTO>? ConvertTo(List<AlagramGroup> model)
        {
            if(model == null) return null;
            List<AlagramGroupDTO> list= new List<AlagramGroupDTO>();

            foreach(AlagramGroup ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
            
            
             
        }
         public List<CommentDTO>? ConvertTo(List<Comment> model)
        {
            if(model == null) return null;
            List<CommentDTO> list= new List<CommentDTO>();

            foreach(Comment ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }
        public List<FriendRequestDTO>? ConvertTo(List<FriendRequest> model)
        {
            if(model == null) return null;
            List<FriendRequestDTO> list= new List<FriendRequestDTO>();

            foreach(FriendRequest ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }
        public List<FriendshipDTO>? ConvertTo(List<Friendship> model)
        {
            if(model == null) return null;
            List<FriendshipDTO> list= new List<FriendshipDTO>();

            foreach(Friendship ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }

         public List<MessageDTO>? ConvertTo(List<Message> model)
        {
            if(model == null) return null;
            List<MessageDTO> list= new List<MessageDTO>();

            foreach(Message ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }
         public List<TopicDTO>? ConvertTo(List<Topic> model)
        {
            if(model == null) return null;
            List<TopicDTO> list= new List<TopicDTO>();

            foreach(Topic ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }
        public List<UserDTO>? ConvertTo(List<User> model)
        {
            if(model == null) return null;
            List<UserDTO> list= new List<UserDTO>();

            foreach(User ag in model)
            {
                list.Add(this.ConvertTo(ag));
            }
            return list;
        }
    }
}