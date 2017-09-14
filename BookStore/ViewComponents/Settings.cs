using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using BookStore.Models;
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

        public async Task<IViewComponentResult> InvokeAsync(User user,string taxonomy)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loginUsername = HttpContext.User.Identity.Name;
                var loginUser = (loginUsername == user.Username)
                    ? user
                    : await _context.Users.FirstOrDefaultAsync(m => m.Username == loginUsername);
                var todayDownloadCount = _context.ActionLogs
                    .Where(x => x.User == loginUser).Where(x => x.CreateTime >= DateTime.Today)
                    .Count(x => x.Taxonomy == TaxonomyEnum.Download);
                var todayPushCount = _context.ActionLogs
                    .Where(x => x.User == loginUser).Where(x => x.CreateTime >= DateTime.Today)
                    .Count(x => x.Taxonomy == TaxonomyEnum.Push);

                var vm = new SettingsComponentViewModel
                {
                    User = loginUser,
                    TodayDownloadCount = todayDownloadCount,
                    TodayPushCount = todayPushCount,
                    Taxonomy = taxonomy
                };
                return View(vm);
            }
            return View("Blank");
        }
    }
}