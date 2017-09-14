using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ViewComponents
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly BookStoreContext _context;

        public ProfileViewComponent(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loginUsername = HttpContext.User.Identity.Name;
                var isLogined = string.Equals(user.Username, loginUsername, StringComparison.CurrentCultureIgnoreCase);
               

                var todayDownloadCount = await _context.ActionLogs
                    .Where(x => x.User == user).Where(x => x.CreateTime >= DateTime.Today)
                    .CountAsync(x => x.Taxonomy == TaxonomyEnum.Download);
                var todayPushCount = await _context.ActionLogs
                    .Where(x => x.User == user).Where(x => x.CreateTime >= DateTime.Today)
                    .CountAsync(x => x.Taxonomy == TaxonomyEnum.Push);

                var vm = new ProfileComponentViewModel
                {
                    User = user,
                    IsLogined=isLogined,
                    TodayDownloadCount = todayDownloadCount,
                    TodayPushCount = todayPushCount
                };
                return View(vm);
            }
            return View("Blank");
        }
    }
}
