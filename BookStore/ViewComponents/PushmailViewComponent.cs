using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
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

        public async Task<ViewViewComponentResult> InvokeAsync(int editionId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loginUser =
                    await _context.Users.FirstOrDefaultAsync(m => m.Username == HttpContext.User.Identity.Name);
                var pushSettings = await _context.PushSettings.Where(x => x.User == loginUser)
                    .OrderByDescending(x => x.IsDefault).ToListAsync();
                var vm = new PushSettingsComponentViewModel
                {
                    PushSettings = pushSettings,
                    EditionId = editionId,
                    LoginedUser = loginUser
                };

                return View(vm);
            }
            return View();
        }
    }
}