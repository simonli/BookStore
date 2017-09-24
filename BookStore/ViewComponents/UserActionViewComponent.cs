using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ViewComponents
{
    public class UserActionViewComponent : ViewComponent
    {
        private readonly BookStoreContext _context;

        public UserActionViewComponent(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<string> InvokeAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated) return "";
            var loginUser =
                await _context.Users.FirstOrDefaultAsync(m => m.Username == HttpContext.User.Identity.Name);
            var todayDownloadCount = _context.ActionLogs
                .Where(x => x.User == loginUser).Where(x => x.CreateTime >= DateTime.Today)
                .Count(x => x.Taxonomy == TaxonomyEnum.Download);
            var todayPushCount = _context.ActionLogs
                .Where(x => x.User == loginUser).Where(x => x.CreateTime >= DateTime.Today)
                .Count(x => x.Taxonomy == TaxonomyEnum.Push);
            return
                $"用户名：{loginUser.Username}, 剩余积分：{loginUser.PointCount}, 今日下载：{todayDownloadCount}, 今日推送：{todayPushCount}";
        }
    }
}