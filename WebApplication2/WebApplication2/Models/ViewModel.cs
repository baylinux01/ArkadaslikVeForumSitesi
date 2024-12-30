namespace WebApplication2.Models
{
    public class ViewModel
    {
        public User? User { get; set; }

        public List<User>? Users { get; set; }

        public Product? Product { get; set; }

        public List<Product>? Products { get; set; }

        public Product2? Product2 { get; set; }

        public List<Product2>? Products2 { get; set; }

        public FriendRequest? FriendRequest { get; set; }

        public List<FriendRequest>? FriendRequests { get; set; }
        public Friendship? Friendship { get; set; }

        public List<Friendship>? Friendships { get; set; }

        public Message? Message { get; set; }

        public List<Message>? Messages { get; set; }

        public Topic? Topic { get; set; }

        public List<Topic>? Topics { get; set; }

        public Comment? Comment { get; set; }

        public List<Comment>? Comments { get; set; }

        public AlagramGroup? AlagramGroup { get; set; }

        public List<AlagramGroup>? AlagramGroups { get; set; }

        public AlagramComment? AlagramComment { get; set; }

        public List<AlagramComment>? AlagramComments { get; set; }

    }
}
