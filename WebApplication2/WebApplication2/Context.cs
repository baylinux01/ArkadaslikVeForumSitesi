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
            optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=postgres; Username=postgres;Password=root");
            //String mysqlconstring = "server=localhost:3306;user id=root;database=sys;allowuservariables=True;password=mypassword;persistsecurityinfo=True;";
            //optionsBuilder.UseMySql(mysqlconstring);
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
            .WithMany(e=>e.BanningUsers);

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
