using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database_configuration
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        // Entiteter i modellen
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<SubTopic> SubTopics { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<InfoTopic> InfoTopics { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Like> Likes { get; set; }

        // Fluent API - diverse konfigurering
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Begrensinger på antall tegn
            modelBuilder.Entity<Post>().Property(p => p.Title).HasMaxLength(100);
            modelBuilder.Entity<Post>().Property(p => p.Content).HasMaxLength(4000);
            modelBuilder.Entity<Comment>().Property(c => c.Content).HasMaxLength(4000);

            // En bruker kan ha mange poster
            modelBuilder.Entity<User>()
                .HasMany<Post>(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Sett poster til null om bruker slettes

            // En bruker kan ha mange svar
            modelBuilder.Entity<User>()
                .HasMany<Comment>(u => u.Comments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Sett kommentarer til null om bruker slettes

            // En post kan ha mange svar
            modelBuilder.Entity<Post>()
                .HasMany<Comment>(p => p.Comments)
                .WithOne(a => a.Post)
                .HasForeignKey(a => a.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Slett alle svar om en post slettes

            // Et tema kan ha mange undertemaer
            modelBuilder.Entity<Topic>()
                .HasMany<SubTopic>(t => t.SubTopics)
                .WithOne(s => s.Topic)
                .HasForeignKey(s => s.TopicId)
                .OnDelete(DeleteBehavior.Cascade); // Slett alle undertemaer om et tema slettes

            // Et undertema kan ha mange poster
            modelBuilder.Entity<SubTopic>()
                .HasMany<Post>(s => s.Posts)
                .WithOne(p => p.SubTopic)
                .HasForeignKey(p => p.SubTopicId)
                .OnDelete(DeleteBehavior.Cascade); // Slett alle poster om et undertema slettes
        }
    }
}
