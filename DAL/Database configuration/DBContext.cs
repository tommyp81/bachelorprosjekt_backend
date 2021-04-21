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
                .OnDelete(DeleteBehavior.SetNull); // Sett poster sin userId til null om bruker slettes

            // En bruker kan ha mange svar
            modelBuilder.Entity<User>()
                .HasMany<Comment>(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Sett kommentarer sin userId til null om bruker slettes

            // En bruker kan ha mange dokumenter
            modelBuilder.Entity<User>()
                .HasMany<Document>(u => u.Documents)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Sett dokumenter sin userId til null om bruker slettes

            // En bruker kan ha mange videoer
            modelBuilder.Entity<User>()
                .HasMany<Video>(u => u.Videos)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Sett videoer sin userId til null om bruker slettes
        }
    }
}
