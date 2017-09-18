using System;
using System.Linq;
using BookStore.Domain.DAL;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Utility
{
    public class GenId
    {
        private BookStoreContext _context;

        public GenId(BookStoreContext context)
        {
            _context = context;
        }

        public static long NewId(string name)
        {
            var options = new DbContextOptionsBuilder<BookStoreContext>()
                .UseSqlite("Data Source=bookstore.db")
                .Options;

            using (var context = new BookStoreContext(options))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var appKeys = context.AppKeys.FirstOrDefault(x => x.Name == name);

                    long newMaxId;
                    if (appKeys == null)
                    {
                        const long initValue = 60000000;
                        appKeys = new Domain.Models.AppKey
                        {
                            MaxId = initValue,
                            Name = name
                        };
                        context.AppKeys.Add(appKeys);
                        newMaxId = initValue;
                    }
                    else
                    {
                        newMaxId = appKeys.MaxId + 1;
                        appKeys.MaxId = newMaxId;
                    }
                    context.SaveChanges();
                    transaction.Commit();
                    return newMaxId;
                }
            }
        }
    }
}