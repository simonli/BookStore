using BookStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<ActionLog> ActionLogList { get; set; }
    }
}