namespace WebApplication2.Models
{
    public class ViewModel
    {
        public UserDTO? User { get; set; }

        public List<UserDTO>? Users { get; set; }

        public Product? Product { get; set; }

        public List<Product>? Products { get; set; }

        public Product2? Product2 { get; set; }

        public List<Product2>? Products2 { get; set; }

        public FriendRequestDTO? FriendRequest { get; set; }

        public List<FriendRequestDTO>? FriendRequests { get; set; }
        public FriendshipDTO? Friendship { get; set; }

        public List<FriendshipDTO>? Friendships { get; set; }

        public MessageDTO? Message { get; set; }

        public List<MessageDTO>? Messages { get; set; }

        public TopicDTO? Topic { get; set; }

        public List<TopicDTO>? Topics { get; set; }

        public CommentDTO? Comment { get; set; }

        public List<CommentDTO>? Comments { get; set; }

        public AlagramGroupDTO? AlagramGroup { get; set; }

        public List<AlagramGroupDTO>? AlagramGroups { get; set; }

        public AlagramCommentDTO? AlagramComment { get; set; }

        public List<AlagramCommentDTO>? AlagramComments { get; set; }

    }
}
