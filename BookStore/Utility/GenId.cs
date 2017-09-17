using System;
using System.Linq;
using BookStore.Domain.DAL;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Utility
{
    public class GenId
    {
        public static long NewId(string name)
        {
            var options = new DbContextOptionsBuilder<BookStoreContext>()
                .UseSqlite("Data Source=bookstore.db")
                .Options;
        
            using (var context = new BookStoreContext(options))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var appkey = context.AppKeys.FirstOrDefault(x => x.Name == name);
                        var maxId = appkey.MaxId;
                        var newMaxId = maxId + 1;
                        appkey.MaxId = newMaxId;
                        context.SaveChanges();
                        transaction.Commit();
                        return newMaxId;
                    }
                    catch (Exception)
                    {
                        // TODO: Handle failure
                        return 0;
                    }
                }
            }
        }
    }
}