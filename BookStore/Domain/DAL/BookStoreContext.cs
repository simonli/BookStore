﻿using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Domain.DAL
{
    public class BookStoreContext:DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options): base(options){ }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookEdition> BookEditions { get; set; }
        public DbSet<BookEditionComment> BookEditionComments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PushSetting> PushSettings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
