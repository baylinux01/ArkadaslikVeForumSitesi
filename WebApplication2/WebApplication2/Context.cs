using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;


namespace WebApplication2
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer("Server=localhost; database=arfosit; user id=sa; password=Deneme.01; TrustServerCertificate=True;");
        } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlagramGroup>()
            .HasOne(e => e.Owner)
            .WithMany();

            modelBuilder.Entity<AlagramGroup>()
            .HasMany(e => e.Members)
            .WithMany(e => e.MemberedGroups);

            modelBuilder.Entity<AlagramGroup>()
            .HasMany(e => e.BannedUsers)
            .WithMany(e=>e.BanningGroups);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BannedUsers)
                .WithMany(e => e.BanningUsers);

            // modelBuilder.Entity<User>()
            //     .HasMany(e => e.MemberedGroups)
            //     .WithOne(e => e.Owner)
            //     .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<AlagramGroup>()
                .HasOne(e => e.Owner)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Comment>()
                .HasOne(e => e.Topic)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
                
            
            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade); // veya .NoAction();

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); // veya .NoAction();
            
            modelBuilder.Entity<Friendship>()
                .HasOne(fr => fr.Friend1)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade); // veya .NoAction();

            modelBuilder.Entity<Friendship>()
                .HasOne(fr => fr.Friend2)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); // veya .NoAction();
            
            modelBuilder.Entity<Message>()
                .HasOne(fr => fr.MessageSender)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade); // veya .NoAction();

            modelBuilder.Entity<Message>()
                .HasOne(fr => fr.MessageReceiver)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); // veya .NoAction();
            
            modelBuilder.Entity<AlagramComment>()
                .HasOne(fr => fr.Owner)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade); // veya .NoAction();

            modelBuilder.Entity<AlagramComment>()
                .HasOne(fr => fr.Group)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); // veya .NoAction();
            
            modelBuilder.Entity<Comment>()
                .HasOne(fr => fr.Owner)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade); // veya .NoAction();

            modelBuilder.Entity<Comment>()
                .HasOne(fr => fr.QuotedComment)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); // veya .NoAction();

            
        }

        public DbSet<User> MyUsers { get; set; }
        public DbSet<Product> MyProducts { get; set; }
        public DbSet<Product2> MyProducts2 { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AlagramGroup> AlagramGroups { get; set; }
        public DbSet<AlagramComment> AlagramComments { get; set; }
    }


}
