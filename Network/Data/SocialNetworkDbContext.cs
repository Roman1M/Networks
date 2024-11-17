using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class SocialNetworkDbContext : IdentityDbContext<User>
    {
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options) : base(options) { }

        public SocialNetworkDbContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SocialNetwork;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Like>()
                .HasOne(i => i.User)
                .WithMany(i => i.Likes)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriend",
                    j => j.HasOne<User>().WithMany().HasForeignKey("FriendId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
                );
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
