using System.Threading.Tasks;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ViewComponents
{
    public class NavMenuViewComponent : ViewComponent
    {
        private readonly BookStoreContext _context;

        public NavMenuViewComponent(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginUser = new User();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                loginUser = await _context.Users.FirstOrDefaultAsync(m => m.Username == HttpContext.User.Identity.Name);
            }
            return View(loginUser);
        }
    }
}