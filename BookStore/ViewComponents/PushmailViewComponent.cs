using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ViewComponents
{
    public class PushmailViewComponent : ViewComponent
    {
        private readonly BookStoreContext _context;

        public PushmailViewComponent(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<string> InvokeAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loginUser =
                    await _context.Users.FirstOrDefaultAsync(m => m.Username == HttpContext.User.Identity.Name);

                return View();
            }
            return View();
        }
    }
}