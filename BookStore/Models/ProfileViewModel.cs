using BookStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ProfileViewModel<T> 
    {
        public User User { get; set; }
        public List<T> ActionLogList { get; set; }
    }

    
}