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
    public class SettingsViewComponent : ViewComponent
    {
        private readonly BookStoreContext _context;

        public SettingsViewComponent(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loginUsername = HttpContext.User.Identity.Name;
                User loginUser;
                if (user == null)
                {
                    loginUser = await _context.Users.FirstOrDefaultAsync(m => m.Username == loginUsername);
                }
                else
                {
                    loginUser = user.Username == loginUsername ? user : await _context.Users.FirstOrDefaultAsync(m => m.Username ==loginUsername);
                }

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
                    TodayPushCount = todayPushCount
                };
                return  View(vm);
            }
            return View("Blank");
        }


    }
}