using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Utility
{
    public class IdGen
    {
        public static long NewId(string name)
        {
            var options = new DbContextOptionsBuilder<BookStoreContext>()
//                .UseMySql("Server=localhost;User Id=root;Password=cncode;Database=bookstore")
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
                        long initValue = AppkeyUtil.GetInitValue(name);
                        appKeys = new AppKey
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