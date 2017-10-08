using System.Linq;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;

namespace BookStore.Utility
{
    public class IdGenService
    {
        private readonly BookStoreContext _context;

        public IdGenService(BookStoreContext context)
        {
            _context = context;
        }

        public long NewId(string name)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var appKeys = _context.AppKeys.FirstOrDefault(x => x.Name == name);

                long newMaxId;
                if (appKeys == null)
                {
                    long initValue = AppkeyUtil.GetInitValue(name);
                    appKeys = new AppKey
                    {
                        MaxId = initValue,
                        Name = name
                    };
                    _context.AppKeys.Add(appKeys);
                    newMaxId = initValue;
                }
                else
                {
                    newMaxId = appKeys.MaxId + 1;
                    appKeys.MaxId = newMaxId;
                }
                _context.SaveChanges();
                transaction.Commit();
                return newMaxId;
            }
        }
    }
}