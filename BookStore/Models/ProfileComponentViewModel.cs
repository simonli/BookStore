using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.Models;

namespace BookStore.Models
{
    public class ProfileComponentViewModel
    {
        public User User { get; set; }
        public bool IsLogined { get; set; }
        public int TodayDownloadCount { get; set; }
        public int TodayPushCount { get; set; }
    }
}