using System.Linq;
using BookStore.Domain.Models;
using BookStore.Utility;

namespace BookStore.Domain.DAL
{
    public class DbInitializer
    {
        public static void Initialize(BookStoreContext context)
        {
            if (context.Users.Any())
            {
                return; //DB has
            }
            var user = new User
            {
                Username = "simon",
                Password = Utils.GeneratePassword("cncode"),
                Email = "simonli@188.com"
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}