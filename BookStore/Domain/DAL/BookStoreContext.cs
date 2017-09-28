using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Domain.DAL
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookEdition> BookEditions { get; set; }
        public DbSet<BookEditionComment> BookEditionComments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PushSetting> PushSettings { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<UserPointLog> UserPointLogs { get; set; }
        public DbSet<AppKey> AppKeys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasIndex(x => x.Title);
            modelBuilder.Entity<Book>().HasIndex(x => x.Author);
            modelBuilder.Entity<Book>().HasIndex(x => x.Isbn);
            modelBuilder.Entity<Book>().HasIndex(x => x.Publisher);

            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<AppKey>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<BookTag>()
                .HasOne(x => x.Book)
                .WithMany(x => x.BookTags)
                .HasForeignKey(k => k.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.BookTags)
                .HasForeignKey(k => k.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(x => x.IsDelete).HasDefaultValue(0);
            modelBuilder.Entity<Book>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
            modelBuilder.Entity<Book>().Property(x => x.IsDelete).HasDefaultValue(0);
            modelBuilder.Entity<BookEdition>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
            modelBuilder.Entity<BookEditionComment>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
            modelBuilder.Entity<PushSetting>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
            modelBuilder.Entity<PushSetting>().Property(x => x.IsDefault).HasDefaultValue(0);
            modelBuilder.Entity<ActionLog>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserPointLog>().Property(x => x.CreateTime).ValueGeneratedOnAdd();
        }
    }
}