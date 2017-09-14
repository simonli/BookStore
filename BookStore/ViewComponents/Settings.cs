using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ViewComponents
{
    public class Settings : ViewComponent
    {
        private readonly BookStoreContext _context;

        public Settings(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int abc)
        {
//            var user = u ?? await _context.Users.FirstOrDefaultAsync(m =>
//                           m.Username == HttpContext.User.Identity.Name);
            return View();
        }
    }
}